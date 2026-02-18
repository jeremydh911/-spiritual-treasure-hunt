using NUnit.Framework;
using UnityEngine;
using System.IO;

public class TruthsTests
{
    string SavePathFor(string playerId) => Path.Combine(Application.persistentDataPath, "saves", $"player_{playerId}.json");

    [Test]
    public void NewTruths_ArePresentInResourceIndex()
    {
        var ta = Resources.Load<TextAsset>("Truths/truths_index");
        Assert.IsNotNull(ta, "truths_index resource should exist");
        var text = ta.text;

        // identity / encouragement truths
        Assert.IsTrue(text.Contains("TRU-MADE-NEW"));
        Assert.IsTrue(text.Contains("TRU-PURPOSE"));
        Assert.IsTrue(text.Contains("TRU-BLESSED"));

        // newly added truths from recent content expansion
        Assert.IsTrue(text.Contains("TRU-CHOSEN"));
        Assert.IsTrue(text.Contains("TRU-NOTALONE"));
        Assert.IsTrue(text.Contains("TRU-REDEEMED"));
        Assert.IsTrue(text.Contains("TRU-FRIEND"));
        Assert.IsTrue(text.Contains("TRU-AMBASSADOR"));
        Assert.IsTrue(text.Contains("TRU-FREE"));
        Assert.IsTrue(text.Contains("TRU-ROYALTY"));
        Assert.IsTrue(text.Contains("TRU-SAFE"));
        Assert.IsTrue(text.Contains("TRU-RENEWED"));
    }

    [Test]
    public void Affirmation_CompleteToday_AddsNewTruthToProfile()
    {
        var go = new GameObject("pt");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "truthtest_1";
        profile.ownedTruths = new string[0];

        Assert.IsFalse(profile.HasTruth("TRU-MADE-NEW"));
        AffirmationManager.CompleteToday(profile, "TRU-MADE-NEW");
        Assert.IsTrue(profile.HasTruth("TRU-MADE-NEW"));

        var path = SavePathFor(profile.playerId);
        Assert.IsTrue(File.Exists(path));

        Object.DestroyImmediate(go);
    }

    [Test]
    public void TruthQuests_Additions_ArePresentInContent()
    {
        var path = Path.Combine(Application.dataPath, "..", "Content", "Quests", "truth_quests.json");
        Assert.IsTrue(File.Exists(path), "truth_quests.json must exist in Content/Quests");
        var text = File.ReadAllText(path);

        // ensure recently-added truth quests appear in the content file
        Assert.IsTrue(text.Contains("\"id\": \"TRU-006\""), "TRU-006 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-007\""), "TRU-007 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-008\""), "TRU-008 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-009\""), "TRU-009 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-010\""), "TRU-010 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-011\""), "TRU-011 quest should be present");
        Assert.IsTrue(text.Contains("\"id\": \"TRU-012\""), "TRU-012 quest should be present");
    }
}