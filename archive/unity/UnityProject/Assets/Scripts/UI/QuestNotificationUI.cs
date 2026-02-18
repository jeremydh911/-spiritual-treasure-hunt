using UnityEngine;

/// <summary>
/// Subscribes to QuestManager events and shows simple UI feedback via ToastManager.
/// Attach to a UI Canvas and ensure ToastManager.Instance is present.
/// </summary>
public class QuestNotificationUI : MonoBehaviour
{
    private void OnEnable()
    {
        QuestManager.OnQuestCompleted += OnQuestCompleted;
        QuestManager.OnVirtueAwarded += OnVirtueAwarded;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestCompleted -= OnQuestCompleted;
        QuestManager.OnVirtueAwarded -= OnVirtueAwarded;
    }

    private void OnQuestCompleted(string questId, PlayerProfile profile)
    {
        ToastManager.Instance?.ShowToast($"Quest complete: {questId}");
    }

    private void OnVirtueAwarded(string virtueId, PlayerProfile profile)
    {
        ToastManager.Instance?.ShowToast($"Virtue awarded: {virtueId}");
    }
}