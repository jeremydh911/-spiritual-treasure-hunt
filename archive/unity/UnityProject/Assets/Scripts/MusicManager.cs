using System.Linq;
using UnityEngine;

public static class MusicManager
{
    private class MusicJson { public string id; public string title; public string description; public string assetPath; }
    private static MusicJson[] _index = null;

    private static void EnsureLoaded()
    {
        if (_index != null) return;
        var ta = Resources.Load<TextAsset>("music_index");
        if (ta == null) { _index = new MusicJson[0]; return; }
        _index = JsonUtility.FromJson<Wrapper>("{\"items\": " + ta.text + "}").items;
    }

    [System.Serializable]
    private class Wrapper { public MusicJson[] items; }

    public static MusicJson[] GetAll() { EnsureLoaded(); return _index; }
    public static MusicJson GetById(string id) { EnsureLoaded(); return _index.FirstOrDefault(m => m.id == id); }

    public static bool UnlockTrack(PlayerProfile profile, string trackId)
    {
        if (profile == null || string.IsNullOrEmpty(trackId)) return false;
        var track = GetById(trackId);
        if (track == null) { Debug.LogWarning($"Music track not found: {trackId}"); return false; }
        if (profile.HasMusicTrack(trackId)) return false;
        profile.AddMusicTrack(trackId);
        SaveManager.SaveLocalProfile(profile);
        OnTrackUnlocked?.Invoke(trackId, profile);
        Debug.Log($"Unlocked music track: {trackId}");
        return true;
    }

    public static event QuestManager.QuestEvent OnTrackUnlocked;
}