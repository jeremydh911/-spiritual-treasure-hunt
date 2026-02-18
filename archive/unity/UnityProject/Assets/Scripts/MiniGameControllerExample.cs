using UnityEngine;

/// <summary>
/// Example mini‑game controller — simulates a win in the inspector for testing.
/// Assign PlayerProfile and a questId (e.g., "ECHO-001" or "WIS-001").
/// This example applies a scripture-based multiplier to the score when an equipped scripture
/// has a "weapon" tag (demonstrates scripture-as-weapon integration).
/// </summary>
public class MiniGameControllerExample : MiniGameBase
{
    public string questId;
    public int lastScore = 0;
    public int baseScore = 100;

    // Call this from a Win condition in a real mini-game.
    public void SimulateWin()
    {
        // compute scripture multiplier (if any)
        float multiplier = 1f;
        var eq = playerProfile?.GetEquippedScripture();
        if (!string.IsNullOrEmpty(eq))
        {
            var eff = ScriptureManager.UseScripture(eq);
            if (eff == ScriptureManager.ScriptureEffect.WeaponBoost) multiplier = 1.5f;
            else if (eff == ScriptureManager.ScriptureEffect.PrayerBuff) multiplier = 1.1f;
        }

        lastScore = Mathf.RoundToInt(baseScore * multiplier);
        TelemetryManager.LogEvent($"minigame_score:{questId}:{lastScore}");

        CompleteMiniGame(questId);
    }
} 