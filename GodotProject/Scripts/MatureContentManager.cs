using Godot;
using System;

public static class MatureContentManager
{
    public static bool IsMatureContentAllowed(PlayerProfile profile)
    {
        if (profile == null) return false;
        return profile.IsAdultModeAllowed();
    }

    // Try multiple local locations so the app is offline-first.
    public static string LoadMatureText(string key)
    {
        var candidates = new string[] {
            System.IO.Path.Combine("..", "UnityProject", "Assets", "Resources", "MatureContent", key + ".txt"),
            System.IO.Path.Combine("..", "Design", "Guides", "Guide_" + key + ".md"),
            System.IO.Path.Combine("..", "backend", "guides", key + ".md")
        };
        foreach (var p in candidates)
        {
            if (System.IO.File.Exists(p)) return System.IO.File.ReadAllText(p);
        }
        return null;
    }
}
