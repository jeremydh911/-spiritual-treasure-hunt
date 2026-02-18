using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TruthBarrierTests
{
    [UnityTest]
    public IEnumerator Scripture_DispelLies_AllowsBarrierEntry()
    {
        var pgo = new GameObject("player");
        var profile = pgo.AddComponent<PlayerProfile>();
        profile.playerId = "tb_profile_1";
        profile.ownedScriptures = new string[] { "SCRIPT-ROM8-1" };

        // equip the dispel scripture
        var ok = ScriptureManager.EquipScripture(profile, "SCRIPT-ROM8-1");
        Assert.IsTrue(ok);

        var barrierGO = new GameObject("barrier");
        var barrier = barrierGO.AddComponent<TruthBarrier>();
        barrier.barrierActive = true;

        // Attempt to enter should succeed because equipped scripture dispels lies
        var canEnter = barrier.AttemptEnter(profile);
        Assert.IsTrue(canEnter);
        Assert.IsFalse(barrier.barrierActive);

        Object.DestroyImmediate(pgo);
        Object.DestroyImmediate(barrierGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Scripture_UseScripture_ReturnsDispelLies_ForTaggedVerse()
    {
        var eff = ScriptureManager.UseScripture("SCRIPT-ROM8-1");
        Assert.AreEqual(ScriptureManager.ScriptureEffect.DispelLies, eff);
        yield return null;
    }
}