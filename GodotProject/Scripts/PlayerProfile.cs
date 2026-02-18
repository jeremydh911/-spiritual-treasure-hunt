using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Lightweight C# port of the Unity PlayerProfile for Godot.
public partial class PlayerProfile : Node
{
    public string playerId = "";
    public string displayName = "";
    public string dob = ""; // ISO yyyy-MM-dd
    public bool adultModeEnabled = false; // legacy/admin flag
    public bool adultModeDisabledByChurch = false;

    public List<string> ownedFruits = new List<string>();
    public List<string> equippedFruits = new List<string>();
    public bool isRainbowEquipped = false;

    public List<string> ownedVirtues = new List<string>();
    public List<string> ownedActivities = new List<string>();

    public List<string> ownedScriptures = new List<string>();
    public string equippedScripture = null;

    public List<string> ownedMusicTracks = new List<string>();

    public int GetAge()
    {
        if (string.IsNullOrEmpty(dob)) return 0;
        if (!DateTime.TryParse(dob, out var dt)) return 0;
        var today = DateTime.UtcNow;
        int age = today.Year - dt.Year;
        if (dt > today.AddYears(-age)) age--;
        return age;
    }

    public bool IsAdultByAge(int minYears = 18) => GetAge() >= minYears;

    public bool IsAdultModeAllowed()
    {
        if (adultModeDisabledByChurch) return false;
        if (IsAdultByAge(18)) return true;
        return adultModeEnabled;
    }

    // Simple collection helpers
    public bool HasFruit(string fruitName) => ownedFruits.Contains(fruitName);
    public void AddFruit(string fruitName) { if (!ownedFruits.Contains(fruitName)) ownedFruits.Add(fruitName); }

    public bool HasVirtue(string v) => ownedVirtues.Contains(v);
    public void AddVirtue(string v) { if (!ownedVirtues.Contains(v)) ownedVirtues.Add(v); }

    public bool HasScripture(string id) => ownedScriptures.Contains(id);
    public void AddScripture(string id) { if (!ownedScriptures.Contains(id)) ownedScriptures.Add(id); }
    public bool EquipScripture(string id) { if (!HasScripture(id)) return false; equippedScripture = id; return true; }
    public void UnequipScripture() { equippedScripture = null; }
    public string GetEquippedScripture() => equippedScripture;

    public bool HasMusicTrack(string id) => ownedMusicTracks.Contains(id);
    public void AddMusicTrack(string id) { if (!ownedMusicTracks.Contains(id)) ownedMusicTracks.Add(id); }

    public bool IsStewardshipReady() => HasVirtue("Stewardship") || ownedActivities.Contains("StewardshipPractice");
}
