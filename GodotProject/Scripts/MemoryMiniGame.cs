using Godot;
using System.Threading.Tasks;

/// <summary>
/// Very small memory mini-game helper for the Godot truth quest prototype.
/// - `SimulateWin()` returns true for automated tests.
/// - `PlayAsync()` simulates a short play duration and returns success/fail.
/// </summary>
public partial class MemoryMiniGame : Node
{
    [Export] public string scriptureId = "SCRIPT-1PET2-9";

    // Simulate a correct play (used by tests and quick demo)
    public bool SimulateWin()
    {
        // simple deterministic success
        return true;
    }

    // A small logical memory game implementation for tests/demo
    private int[] cards = null; // pairs represented by integers
    private bool[] matched = null;
    private int firstFlipped = -1;
    private int secondFlipped = -1;

    public void InitGrid(int pairs = 3)
    {
        var size = pairs * 2;
        cards = new int[size];
        matched = new bool[size];
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
    }

    public bool FlipCard(int index)
    {
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
