using NUnit.Framework;
using UnityEngine;
using System.IO;

public class ScriptureTests
{
    string SavePathFor(string playerId)
    {
        return Path.Combine(Application.persistentDataPath, "saves", $"player_{playerId}.json");
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            var dir = Path.Combine(Application.persistentDataPath, "saves");
            if (Directory.Exists(dir))
            {
                foreach (var f in Directory.GetFiles(dir, "player_scripturetest_*.json")) File.Delete(f);
            }
        }
        catch { }
    }

    [Test]
    public void LearnAndEquipScripture_PersistsAndFiresEvents()
    {
        var go = new GameObject("sp");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "scripturetest_1";
        profile.ownedScriptures = new string[0];

        bool learnedFired = false;
        ScriptureManager.OnScriptureLearned += (id, p) => { if (p == profile && id == "SCRIPT-EPH6-17") learnedFired = true; };

        var learned = ScriptureManager.LearnScripture(profile, "SCRIPT-EPH6-17");
        Assert.IsTrue(learned, "Learning scripture should succeed");
        Assert.IsTrue(profile.HasScripture("SCRIPT-EPH6-17"));
        Assert.IsTrue(learnedFired, "OnScriptureLearned should be fired");

        bool equippedFired = false;
        ScriptureManager.OnScriptureEquipped += (id, p) => { if (p == profile && id == "SCRIPT-EPH6-17") equippedFired = true; };
        var equipOk = ScriptureManager.EquipScripture(profile, "SCRIPT-EPH6-17");
        Assert.IsTrue(equipOk, "Equip should succeed for learned scripture");
        Assert.AreEqual("SCRIPT-EPH6-17", profile.GetEquippedScripture());
        Assert.IsTrue(equippedFired, "OnScriptureEquipped should fire");

        // also ensure the new dispel scripture exists and maps to DispelLies
        var dispel = ScriptureManager.GetById("SCRIPT-ROM8-1");
        Assert.IsNotNull(dispel, "Scripture ROM 8:1 should exist in the index");
        Assert.AreEqual(ScriptureManager.ScriptureEffect.DispelLies, ScriptureManager.UseScripture("SCRIPT-ROM8-1"));

        // persisted
        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path), "Profile should be saved after learning/equipping scripture");

        Object.DestroyImmediate(go);
        ScriptureManager.OnScriptureLearned = null;
        ScriptureManager.OnScriptureEquipped = null;
    }
}