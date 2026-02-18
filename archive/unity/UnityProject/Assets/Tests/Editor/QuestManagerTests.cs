using NUnit.Framework;
using UnityEngine;
using System.IO;

public class QuestManagerTests
{
    string SavePathFor(string playerId)
    {
        return Path.Combine(Application.persistentDataPath, "saves", $"player_{playerId}.json");
    }

    [TearDown]
    public void TearDown()
    {
        // remove any test save files created during tests
        try
        {
            var dir = Path.Combine(Application.persistentDataPath, "saves");
            if (Directory.Exists(dir))
            {
                foreach (var f in Directory.GetFiles(dir, "player_qmtest_*.json")) File.Delete(f);
            }
        }
        catch { }
    }

    [Test]
    public void CompleteQuest_AwardsRewardsAndSaves()
    {
        var go = new GameObject("pm");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "qmtest_profile_1";
        profile.ownedFruits = new string[0];
        profile.ownedVirtues = new string[0];
        profile.ownedActivities = new string[0];
        profile.completedQuests = new string[0];

        bool eventFired = false;
        QuestManager.OnQuestCompleted += (id, p) => { if (id == "NEIGH-005") eventFired = true; };

        // NEIGH-005 awards fruit "Goodness" according to phase1_quests.json
        var result = QuestManager.CompleteQuest(profile, "NEIGH-005");
        Assert.IsTrue(result, "CompleteQuest should return true for a fresh quest");
        Assert.IsTrue(profile.completedQuests != null && System.Array.Exists(profile.completedQuests, x => x == "NEIGH-005"), "Quest should be recorded as completed");
        Assert.IsTrue(profile.HasFruit("Goodness"), "Fruit reward should be granted");
        Assert.IsTrue(eventFired, "OnQuestCompleted event should be raised");

        // saved file should exist
        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path), "Profile should be saved to disk after completion");

        Object.DestroyImmediate(go);
        QuestManager.OnQuestCompleted = null;
    }

    [Test]
    public void CompleteQuest_AwardsMusicTrack()
    {
        var go = new GameObject("pm_music");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "qmtest_profile_music";
        profile.ownedMusicTracks = new string[0];
        profile.completedQuests = new string[0];

        // JOY-002 should award music track WOR-JOY-001
        var res = QuestManager.CompleteQuest(profile, "JOY-002");
        Assert.IsTrue(res, "CompleteQuest should succeed for JOY-002");
        Assert.IsTrue(profile.HasMusicTrack("WOR-JOY-001"), "Music track should be unlocked on quest completion");

        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path), "Profile should be saved after music track award");

        Object.DestroyImmediate(go);
    }

    [Test]
    public void MatureQuest_IsGatedByMatureMode()
    {
        var go = new GameObject("pm_prop");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "qmtest_profile_prop";
        profile.completedQuests = new string[0];

        // Unverified should be blocked
        profile.ageVerified = false;
        var resBlocked = QuestManager.CompleteQuest(profile, "PROP-001");
        Assert.IsFalse(resBlocked, "Mature quest should be blocked for unverified profiles");
        Assert.IsFalse(QuestManager.CanStartQuest(profile, "PROP-001"));

        // Verified age should be allowed (no parental consent required)
        profile.ageVerified = true;
        profile.adultModeDisabledByChurch = false;
        var resAllowed = QuestManager.CompleteQuest(profile, "PROP-001");
        Assert.IsTrue(resAllowed, "Mature quest should be allowed for age-verified profiles");
        Assert.IsTrue(QuestManager.CanStartQuest(profile, "PROP-001"));

        Object.DestroyImmediate(go);
    }

    [Test]
    public void TryClaimVirtue_AllowsClaimOnlyAfterQuestCompleted()
    {
        var go = new GameObject("pm2");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "qmtest_profile_2";
        profile.ownedVirtues = new string[0];
        profile.completedQuests = new string[0];

        // Attempt to claim virtue before completing quest should fail
        var pre = QuestManager.TryClaimVirtue(profile, "Wisdom");
        Assert.IsFalse(pre, "Claiming virtue should fail if quest not completed");

        // mark required quest completed and try again
        profile.AddCompletedQuest("WIS-001");
        bool awarded = QuestManager.TryClaimVirtue(profile, "Wisdom");
        Assert.IsTrue(awarded, "Claiming virtue should succeed after quest completion");
        Assert.IsTrue(profile.HasVirtue("Wisdom"));

        // saved file should exist
        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path), "Profile should be saved after claiming virtue");

        Object.DestroyImmediate(go);
    }

    [Test]
    public void CompleteQuest_AwardsActivityAndFiresEvent()
    {
        var go = new GameObject("pm3");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "qmtest_profile_3";
        profile.ownedActivities = new string[0];
        profile.completedQuests = new string[0];

        // inject a test quest that awards an activity
        QuestManager.AddOrReplaceTestQuest("ACT-TEST-001", activityReward: "Service");

        bool activityEvent = false;
        QuestManager.OnActivityAwarded += (id, p) => { if (id == "Service") activityEvent = true; };

        var res = QuestManager.CompleteQuest(profile, "ACT-TEST-001");
        Assert.IsTrue(res, "CompleteQuest should succeed for injected test quest");
        Assert.IsTrue(profile.HasActivity("Service"), "Activity should be granted to profile");
        Assert.IsTrue(activityEvent, "OnActivityAwarded event should be fired");

        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path), "Profile should be saved after activity award");

        Object.DestroyImmediate(go);
        QuestManager.OnActivityAwarded = null;
    }

    [Test]
    public void PlayerProfile_IsStewardshipReady_Works()
    {
        var go = new GameObject("pm_stew");
        var profile = go.AddComponent<PlayerProfile>();
        profile.ownedVirtues = new string[0];
        profile.ownedActivities = new string[0];

        Assert.IsFalse(profile.IsStewardshipReady(), "Profile without stewardship should not be ready");
        profile.AddActivity("StewardshipPractice");
        Assert.IsTrue(profile.IsStewardshipReady(), "Profile should be stewardship-ready after activity");

        profile.ownedActivities = new string[0];
        profile.AddVirtue("Stewardship");
        Assert.IsTrue(profile.IsStewardshipReady(), "Profile should be stewardship-ready after virtue");

        Object.DestroyImmediate(go);
    }
}