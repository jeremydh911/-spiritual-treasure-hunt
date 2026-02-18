using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TruthBarrierDemoTests
{
    [UnityTest]
    public IEnumerator TruthBarrierDemo_RunDemo_DeactivatesBarrier()
    {
        var go = new GameObject("demoRoot");
        var demo = go.AddComponent<TruthBarrierDemo>();

        var pgo = new GameObject("player");
        var profile = pgo.AddComponent<PlayerProfile>();
        profile.ownedScriptures = new string[] { "SCRIPT-ROM8-1" };
        ScriptureManager.EquipScripture(profile, "SCRIPT-ROM8-1");
        demo.demoProfile = profile;

        var bgo = new GameObject("barrier");
        var barrier = bgo.AddComponent<TruthBarrier>();
        barrier.barrierActive = true;
        demo.barrier = barrier;

        // run the demo action
        demo.RunDemo();

        Assert.IsFalse(barrier.barrierActive, "Barrier should be deactivated by the demo run when dispel scripture is equipped.");

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(pgo);
        Object.DestroyImmediate(bgo);
        yield return null;
    }
}