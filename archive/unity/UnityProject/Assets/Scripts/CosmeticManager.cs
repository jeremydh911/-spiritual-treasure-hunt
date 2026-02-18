using System.Linq;
using UnityEngine;

public static class CosmeticManager
{
    // Returns true if the cosmetic is visible for the given player profile
    public static bool IsCosmeticVisibleForPlayer(CosmeticItem item, PlayerProfile profile)
    {
        if (item == null || profile == null) return false;

        // Spiritual Armor is always available to all players (Ephesians) regardless of approval/denom.
        if (item.isSpiritualArmorPiece) return true;

        if (!item.approved) return false;
        if (item.requiresAdultMode && !profile.IsAdultModeAllowed()) return false;
        if (profile.adultModeDisabledByChurch) return false;
        if (item.allowedDenoms != null && item.allowedDenoms.Length > 0)
        {
            if (!item.allowedDenoms.Contains(profile.denomId)) return false;
        }
        return true;
    } 
}