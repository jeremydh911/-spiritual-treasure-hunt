using Godot;
using System;
using System.Collections.Generic;

// Minimal QuestManager that loads JSON from the repo's Content/Quests folder.
public partial class QuestManager : Node
{
    public Dictionary<string, Godot.Collections.Dictionary> quests = new();

    public override void _Ready()
    {
        LoadQuests();
    }

    public void LoadQuests()
    {
        var path = System.IO.Path.Combine("..", "Content", "Quests", "phase1_quests.json");
        if (!System.IO.File.Exists(path))
        {
            GD.PrintWarn($"QuestManager: quest file not found at {path}");
            return;
        }

        var txt = System.IO.File.ReadAllText(path);
        var res = JSON.Parse(txt);
        if (res.Error != Error.Ok)
        {
            GD.PrintErr("QuestManager: JSON parse error");
            return;
        }

        var arr = (Godot.Collections.Array)res.Result;
        foreach (var entry in arr)
        {
            var d = (Godot.Collections.Dictionary)entry;
            var id = d.Contains("id") ? d["id"].ToString() : Guid.NewGuid().ToString();
            quests[id] = d;
        }

        GD.Print($"QuestManager: loaded {quests.Count} quests");
    }

    public bool IsQuestAllowed(PlayerProfile profile, string questId)
    {
        if (!quests.ContainsKey(questId)) return false;
        var q = quests[questId];
        if (q.Contains("ageRange") && q["ageRange"].ToString().Contains("18+"))
        {
            return profile != null && profile.IsAdultByAge(18) && !profile.adultModeDisabledByChurch;
        }
        return true;
    }
}
