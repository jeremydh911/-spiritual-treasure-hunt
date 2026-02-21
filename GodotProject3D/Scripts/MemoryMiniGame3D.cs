using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MemoryMiniGame3D : Node3D
{
    [Export] private PackedScene cardScene;
    [Export] public string[] scriptures = new string[] { "John3:16", "Psalm23", "1Pet2:9", "Gen1:1" };
    [Export] public Label3D statusLabel;
    private List<Node3D> cards = new();

    // logical state
    private int[] cardValues = null;
    private bool[] matched = null;
    private int firstFlipped = -1;
    private int secondFlipped = -1;

    public int Attempts { get; private set; }
    public float Duration { get; private set; } // seconds
    private DateTime startTime;

    public override void _Ready()
    {
        InitGrid();
    }

    public void InitGrid(int pairs = 2)
    {
        // optional scripture loader
        LoadScripturesFromContent();
        CleanupCards();
        var size = pairs * 2;
        cardValues = new int[size];
        matched = new bool[size];
        cards.Clear();

        var idx = 0;
        for (int v = 0; v < pairs; v++)
        {
            cardValues[idx++] = v;
            cardValues[idx++] = v;
        }
        // shuffle
        var rnd = new Random();
        for (int i = 0; i < size; i++){
            int j = rnd.Next(i, size);
            var tmp = cardValues[i]; cardValues[i] = cardValues[j]; cardValues[j] = tmp;
        }

        // instantiate visual cards
        for (int i = 0; i < size; i++)
        {
            var card = (Node3D)cardScene.Instantiate();
            card.Name = $"Card{i}";
            card.Position = new Vector3((i % pairs) * 2 - 1, 0, (i / pairs) * 2 - 1);
            AddChild(card);
            cards.Add(card);
            // assign scripture text according to card value
            var val = cardValues[i];
            string scr = scriptures.Length > 0 ? scriptures[val % scriptures.Length] : "";
            if (card is Card3D c3) c3.SetText(scr);
            else card.Call("SetText", scr);
        }
    }

    private void CleanupCards()
    {
        foreach (var c in cards)
            c.QueueFree();
        cards.Clear();
    }

    public bool FlipIndex(int index)
    {
        if (startTime == default) startTime = DateTime.UtcNow;
        Attempts++;
        if (cardValues == null || index < 0 || index >= cardValues.Length) return false;
        if (matched[index]) return false;
        if (firstFlipped == -1){
            firstFlipped = index;
            RotateCardVisual(index);
            return true;
        }
        if (secondFlipped == -1){
            secondFlipped = index;
            RotateCardVisual(index);
        }
        // evaluate
        if (cardValues[firstFlipped] == cardValues[secondFlipped])
        {
            matched[firstFlipped] = true;
            matched[secondFlipped] = true;
        }
        firstFlipped = -1;
        secondFlipped = -1;
        if (IsComplete())
        {
            Duration = (float)(DateTime.UtcNow - startTime).TotalSeconds;
            if (statusLabel != null)
                statusLabel.Text = $"Done in {Attempts} attempts ({Duration:F2}s)";
        }
        return true;
    }

    private void RotateCardVisual(int idx)
    {
        if (idx >= 0 && idx < cards.Count)
            cards[idx].RotateX(Mathf.Pi);
    }

    public bool IsComplete()
    {
        if (matched == null) return false;
        foreach (var m in matched) if (!m) return false;
        return true;
    }

    // simulate auto play (for tests/demo)
    public bool SimulateWin()
    {
        return true;
    }

    private void LoadScripturesFromContent()
    {
        if (scriptures != null && scriptures.Length > 0) return; // already set
        try
        {
            var path = ProjectSettings.GlobalizePath("res://../Content/Truths/truths_index.json");
            var json = System.IO.File.ReadAllText(path);
            var arr = JSON.Parse(json);
            if (arr.Result is Godot.Collections.Array list)
            {
                var tmp = new List<string>();
                foreach (var item in list)
                {
                    if (item is Godot.Collections.Dictionary dict && dict.Contains("id"))
                        tmp.Add(dict["id"].ToString());
                }
                scriptures = tmp.ToArray();
            }
        }
        catch (Exception) { /* ignore */ }
    }

    public async Task<bool> PlayAsync()
    {
        if (cardValues == null) InitGrid(2);
        for (int i = 0; i < cardValues.Length; i++)
        {
            if (matched[i]) continue;
            for (int j = i + 1; j < cardValues.Length; j++)
            {
                if (matched[j]) continue;
                FlipIndex(i);
                await ToSignal(GetTree().CreateTimer(0.02f), "timeout");
                FlipIndex(j);
                await ToSignal(GetTree().CreateTimer(0.02f), "timeout");
                if (IsComplete()) return true;
            }
        }
        return IsComplete();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb && mb.Pressed)
        {
            var ray = GetWorld3D().DirectSpaceState.IntersectRay(
                GetViewport().GetCamera3D().ProjectRayOrigin(GetViewport().GetMousePosition()),
                GetViewport().GetCamera3D().ProjectRayNormal(GetViewport().GetMousePosition()) * 1000
            );
            if (ray.Count > 0 && ray.Contains("collider"))
            {
                var hit = ray["collider"] as Node3D;
                int idx = cards.IndexOf(hit);
                if (idx >= 0) FlipIndex(idx);
            }
        }
    }
}