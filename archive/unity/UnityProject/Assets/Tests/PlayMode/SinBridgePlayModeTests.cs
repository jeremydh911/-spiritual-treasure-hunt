using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class SinBridgePlayModeTests
{
    [UnityTest]
    public IEnumerator SinBridge_Build_CreatesExpectedElements()
    {
        var go = new GameObject("sinRoot");
        var ui = go.AddComponent<SinBridgeUI>();

        ui.Build();

        // Verify structure
        var left = go.transform.Find("SinBridge_Left");
        var mid = go.transform.Find("SinBridge_Middle");
        var right = go.transform.Find("SinBridge_Right");

        Assert.IsNotNull(left, "Left column should exist");
        Assert.IsNotNull(mid, "Middle column should exist");
        Assert.IsNotNull(right, "Right column should exist");

        var humanBox = left.Find("HumanBox");
        var barrierLabel = mid.Find("BarrierBox/Label");
        var kingdomBox = right.Find("KingdomBox");

        Assert.IsNotNull(humanBox, "HumanBox should be created");
        Assert.IsNotNull(barrierLabel, "Barrier Label should be created");
        Assert.IsNotNull(kingdomBox, "KingdomBox should be created");

        var labelText = barrierLabel.GetComponent<Text>().text;
        Assert.AreEqual(ui.barrierLabel, labelText);

        Object.DestroyImmediate(go);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SinBridge_PlaySequence_TransitionsToCoveredAndWalks()
    {
        var go = new GameObject("sinRoot2");
        var ui = go.AddComponent<SinBridgeUI>();

        // shorten timings for fast test
        ui.startDelay = 0f;
        ui.bridgeDropDuration = 0.02f;
        ui.barrierCoverFade = 0.02f;
        ui.walkDuration = 0.02f;
        ui.endGlowDuration = 0.02f;

        ui.Build();

        ui.Play();

        // wait a small amount longer than the summed durations
        yield return new WaitForSeconds(0.2f);

        var barrierLabel = go.transform.Find("SinBridge_Middle/BarrierBox/Label");
        Assert.IsNotNull(barrierLabel);
        Assert.AreEqual("Covered", barrierLabel.GetComponent<Text>().text);

        // Ensure the human moved toward the kingdom (x position increased)
        var humanRT = go.transform.Find("SinBridge_Left/HumanBox")?.GetComponent<RectTransform>();
        var kingdomRT = go.transform.Find("SinBridge_Right/KingdomBox")?.GetComponent<RectTransform>();
        Assert.IsNotNull(humanRT);
        Assert.IsNotNull(kingdomRT);

        Assert.GreaterOrEqual(humanRT.anchoredPosition.x, 0f);

        Object.DestroyImmediate(go);
        yield return null;
    }
}