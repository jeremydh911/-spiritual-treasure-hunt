using System.IO;
using UnityEngine;

/// <summary>
/// Simple local save manager for prototype purposes.
/// - Saves minimal PlayerProfile fields to JSON in Application.persistentDataPath
/// - SyncToCloud is a stub that checks parental consent and would call backend APIs
/// </summary>
public static class SaveManager
{
    private static string SaveFolder => Path.Combine(Application.persistentDataPath, "saves");

    public class PlayerData
    {
        public string playerId;
        public string[] ownedFruits;
        public string[] ownedArmorPieces;
        public string[] ownedVirtues;
        public string[] ownedActivities;
        public string[] ownedScriptures;
        public string[] ownedMusicTracks;
        public string equippedScripture;
        public string[] equippedFruits;
        public bool isRainbowEquipped;
        public string[] completedQuests;
        public bool cloudSaveEnabled;
        public string lastAngelReviveAt;
        public bool isDown;
    }

    public static void SaveLocalProfile(PlayerProfile profile)
    {
        if (profile == null || string.IsNullOrEmpty(profile.playerId)) return;
        try
        {
            if (!Directory.Exists(SaveFolder)) Directory.CreateDirectory(SaveFolder);
            var data = new PlayerData
            {
                playerId = profile.playerId,
                ownedFruits = profile.ownedFruits,
                ownedArmorPieces = profile.ownedArmorPieces,
                ownedVirtues = profile.ownedVirtues,
                ownedActivities = profile.ownedActivities,
                ownedScriptures = profile.ownedScriptures,
                ownedMusicTracks = profile.ownedMusicTracks,
                equippedScripture = profile.equippedScripture,
                equippedFruits = profile.equippedFruits,
                isRainbowEquipped = profile.isRainbowEquipped,
                completedQuests = profile.completedQuests,
                cloudSaveEnabled = profile.cloudSaveEnabled,
                lastAngelReviveAt = profile.lastAngelReviveAt,
                isDown = profile.isDown
            };
            var json = JsonUtility.ToJson(data, true);
            var path = Path.Combine(SaveFolder, $"player_{profile.playerId}.json");
            File.WriteAllText(path, json);
            Debug.Log($"Saved profile locally: {path}");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("SaveLocalProfile failed: " + ex.Message);
        }
    }

    public static bool LoadLocalProfile(PlayerProfile profile)
    {
        if (profile == null || string.IsNullOrEmpty(profile.playerId)) return false;
        var path = Path.Combine(SaveFolder, $"player_{profile.playerId}.json");
        if (!File.Exists(path)) return false;
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<PlayerData>(json);
            profile.ownedFruits = data.ownedFruits ?? new string[0];
            profile.ownedArmorPieces = data.ownedArmorPieces ?? new string[0];
            profile.ownedVirtues = data.ownedVirtues ?? new string[0];
            profile.ownedActivities = data.ownedActivities ?? new string[0];
            profile.ownedScriptures = data.ownedScriptures ?? new string[0];
            profile.ownedMusicTracks = data.ownedMusicTracks ?? new string[0];
            profile.equippedScripture = data.equippedScripture;
            profile.equippedFruits = data.equippedFruits ?? new string[0];
            profile.isRainbowEquipped = data.isRainbowEquipped;
            profile.completedQuests = data.completedQuests ?? new string[0];
            profile.cloudSaveEnabled = data.cloudSaveEnabled;
            profile.lastAngelReviveAt = data.lastAngelReviveAt;
            profile.isDown = data.isDown;
            Debug.Log($"Loaded local profile: {path}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("LoadLocalProfile failed: " + ex.Message);
            return false;
        }
    }

    // Stub: would securely upload profile data to cloud endpoint if parental consent exists.
    public static void SyncToCloudIfAllowed(PlayerProfile profile)
    {
        if (profile == null) return;
        if (!profile.CanUseCloudSave())
        {
            Debug.Log("Cloud sync skipped — parental consent required.");
            return;
        }
        // TODO: implement secure cloud upload with parental consent verification
        Debug.Log("[stub] Syncing profile to cloud for player: " + profile.playerId);
    }
}