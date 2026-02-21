using Godot;
using System;
using System.IO;
using System.Threading.Tasks;

public partial class TruthQuest_IAmChosen3D : Node3D
{
    public MemoryMiniGame3D memoryGame;
    public Node3D barrier; // generic
    public Label3D statusLabel;

    public override void _Ready()
    {
        memoryGame = GetNodeOrNull<MemoryMiniGame3D>("MemoryMiniGame3D");
        barrier = GetNodeOrNull<Node3D>("Barrier");
        statusLabel = GetNodeOrNull<Label3D>("Status");
    }

    public async Task<bool> RunDemoAsync(PlayerProfile profile)
    {
        if (statusLabel != null) statusLabel.Text = "Starting 'I Am Chosen' quest (3D)...";

        if (!profile.HasScripture("SCRIPT-1PET2-9")) profile.AddScripture("SCRIPT-1PET2-9");

        var ok = memoryGame != null ? await memoryGame.PlayAsync() : true;
        if (!ok)
        {
            if (statusLabel != null) statusLabel.Text = "Try again to remember the verse.";
            return false;
        }

        ScriptureManager.EquipScripture(profile, "SCRIPT-1PET2-9");
        if (statusLabel != null) statusLabel.Text = "Verse equipped — approaching the barrier...";

        var canEnter = true;
        if (barrier != null)
        {
            // barrier may implement IBarrier3D interface or simple property
            var method = barrier.GetMethod("AttemptEnter");
            if (method != null)
                canEnter = (bool)barrier.Call("AttemptEnter", profile);
        }

        if (canEnter)
        {
            profile.AddTruth("TRU-CHOSEN");
            if (statusLabel != null) statusLabel.Text = "Truth gained: I Am Chosen";
            WriteResultToDisk(true);
            return true;
        }

        if (statusLabel != null) statusLabel.Text = "Barrier remains — try another scripture.";
        WriteResultToDisk(false);
        return false;
    }

    public void RunDemo(PlayerProfile profile)
    {
        _ = RunDemoAsync(profile);
    }

    private void WriteResultToDisk(bool pass)
    {
        try
        {
            var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "truthquest_ichosen3d_result.txt");
            File.WriteAllText(outPath, (pass ? "PASS" : "FAIL") + "\n");
            GD.Print($"TruthQuest_IAmChosen3D: result written to {outPath} -> {(pass?"PASS":"FAIL")}");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Failed to write result: " + ex.Message);
        }
    }
}
