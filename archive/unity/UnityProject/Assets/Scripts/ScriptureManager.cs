using System.Linq;
using UnityEngine;

/// <summary>
/// Load scriptures from Resources/scriptures_index.json and provide APIs to learn/equip/use scriptures.
/// - Scriptures can have tags: "weapon", "edification", "prayer", "memory" etc.
/// - UseScripture returns a ScriptureEffect for the calling gameplay logic to act on.
/// </summary>
public static class ScriptureManager
{
    public enum ScriptureEffect { None, WeaponBoost, Edification, PrayerBuff }

    private class ScriptureJson { public string id; public string reference; public string text; public string[] tags; }
    private static ScriptureJson[] _index = null;

    private static void EnsureLoaded()
    {
        if (_index != null) return;
        var ta = Resources.Load<TextAsset>("scriptures_index");
        if (ta == null)
        {
            _index = new ScriptureJson[0];
            Debug.LogWarning("Scripture index not found: scriptures_index.json");
            return;
        }
        _index = JsonUtility.FromJson<Wrapper>("{\"items\": " + ta.text + "}").items;
    }

    [System.Serializable]
    private class Wrapper { public ScriptureJson[] items; }

    public static ScriptureJson[] GetAll() { EnsureLoaded(); return _index; }

    public static ScriptureJson GetById(string id) { EnsureLoaded(); return _index.FirstOrDefault(s => s.id == id); }

    public static bool LearnScripture(PlayerProfile profile, string id)
    {
        if (profile == null || string.IsNullOrEmpty(id)) return false;
        var s = GetById(id);
        if (s == null) { Debug.LogWarning($"Scripture not found: {id}"); return false; }
        if (profile.HasScripture(id)) return false;
        profile.AddScripture(id);
        SaveManager.SaveLocalProfile(profile);
        OnScriptureLearned?.Invoke(id, profile);
        return true;
    }

    public static bool EquipScripture(PlayerProfile profile, string id)
    {
        if (profile == null) return false;
        if (string.IsNullOrEmpty(id)) { profile.UnequipScripture(); SaveManager.SaveLocalProfile(profile); OnScriptureEquipped?.Invoke(null, profile); return true; }
        if (!profile.HasScripture(id)) return false;
        var ok = profile.EquipScripture(id);
        if (ok)
        {
            SaveManager.SaveLocalProfile(profile);
            OnScriptureEquipped?.Invoke(id, profile);
        }
        return ok;
    }

    public static ScriptureEffect UseScripture(string scriptureId)
    {
        var s = GetById(scriptureId);
        if (s == null) return ScriptureEffect.None;
        var tags = s.tags ?? new string[0];
        // priority: dispel truth/lies, weapon, prayer, edification
        if (tags.Contains("dispel") || tags.Contains("truth")) return ScriptureEffect.DispelLies;
        if (tags.Contains("weapon")) return ScriptureEffect.WeaponBoost;
        if (tags.Contains("prayer")) return ScriptureEffect.PrayerBuff;
        if (tags.Contains("edification")) return ScriptureEffect.Edification;
        return ScriptureEffect.None;
    }

    public static event QuestManager.QuestEvent OnScriptureLearned;
    public static event QuestManager.QuestEvent OnScriptureEquipped;
}
