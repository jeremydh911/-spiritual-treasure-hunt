using UnityEngine;

/// <summary>
/// Small debug/test helper to mark phase1 quests complete from the Unity inspector.
/// Use to simulate quest completion and verify virtue awarding + saving.
/// </summary>
public class QuestCompletionTester : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public string questIdToComplete;

    public void CompleteQuestNow()
    {
        if (playerProfile == null) Debug.LogWarning("Assign PlayerProfile in inspector.");
        else QuestManager.CompleteQuest(playerProfile, questIdToComplete);
    }
}