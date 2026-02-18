using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.IO;

public class MiniGamePlayModeTests
{
    string SavePathFor(string playerId)
    {
        return Path.Combine(Application.persistentDataPath, "saves", $"player_{playerId}.json");
    }

    [UnityTest]
    public IEnumerator MiniGameWin_AwardsVirtueAndSaves()
    {
        var go = new GameObject("pm_play_1");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "playtest_profile_1";
        profile.ownedVirtues = new string[0];
        profile.completedQuests = new string[0];

        var mgGo = new GameObject("miniGame");
        var mg = mgGo.AddComponent<MiniGameControllerExample>();
        mg.playerProfile = profile;
        mg.questId = "WIS-001"; // WIS-001 exists in Resources and awards Wisdom

        bool virtueEvent = false;
        QuestManager.OnVirtueAwarded += (id, p) => { if (id == "Wisdom") virtueEvent = true; };

        mg.SimulateWin();
        yield return null; // let the frame process

        Assert.IsTrue(profile.HasVirtue("Wisdom"));
        Assert.IsTrue(virtueEvent, "OnVirtueAwarded should fire on mini-game completion");
        Assert.IsTrue(File.Exists(SavePathFor(profile.playerId)), "Profile should be saved after mini-game completion");

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(mgGo);
        QuestManager.OnVirtueAwarded = null;
    }

    [UnityTest]
    public IEnumerator MiniGameWin_InjectionActivityAwardWorks()
    {
        var go = new GameObject("pm_play_2");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "playtest_profile_2";
        profile.ownedActivities = new string[0];
        profile.completedQuests = new string[0];

        // inject a test quest that awards an activity
        QuestManager.AddOrReplaceTestQuest("ACT-MINI-001", activityReward: "Service");

        var mgGo = new GameObject("miniGame2");
        var mg = mgGo.AddComponent<MiniGameControllerExample>();
        mg.playerProfile = profile;
        mg.questId = "ACT-MINI-001";

        bool activityEvent = false;
        QuestManager.OnActivityAwarded += (id, p) => { if (id == "Service") activityEvent = true; };

        mg.SimulateWin();
        yield return null;

        Assert.IsTrue(profile.HasActivity("Service"));
        Assert.IsTrue(activityEvent, "OnActivityAwarded should fire for injected quest");
        Assert.IsTrue(File.Exists(SavePathFor(profile.playerId)), "Profile should be saved after activity award");

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(mgGo);
        QuestManager.OnActivityAwarded = null;
    }

    [UnityTest]
    public IEnumerator MiniGame_ScriptureMultiplier_AppliesToScore()
    {
        var go = new GameObject("pm_mul");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "playtest_profile_mul";
        profile.ownedScriptures = new string[] { "SCRIPT-EPH6-17" };
        ScriptureManager.EquipScripture(profile, "SCRIPT-EPH6-17");

        var mgGo = new GameObject("miniGameMul");
        var mg = mgGo.AddComponent<MiniGameControllerExample>();
        mg.playerProfile = profile;
        mg.questId = "ECHO-001";

        mg.SimulateWin();
        yield return null;

        Assert.AreEqual(150, mg.lastScore, "Equipped weapon scripture should increase mini-game score by 50%");

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(mgGo);
    }
}