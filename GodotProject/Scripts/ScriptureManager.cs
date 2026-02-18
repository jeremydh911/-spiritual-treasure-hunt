using Godot;
using System;

public enum ScriptureEffect { None, DispelLies, Edify, WeaponBoost }

public static class ScriptureManager
{
    // Try to read scriptures index from GodotProject/Resources, then repo Content, then archived Unity resources.
    private static Godot.Collections.Array LoadIndex()
    {
        var paths = new string[] {
            System.IO.Path.Combine("..", "GodotProject", "Resources", "scriptures_index.json"),
            System.IO.Path.Combine("..", "Content", "scriptures_index.json"),
            System.IO.Path.Combine("..", "archive", "unity", "UnityProject", "Assets", "Resources", "scriptures_index.json")
        };
        foreach (var p in paths)
        {
            if (System.IO.File.Exists(p))
            {
                var txt = System.IO.File.ReadAllText(p);
                var res = JSON.Parse(txt);
                if (res.Error == Error.Ok) return (Godot.Collections.Array)res.Result;
            }
        }
        return null;
    }

    public static ScriptureEffect UseScripture(string scriptureId)
    {
        if (string.IsNullOrEmpty(scriptureId)) return ScriptureEffect.None;
        var idx = LoadIndex();
        if (idx == null) return ScriptureEffect.None;

        foreach (var e in idx)
        {
            var d = (Godot.Collections.Dictionary)e;
            if (d.Contains("id") && d["id"].ToString() == scriptureId)
            {
                var tags = d.Contains("tags") ? (Godot.Collections.Array)d["tags"] : null;
                if (tags != null && (tags.Contains("dispel") || tags.Contains("truth"))) return ScriptureEffect.DispelLies;
                if (tags != null && tags.Contains("weapon")) return ScriptureEffect.WeaponBoost;
                if (tags != null && tags.Contains("edification")) return ScriptureEffect.Edify;
            }
        }
        return ScriptureEffect.None;
    }

    public static bool EquipScripture(PlayerProfile profile, string scriptureId)
    {
        if (profile == null) return false;
        return profile.EquipScripture(scriptureId);
    }
}
