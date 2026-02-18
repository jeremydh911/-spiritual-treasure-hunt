using UnityEngine;

/// <summary>
/// Prototype rhythm mini-game: player taps a button in rhythm; on enough correct taps, the mini-game completes.
/// Designed to be child‑friendly and mapped to worship/music quests.
/// </summary>
public class RhythmMiniGame : MiniGameBase
{
    public string questId;
    public int requiredHits = 8;
    private int hitCount = 0;

    public void OnTap()
    {
        int increment = 1;
        if (playerProfile != null)
        {
            var eq = playerProfile.GetEquippedScripture();
            if (!string.IsNullOrEmpty(eq))
            {
                var eff = ScriptureManager.UseScripture(eq);
                if (eff == ScriptureManager.ScriptureEffect.WeaponBoost) increment = 2;
                else if (eff == ScriptureManager.ScriptureEffect.PrayerBuff) increment = 1; // small buff could be applied elsewhere
            }
        }
        hitCount += increment;
        if (hitCount >= requiredHits) OnWin();
    }

    private void OnWin()
    {
        CompleteMiniGame(questId);
    }

    // Expose for testing
    public int GetHitCount() => hitCount;
}