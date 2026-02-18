using System;
using UnityEngine;

/// <summary>
/// AngelManager handles angelic interactions in the game. Angels only provide beneficial actions.
/// This is a gameplay/controller layer — server should verify and audit angel revives or special actions.
/// </summary>
public static class AngelManager
{
    // Simple rate limit example for revives (UTC timestamp stored in PlayerProfile.lastAngelReviveAt)
    public static TimeSpan ReviveCooldown = TimeSpan.FromHours(24);

    public static bool CanRevive(PlayerProfile profile)
    {
        if (profile == null) return false;
        if (profile.lastAngelReviveAt == null || profile.lastAngelReviveAt == "") return true;
        if (DateTime.TryParse(profile.lastAngelReviveAt, out DateTime last))
        {
            return DateTime.UtcNow - last >= ReviveCooldown;
        }
        return true;
    }

    public static bool RevivePlayer(PlayerProfile profile)
    {
        if (profile == null) return false;
        if (!CanRevive(profile)) return false;
        // For prototype: set a flag that player is revived (game logic should interpret this)
        profile.isDown = false; // ensure PlayerProfile contains isDown (added)
        profile.lastAngelReviveAt = DateTime.UtcNow.ToString("o");
        // Log event (telemetry is opt-outable)
        TelemetryManager.LogEvent($"angel_revive:{profile.playerId}");
        Debug.Log($"Angel revived player {profile.playerId}");
        return true;
    }

    public static void DeliverMessage(PlayerProfile profile, string message)
    {
        // Show a short encouraging message; in production, hook to UI system.
        Debug.Log($"Angel message to {profile?.playerId}: {message}");
        TelemetryManager.LogEvent($"angel_message:{profile?.playerId}");
    }

    public static void GuideToTreasure(PlayerProfile profile, string hint)
    {
        // Mark a special quest/treasure hint on the player's map (stub)
        Debug.Log($"Angel guides {profile?.playerId} to: {hint}");
        TelemetryManager.LogEvent($"angel_guide:{profile?.playerId}");
    }

    // Mature / adult-only angel helps (gated by MatureContentManager)
    public static bool ProvideAdultCounsel(PlayerProfile profile, string topic)
    {
        if (!MatureContentManager.IsMatureContentAllowed(profile)) return false;
        // Provide an adult‑level counsel summary (not a replacement for pastoral care)
        DeliverMessage(profile, $"Angel counsel on {topic}: seek God, pray, and get help from trusted leaders.");
        TelemetryManager.LogEvent($"angel_adult_counsel:{profile?.playerId}:{topic}");
        return true;
    }

    public static bool ComfortInGrief(PlayerProfile profile)
    {
        if (!MatureContentManager.IsMatureContentAllowed(profile)) return false;
        DeliverMessage(profile, "An angel brings comfort — seek community and prayer in times of grief.");
        TelemetryManager.LogEvent($"angel_comfort:{profile?.playerId}");
        return true;
    }

    public static bool ExplainDifficultDoctrine(PlayerProfile profile, string doctrineKey)
    {
        if (!MatureContentManager.IsMatureContentAllowed(profile)) return false;
        // Provide a short exegesis help text keyed by doctrineKey (stub)
        DeliverMessage(profile, $"Angel explains {doctrineKey}: see the mature guide in your Parent Portal.");
        TelemetryManager.LogEvent($"angel_explain:{profile?.playerId}:{doctrineKey}");
        return true;
    }
}