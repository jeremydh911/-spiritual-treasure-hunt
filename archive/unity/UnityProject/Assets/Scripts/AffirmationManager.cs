using UnityEngine;

/// <summary>
/// Handles daily affirmations/truths and tracks completion/streaks.
/// Affirmations are lightweight: record that the player completed today's affirmation and award small rewards.
/// </summary>
public static class AffirmationManager
{
    // For prototype: store lastAffirmationDate on profile as ISO date string
    public static bool HasCompletedToday(PlayerProfile profile)
    {
        if (profile == null || string.IsNullOrEmpty(profile.lastAffirmationDate)) return false;
        if (System.DateTime.TryParse(profile.lastAffirmationDate, out System.DateTime d))
        {
            return d.Date == System.DateTime.UtcNow.Date;
        }
        return false;
    }

    public static void CompleteToday(PlayerProfile profile, string truthId)
    {
        if (profile == null) return;
        profile.lastAffirmationDate = System.DateTime.UtcNow.ToString("yyyy-MM-dd");
        // Optionally award small spiritualStrength or a cosmetic
        profile.AddTruth(truthId);
        SaveManager.SaveLocalProfile(profile);
        TelemetryManager.LogEvent($"affirmation_complete:{profile.playerId}:{truthId}");
    }
}