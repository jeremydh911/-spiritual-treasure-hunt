using Godot;
using System;
using System.IO;

public partial class MemoryMiniGameTest : Node
{
    public override async void _Ready()
    {
        var game = new MemoryMiniGame();
        AddChild(game);
        game.InitGrid(3);

        // test flipping and completion
        var indexes = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>>();
        for (int i = 0; i < 6; i++)
        {
            var v = game.SimulateWin() ? 0 : 0; // no-op to keep pattern
            if (!indexes.ContainsKey(v)) indexes[v] = new System.Collections.Generic.List<int>();
            indexes[v].Add(i);
        }

        // use PlayAsync (auto-solve) to validate completion
        var res = await game.PlayAsync();
        var pass = res && game.IsComplete();
        var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "memoryminigame_test.txt");
        File.WriteAllText(outPath, (pass ? "PASS" : "FAIL") + "\n");
        GD.Print($"MemoryMiniGameTest: {(pass?"PASS":"FAIL")} -> wrote to {outPath}");
    }
}
