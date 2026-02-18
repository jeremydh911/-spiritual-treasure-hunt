using UnityEngine;

/// <summary>
/// Simple example of a truth/barrier gate that can be dispelled by an equipped Scripture with a 'dispel'/'truth' tag.
/// - Call AttemptEnter(profile) to check if the player can pass the barrier.
/// - Demonstrates the 'dispel lies' gameplay mechanic.
/// </summary>
public class TruthBarrier : MonoBehaviour
{
    public bool barrierActive = true;
    public string blockingReason = "a_field_of_doubt";

    /// <summary>
    /// Attempt to enter — returns true if barrier is inactive or scripture dispels it.
    /// </summary>
    public bool AttemptEnter(PlayerProfile profile)
    {
        if (!barrierActive) return true;
        if (profile == null) return false;
        var eq = profile.GetEquippedScripture();
        if (string.IsNullOrEmpty(eq)) return false;
        var eff = ScriptureManager.UseScripture(eq);
        if (eff == ScriptureManager.ScriptureEffect.DispelLies)
        {
            barrierActive = false;
            TelemetryManager.LogEvent($"truthbarrier_dispelled:{profile.playerId}:{eq}");
            return true;
        }
        return false;
    }
}