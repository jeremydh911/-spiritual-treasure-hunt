using Godot;
using System.Threading.Tasks;

/// <summary>
/// Very small memory mini-game helper for the Godot truth quest prototype.
/// - `SimulateWin()` returns true for automated tests.
/// - `PlayAsync()` simulates a short play duration and returns success/fail.
/// </summary>
public partial class MemoryMiniGame : Node2D
{
    [Export] public string scriptureId = "SCRIPT-1PET2-9";
    [Export] public PackedScene cardButtonScene;
    [Export] public Label statusLabel; // optional, update when game completes

    // logical state
    private int[] cards = null; // pairs represented by integers
    private bool[] matched = null;
    private int firstFlipped = -1;
    private int secondFlipped = -1;

    private List<Button> uiCards = new();

    public int Attempts { get; private set; }
    public float Duration { get; private set; }
    private DateTime startTime;

    // Simulate a correct play (used by tests and quick demo)
    public bool SimulateWin()
    {
        // simple deterministic success
        return true;
    }

    public override void _Ready()
    {
        // optional: auto-create buttons if cardButtonScene provided
        if (cardButtonScene != null && uiCards.Count == 0)
        {
            InitGrid(3);
        }
    }

    public void InitGrid(int pairs = 3)
    {
        CleanupUI();

        var size = pairs * 2;
        cards = new int[size];
        matched = new bool[size];
        uiCards.Clear();
        var idx = 0;
        for (int v = 0; v < pairs; v++)
        {
            cards[idx++] = v;
            cards[idx++] = v;
        }
        // simple shuffle
        var rnd = new Random();
        for (int i = 0; i < size; i++)
        {
            int j = rnd.Next(i, size);
            var tmp = cards[i]; cards[i] = cards[j]; cards[j] = tmp;
        }
        // create UI buttons
        for (int i = 0; i < size; i++)
        {
            var btn = cardButtonScene != null ? (Button)cardButtonScene.Instantiate() : new Button();
            btn.Name = $"Card{i}";
            btn.Text = "?";
            btn.RectPosition = new Vector2((i % pairs) * 80, (i / pairs) * 80);
            AddChild(btn);
            uiCards.Add(btn);
            int ii = i;
            btn.Pressed += () => OnCardPressed(ii);
        }
    }

    private void CleanupUI()
    {
        foreach (var b in uiCards) b.QueueFree();
        uiCards.Clear();
    }

    private void OnCardPressed(int index)
    {
        FlipCard(index);
        if (uiCards.Count > index)
        {
            var btn = uiCards[index];
            btn.Text = cards[index].ToString();
            if (matched[index]) btn.Disabled = true;
        }
    }

    public bool FlipCard(int index)
    {
        if (startTime == default) startTime = DateTime.UtcNow;
        Attempts++;

        if (cards == null || index < 0 || index >= cards.Length) return false;
        if (matched[index]) return false;
        if (firstFlipped == -1) { firstFlipped = index; return true; }
        if (secondFlipped == -1) { secondFlipped = index; }
        // evaluate
        if (cards[firstFlipped] == cards[secondFlipped])
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

    public bool IsComplete()
    {
        if (matched == null) return false;
        foreach (var m in matched) if (!m) return false;
        return true;
    }

    // Async play simulation (placeholder for real UI)
    public async Task<bool> PlayAsync()
    {
        if (cards == null) InitGrid(3);
        // naive auto-solve for demo/tests: flip matching pairs by scanning
        for (int i = 0; i < cards.Length; i++)
        {
            if (matched[i]) continue;
            for (int j = i + 1; j < cards.Length; j++)
            {
                if (matched[j]) continue;
                // flip i and j
                FlipCard(i);
                await ToSignal(GetTree().CreateTimer(0.02f), "timeout");
                FlipCard(j);
                await ToSignal(GetTree().CreateTimer(0.02f), "timeout");
                if (IsComplete()) return true;
            }
        }
        return IsComplete();
    }
}
