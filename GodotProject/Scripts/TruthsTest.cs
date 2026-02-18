using Godot;
using System;
using System.IO;

/// <summary>
/// Simple runtime check scene for Godot: verifies `Content/Truths/truths_index.json`
/// contains the newly added truth IDs and logs results to the console and a small
/// results file under user:// for offline validation.
/// </summary>
public partial class TruthsTest : Node
{
    public override void _Ready()
    {
        var rel = Path.Combine("..", "Content", "Truths", "truths_index.json");
        if (!File.Exists(rel))
        {
            GD.PrintErr($"TruthsTest: missing file at {rel}");
            return;
        }

        var text = File.ReadAllText(rel);
        var expected = new string[] { "TRU-CHOSEN", "TRU-NOTALONE", "TRU-REDEEMED", "TRU-FRIEND", "TRU-AMBASSADOR", "TRU-FREE", "TRU-ROYALTY" };
        bool allPresent = true;
        foreach (var id in expected)
        {
            var ok = text.Contains(id);
            GD.Print($"TruthsTest: {id} present: {ok}");
            allPresent &= ok;
        }

        var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "truths_test_result.txt");
        File.WriteAllText(outPath, (allPresent ? "PASS" : "FAIL") + "\n");
        GD.Print($"TruthsTest: results written to {outPath}");
    }
}
