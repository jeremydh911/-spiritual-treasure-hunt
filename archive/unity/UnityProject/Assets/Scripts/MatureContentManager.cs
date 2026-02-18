using UnityEngine;

public static class MatureContentManager
{
    // Returns true if player is allowed to view Mature Mode content
    public static bool IsMatureContentAllowed(PlayerProfile profile)
    {
        if (profile == null) return false;
        return profile.IsAdultModeAllowed();
    }

    // Load text asset from Resources/MatureContent by key
    public static string LoadMatureText(string key)
    {
        var ta = Resources.Load<TextAsset>("MatureContent/" + key);
        return ta != null ? ta.text : null;
    }
}