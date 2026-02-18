using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Runtime demo controller for the `TruthBarrier` dispel mechanic.
/// - Safe, self-contained: if references are missing it will create minimal demo objects.
/// - Intended for manual QA (attach to a scene) and for lightweight PlayMode tests.
/// </summary>
public class TruthBarrierDemo : MonoBehaviour
{
    public PlayerProfile demoProfile;
    public TruthBarrier barrier;
    public Text statusText;

    private void Start()
    {
        // ensure demo profile exists
        if (demoProfile == null)
        {
            var pgo = new GameObject("DemoPlayer");
            demoProfile = pgo.AddComponent<PlayerProfile>();
            demoProfile.playerId = "demo_player_truthbarrier";
            demoProfile.ownedScriptures = new string[] { "SCRIPT-ROM8-1" };
            // equip for demonstration
            ScriptureManager.EquipScripture(demoProfile, "SCRIPT-ROM8-1");
        }

        // ensure barrier exists
        if (barrier == null)
        {
            var bgo = new GameObject("DemoBarrier");
            barrier = bgo.AddComponent<TruthBarrier>();
            barrier.barrierActive = true;
        }

        // Run the demo automatically in the scene
        RunDemo();
    }

    /// <summary>
    /// Performs the demo action (attempt to enter using equipped scripture) and
    /// updates the optional UI `statusText`.
    /// </summary>
    public void RunDemo()
    {
        if (demoProfile == null || barrier == null)
        {
            Debug.LogWarning("TruthBarrierDemo: missing profile or barrier references.");
            return;
        }

        var canEnter = barrier.AttemptEnter(demoProfile);
        var msg = canEnter ? "Barrier dispelled — entry allowed" : "Barrier remains active";
        Debug.Log("TruthBarrierDemo: " + msg);
        if (statusText != null) statusText.text = msg;
    }
}