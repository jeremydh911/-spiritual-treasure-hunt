using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ScripturePlayModeTests
{
    [UnityTest]
    public IEnumerator UseScripture_ReturnsWeaponBoostForWeaponTaggedScripture()
    {
        var res = ScriptureManager.UseScripture("SCRIPT-EPH6-17");
        Assert.AreEqual(ScriptureManager.ScriptureEffect.WeaponBoost, res);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EquipScripture_AllowsMiniGameAccess()
    {
        var go = new GameObject("pm_scripture");
        var profile = go.AddComponent<PlayerProfile>();
        profile.playerId = "play_scripture_1";
        profile.ownedScriptures = new string[] { "SCRIPT-EPH6-17" };

        var ok = ScriptureManager.EquipScripture(profile, "SCRIPT-EPH6-17");
        Assert.IsTrue(ok);
        Assert.AreEqual("SCRIPT-EPH6-17", profile.GetEquippedScripture());

        // Simulate a mini-game using the equipped scripture for a weapon boost
        var effect = ScriptureManager.UseScripture(profile.GetEquippedScripture());
        Assert.AreEqual(ScriptureManager.ScriptureEffect.WeaponBoost, effect);

        Object.DestroyImmediate(go);
        yield return null;
    }

    [UnityTest]
    public IEnumerator MemoryMatch_RevealDelay_ReducesWithWeaponScripture()
    {
        var mgGo = new GameObject("mmg");
        var mg = mgGo.AddComponent<MemoryMatchMiniGame>();
        var pgo = new GameObject("pp");
        var profile = pgo.AddComponent<PlayerProfile>();
        profile.playerId = "play_mm_1";

        // default (no scripture equipped)
        Assert.AreEqual(0.6f, mg.GetRevealDelay(), 0.001f);

        // equip weapon scripture
        profile.ownedScriptures = new string[] { "SCRIPT-EPH6-17" };
        ScriptureManager.EquipScripture(profile, "SCRIPT-EPH6-17");
        mg.playerProfile = profile;
        Assert.AreEqual(0.35f, mg.GetRevealDelay(), 0.001f);

        Object.DestroyImmediate(mgGo);
        Object.DestroyImmediate(pgo);
        yield return null;
    }

    [UnityTest]
    public IEnumerator RhythmMiniGame_HitCount_IncrementsFasterWithWeaponScripture()
    {
        var go = new GameObject("rm");
        var mg = go.AddComponent<RhythmMiniGame>();
        mg.requiredHits = 5;

        var pgo = new GameObject("pp2");
        var profile = pgo.AddComponent<PlayerProfile>();
        profile.playerId = "play_rm_1";

        // without scripture: each tap increments by 1
        mg.playerProfile = profile;
        mg.OnTap();
        Assert.AreEqual(1, mg.GetHitCount());

        // equip weapon scripture: each tap increments by 2
        profile.ownedScriptures = new string[] { "SCRIPT-EPH6-17" };
        ScriptureManager.EquipScripture(profile, "SCRIPT-EPH6-17");
        mg.playerProfile = profile;
        mg.OnTap();
        Assert.AreEqual(3, mg.GetHitCount()); // previous 1 + 2

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(pgo);
        yield return null;
    }
}