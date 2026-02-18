using Godot;
using System;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Runtime test that exercises the I Am Chosen flow and writes PASS/FAIL to user://
/// </summary>
public partial class TruthQuest_IAmChosenTest : Node
{
    public override async void _Ready()
    {
        var scene = new SceneTree();
        var questNode = new TruthQuest_IAmChosen();
        AddChild(questNode);

        var profile = new PlayerProfile();
        profile.playerId = "test_ichosen";
        profile.ownedScriptures = new System.Collections.Generic.List<string>();

        // ensure the relevant scripture exists in index (scriptures_index.json was updated)
        profile.AddScripture("SCRIPT-1PET2-9");

        var res = await questNode.RunDemoAsync(profile);

        var pass = profile.HasTruth("TRU-CHOSEN") && res;
        var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "truthquest_ichosen_test.txt");
        File.WriteAllText(outPath, (pass ? "PASS" : "FAIL") + "\n");
        GD.Print($"TruthQuest_IAmChosenTest: {(pass?"PASS":"FAIL")} -> wrote to {outPath}");
    }
}
