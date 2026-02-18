using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerProfile : MonoBehaviour
{
    public string playerId;
    public string displayName;
    public string dob; // ISO date string, e.g. 2012-05-20
    public string parentAccountId;
    public string denomId;
    public string churchId;

    // Age verification fields (production): set when a trusted provider verifies DOB/age.
    public bool ageVerified = false; // true when verified by provider
    public string ageVerifiedAt;     // ISO timestamp when verification occurred

    // Legacy/admin flags
    public bool adultModeEnabled = false;
    public string adultModeExpires; // ISO date string
    public bool adultModeDisabledByChurch = false;

    // Cloud saves are OFF by default for child accounts and require parental consent to enable.
    public bool cloudSaveEnabled = false; // requires parental consent
    public string cloudSaveConsentId; // reference to parental consent record (server)

    // Angel / safety state
    public string lastAngelReviveAt; // ISO timestamp of last angelic revive
    public bool isDown = false; // whether the player is currently "down" and needs revive


    // Spiritual collections and armor (owned by player)
    public string[] ownedFruits; // e.g., ["Love","Joy"]
    public string[] ownedArmorPieces; // e.g., ["BeltOfTruth","ShieldOfFaith"]
    public string[] ownedVirtues; // e.g., ["Wisdom","CommonSense"]
    public string[] ownedActivities; // e.g., ["Service","Mentoring"]

    public bool IsAdultModeAllowed()
    {
        // Church admin can always disable Mature Mode for a linked account
        if (adultModeDisabledByChurch) return false;

        // Production: require explicit age verification (trusted provider)
        if (ageVerified) return true;

        // Fallback: explicit adultModeEnabled (legacy/admin flag) — does not bypass verification in normal flows
        return adultModeEnabled;
    }

    /// <summary>
    /// Returns the player's age in years based on `dob` (ISO yyyy-MM-dd). Returns 0 if unknown/invalid.
    /// </summary>
    public int GetAge()
    {
        if (string.IsNullOrEmpty(dob)) return 0;
        if (!System.DateTime.TryParse(dob, out var dt)) return 0;
        var today = System.DateTime.UtcNow;
        int age = today.Year - dt.Year;
        if (dt > today.AddYears(-age)) age--;
        return age;
    }

    public bool IsAdultByAge(int minYears = 18)
    {
        return GetAge() >= minYears;
    }

    // Placeholder check — cloud saves must be enabled by parental consent on the backend.
    public bool CanUseCloudSave()
    {
        // TODO: verify parental consent record from server before allowing cloud sync.
        return cloudSaveEnabled;
    }

    // Equipped state (fruits and rainbow)
    public string[] equippedFruits; // currently equipped individual fruits (max 9)
    public bool isRainbowEquipped = false;

    private void Awake()
    {
        EnsureSpiritualArmor();
        if (ownedFruits == null) ownedFruits = new string[0];
        if (equippedFruits == null) equippedFruits = new string[0];
    }

    // Ensure every player has the full Ephesians 'Spiritual Armor' set by default
    public void EnsureSpiritualArmor()
    {
        if (ownedArmorPieces == null || ownedArmorPieces.Length == 0)
        {
            ownedArmorPieces = new string[] {
                "BeltOfTruth",
                "BreastplateOfRighteousness",
                "GospelOfPeaceShoes",
                "ShieldOfFaith",
                "HelmetOfSalvation",
                "SwordOfTheSpirit"
            };
        }
    }

    // Fruit ownership & equipment APIs
    public bool HasFruit(string fruitName)
    {
        return ownedFruits != null && ownedFruits.Contains(fruitName);
    }

    public void AddFruit(string fruitName)
    {
        if (string.IsNullOrEmpty(fruitName)) return;
        if (ownedFruits == null) ownedFruits = new string[0];
        if (!ownedFruits.Contains(fruitName))
        {
            var list = ownedFruits.ToList();
            list.Add(fruitName);
            ownedFruits = list.ToArray();
        }
    }

    // Virtue ownership APIs
    public bool HasVirtue(string virtueId)
    {
        return ownedVirtues != null && ownedVirtues.Contains(virtueId);
    }

    public void AddVirtue(string virtueId)
    {
        if (string.IsNullOrEmpty(virtueId)) return;
        if (ownedVirtues == null) ownedVirtues = new string[0];
        if (!ownedVirtues.Contains(virtueId))
        {
            var list = ownedVirtues.ToList();
            list.Add(virtueId);
            ownedVirtues = list.ToArray();
        }
    }

    // Activity ownership APIs
    public bool HasActivity(string activityId)
    {
        return ownedActivities != null && ownedActivities.Contains(activityId);
    }

    public void AddActivity(string activityId)
    {
        if (string.IsNullOrEmpty(activityId)) return;
        if (ownedActivities == null) ownedActivities = new string[0];
        if (!ownedActivities.Contains(activityId))
        {
            var list = ownedActivities.ToList();
            list.Add(activityId);
            ownedActivities = list.ToArray();
        }
    }

    // Quest completion tracking
    public string[] completedQuests; // e.g., ["WIS-001","ECHO-001"]

    // Truths & daily affirmation
    public string[] ownedTruths; // e.g., ["TRU-CHILD","TRU-LOVED"]
    public string lastAffirmationDate; // ISO  yyyy-MM-dd for daily affirmation

    // Scripture ownership & equipped scripture (open-book test / selectable weapon)
    public string[] ownedScriptures; // e.g., ["SCRIPT-EPH6-17","SCRIPT-PS23-1"]
    public string equippedScripture; // ID of the currently equipped scripture (used as 'weapon' or quick reference)

    public void AddCompletedQuest(string questId)
    {
        if (string.IsNullOrEmpty(questId)) return;
        if (completedQuests == null) completedQuests = new string[0];
        if (!completedQuests.Contains(questId))
        {
            var list = completedQuests.ToList();
            list.Add(questId);
            completedQuests = list.ToArray();
        }
    }

    // Truth ownership APIs
    public bool HasTruth(string truthId)
    {
        return ownedTruths != null && ownedTruths.Contains(truthId);
    }

    public void AddTruth(string truthId)
    {
        if (string.IsNullOrEmpty(truthId)) return;
        if (ownedTruths == null) ownedTruths = new string[0];
        if (!ownedTruths.Contains(truthId))
        {
            var list = ownedTruths.ToList();
            list.Add(truthId);
            ownedTruths = list.ToArray();
        }
    }

    // Scripture ownership & equipment APIs (open-book test behavior)
    public bool HasScripture(string scriptureId)
    {
        return ownedScriptures != null && ownedScriptures.Contains(scriptureId);
    }

    public void AddScripture(string scriptureId)
    {
        if (string.IsNullOrEmpty(scriptureId)) return;
        if (ownedScriptures == null) ownedScriptures = new string[0];
        if (!ownedScriptures.Contains(scriptureId))
        {
            var list = ownedScriptures.ToList();
            list.Add(scriptureId);
            ownedScriptures = list.ToArray();
        }
    }

    public bool EquipScripture(string scriptureId)
    {
        if (string.IsNullOrEmpty(scriptureId))
        {
            equippedScripture = null;
            return false;
        }
        if (!HasScripture(scriptureId)) return false;
        equippedScripture = scriptureId;
        return true;
    }

    public void UnequipScripture()
    {
        equippedScripture = null;
    }

    public string GetEquippedScripture()
    {
        return equippedScripture;
    }

    // Music / worship track ownership
    public string[] ownedMusicTracks; // ids of unlocked worship/music tracks

    public bool HasMusicTrack(string trackId)
    {
        return ownedMusicTracks != null && ownedMusicTracks.Contains(trackId);
    }

    public void AddMusicTrack(string trackId)
    {
        if (string.IsNullOrEmpty(trackId)) return;
        if (ownedMusicTracks == null) ownedMusicTracks = new string[0];
        if (!ownedMusicTracks.Contains(trackId))
        {
            var list = ownedMusicTracks.ToList();
            list.Add(trackId);
            ownedMusicTracks = list.ToArray();
        }
    }

    public bool IsStewardshipReady()
    {
        // Consider stewardship 'ready' if the player has the Stewardship virtue or has practiced stewardship activity.
        if (HasVirtue("Stewardship")) return true;
        if (HasActivity("StewardshipPractice")) return true;
        return false;
    }

    public bool EquipFruit(string fruitName)
    {
        if (!HasFruit(fruitName)) return false;
        if (isRainbowEquipped)
        {
            // Unequip rainbow first
            isRainbowEquipped = false;
            equippedFruits = new string[0];
        }

        var list = equippedFruits != null ? equippedFruits.ToList() : new System.Collections.Generic.List<string>();
        if (!list.Contains(fruitName)) list.Add(fruitName);
        equippedFruits = list.ToArray();
        return true;
    }

    public void UnequipFruit(string fruitName)
    {
        if (equippedFruits == null || equippedFruits.Length == 0) return;
        var list = equippedFruits.ToList();
        if (list.Contains(fruitName))
        {
            list.Remove(fruitName);
            equippedFruits = list.ToArray();
        }
    }

    public string[] GetEquippedFruits() => equippedFruits ?? new string[0];

    public bool IsRainbowUnlocked()
    {
        var required = new string[] {"Love","Joy","Peace","Patience","Kindness","Goodness","Faithfulness","Gentleness","Self-control"};
        if (ownedFruits == null) return false;
        return required.All(r => ownedFruits.Contains(r));
    }

    public bool EquipRainbow()
    {
        if (!IsRainbowUnlocked()) return false;
        isRainbowEquipped = true;
        equippedFruits = new string[] {"Love","Joy","Peace","Patience","Kindness","Goodness","Faithfulness","Gentleness","Self-control"};
        return true;
    }

    public void UnequipRainbow()
    {
        isRainbowEquipped = false;
        equippedFruits = new string[0];
    }

    public bool HasAllArmorPieces()
    {
        var required = new string[] {"BeltOfTruth","BreastplateOfRighteousness","GospelOfPeaceShoes","ShieldOfFaith","HelmetOfSalvation","SwordOfTheSpirit"};
        if (ownedArmorPieces == null) return false;
        foreach (var r in required) { bool found = System.Array.Exists(ownedArmorPieces, x => x == r); if (!found) return false; }
        return true;
    }
}