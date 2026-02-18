using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// QuestManager handles quest completion, reward processing (fruits/virtues), local save and cloud sync stub.
/// - Loads a local quest index (JSON in Resources)
/// - CompleteQuest marks completion and applies rewards to the PlayerProfile
/// - TryClaimVirtue checks completion and awards virtue
/// Events: OnQuestCompleted, OnVirtueAwarded
/// </summary>
public static class QuestManager
{
    public delegate void QuestEvent(string id, PlayerProfile profile);
    public static event QuestEvent OnQuestCompleted;
    public static event QuestEvent OnVirtueAwarded;
    public static event QuestEvent OnActivityAwarded; // fired when an activity reward is granted

    private class QuestDTO { public string id; public string virtueReward; public string fruitReward; public string activityReward; public string sensitivity; public string musicTrackReward; }
    private static QuestDTO[] _questsIndex = null;

    private static void EnsureIndexLoaded()
    {
        if (_questsIndex != null) return;
        var ta = Resources.Load<TextAsset>("phase1_quests");
        if (ta == null)
        {
            Debug.LogWarning("Quest index resource not found: phase1_quests.json");
            _questsIndex = new QuestDTO[0];
            return;
        }
        // Basic JSON parsing: reuse existing phase1_quests.json but map only necessary fields
        var wrapper = JsonUtility.FromJson<Wrapper>("{\"items\": " + ta.text + "}");
        _questsIndex = wrapper.items.Select(i => new QuestDTO { id = i.id, virtueReward = i.virtueReward, fruitReward = i.fruitReward, activityReward = i.activityReward, sensitivity = i.sensitivity, musicTrackReward = i.musicTrackReward }).ToArray();
    }

    // JSON wrapper types used only for parsing the resource file
    [System.Serializable]
    private class Wrapper { public QuestJson[] items; }
    [System.Serializable]
    private class QuestJson { public string id; public string virtueReward; public string fruitReward; public string activityReward; public string sensitivity; public string musicTrackReward; }

    public static bool IsQuestCompleted(PlayerProfile profile, string questId)
    {
        if (profile == null) return false;
        if (profile.completedQuests == null) return false;
        return profile.completedQuests.Contains(questId);
    }

    public static bool CompleteQuest(PlayerProfile profile, string questId)
    {
        if (profile == null || string.IsNullOrEmpty(questId)) return false;
        if (IsQuestCompleted(profile, questId)) return false;

        // mark completed
        var list = profile.completedQuests != null ? profile.completedQuests.ToList() : new List<string>();
        list.Add(questId);
        profile.completedQuests = list.ToArray();

        // find rewards in index
        EnsureIndexLoaded();
        var q = _questsIndex.FirstOrDefault(x => x.id == questId);
        if (q != null)
        {
            // Gate mature‑sensitivity quests
            if (!string.IsNullOrEmpty(q.sensitivity) && q.sensitivity.ToLower() == "mature")
            {
                if (!MatureContentManager.IsMatureContentAllowed(profile))
                {
                    Debug.LogWarning($"Quest '{questId}' requires Mature Mode and cannot be completed by this profile.");
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(q.fruitReward)) profile.AddFruit(q.fruitReward);
            if (!string.IsNullOrEmpty(q.virtueReward))
            {
                profile.AddVirtue(q.virtueReward);
                OnVirtueAwarded?.Invoke(q.virtueReward, profile);
            }
            if (!string.IsNullOrEmpty(q.activityReward))
            {
                profile.AddActivity(q.activityReward);
                OnActivityAwarded?.Invoke(q.activityReward, profile);
            }
            if (!string.IsNullOrEmpty(q.musicTrackReward))
            {
                // unlock a worship/music track for the player
                profile.AddMusicTrack(q.musicTrackReward);
                MusicManager.UnlockTrack(profile, q.musicTrackReward);
            }
        }

        // persist and notify listeners
        SaveManager.SaveLocalProfile(profile);
        OnQuestCompleted?.Invoke(questId, profile);
        Debug.Log($"Quest completed: {questId}");
        return true;
            }

    #if UNITY_EDITOR
    /// <summary>
    /// Test helper (Editor only) — add or replace an in-memory quest used by tests.
    /// </summary>
    public static void AddOrReplaceTestQuest(string id, string virtueReward = null, string fruitReward = null, string activityReward = null, string sensitivity = null, string musicTrackReward = null)
    {
        EnsureIndexLoaded();
        var list = _questsIndex.ToList();
        var existing = list.FirstOrDefault(x => x.id == id);
        if (existing != null) list.Remove(existing);
        list.Add(new QuestDTO { id = id, virtueReward = virtueReward, fruitReward = fruitReward, activityReward = activityReward, sensitivity = sensitivity, musicTrackReward = musicTrackReward });
        _questsIndex = list.ToArray();
    }
    #endif

    /// <summary>
    /// Returns whether the given profile is allowed to start/complete the quest (respecting sensitivity gating).
    /// </summary>
    public static bool CanStartQuest(PlayerProfile profile, string questId)
    {
        if (string.IsNullOrEmpty(questId)) return false;
        EnsureIndexLoaded();
        var q = _questsIndex.FirstOrDefault(x => x.id == questId);
        if (q == null) return true;
        if (!string.IsNullOrEmpty(q.sensitivity) && q.sensitivity.ToLower() == "mature")
        {
            return MatureContentManager.IsMatureContentAllowed(profile);
        }
        return true;
    }

    // Attempt to award a virtue only if its associated quest was completed
    public static bool TryClaimVirtue(PlayerProfile profile, string virtueId)
    {
        if (profile == null || string.IsNullOrEmpty(virtueId)) return false;
        EnsureIndexLoaded();
        var q = _questsIndex.FirstOrDefault(x => x.virtueReward == virtueId);
        if (q == null)
        {
            Debug.LogWarning($"No quest maps to virtue {virtueId}." );
            return false;
        }
        if (!IsQuestCompleted(profile, q.id))
        {
            Debug.Log($"Quest {q.id} not completed yet for virtue {virtueId}.");
            return false;
        }
        if (profile.HasVirtue(virtueId))
        {
            Debug.Log($"Player already has virtue {virtueId}.");
            return false;
        }

        profile.AddVirtue(virtueId);
        SaveManager.SaveLocalProfile(profile);
        OnVirtueAwarded?.Invoke(virtueId, profile);
        Debug.Log($"Virtue awarded: {virtueId}");
        return true;
    }
}
