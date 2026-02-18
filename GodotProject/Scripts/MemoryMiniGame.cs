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
        return true;
    }

    // Async play simulation (placeholder for real UI)
    public async Task<bool> PlayAsync()
    {
        // short simulated delay
        await ToSignal(GetTree().CreateTimer(0.12f), "timeout");
        return true; // always succeed for prototype
    }
}
