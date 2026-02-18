using Godot;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Implements the 'I Am Chosen' truth quest demo flow:
/// - runs MemoryMiniGame
/// - equips the memorized scripture
/// - dispels TruthBarrier and awards TRU-CHOSEN
/// - writes PASS/FAIL to user://truthquest_ichosen_result.txt for QA
/// </summary>
public partial class TruthQuest_IAmChosen : Node
{
    public MemoryMiniGame memoryGame;
    public TruthBarrier barrier;
    public Label statusLabel;

    public override void _Ready()
    {
        memoryGame = GetNodeOrNull<MemoryMiniGame>("MemoryMiniGame");
        barrier = GetNodeOrNull<TruthBarrier>("TruthBarrier");
        statusLabel = GetNodeOrNull<Label>("Status");
    }

    public async Task<bool> RunDemoAsync(PlayerProfile profile)
    {
        if (statusLabel != null) statusLabel.Text = "Starting 'I Am Chosen' quest...";

        // Ensure player has the verse in their owned scriptures (simulates memorization acquisition)
        if (!profile.HasScripture("SCRIPT-1PET2-9")) profile.AddScripture("SCRIPT-1PET2-9");

        // Play the memory mini-game (simulated)
        var ok = memoryGame != null ? await memoryGame.PlayAsync() : true;
        if (!ok)
        {
            if (statusLabel != null) statusLabel.Text = "Try again to remember the verse.";
            return false;
        }

        // Equip the memorized scripture
        ScriptureManager.EquipScripture(profile, "SCRIPT-1PET2-9");
        if (statusLabel != null) statusLabel.Text = "Verse equipped — approaching the barrier...";

        // Attempt to cross dispelling the barrier
        var canEnter = barrier != null ? barrier.AttemptEnter(profile) : true;

        if (canEnter)
        {
            // Award the truth to the profile
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
            var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "truthquest_ichosen_result.txt");
            File.WriteAllText(outPath, (pass ? "PASS" : "FAIL") + "\n");
            GD.Print($"TruthQuest_IAmChosen: result written to {outPath} -> {(pass?"PASS":"FAIL")}");
        }
        catch (Exception ex)
        {
            GD.PrintErr("Failed to write result: " + ex.Message);
        }
    }
}
