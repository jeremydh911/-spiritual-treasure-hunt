using UnityEngine;

/// <summary>
/// Base class for mini‑games. Call CompleteMiniGame(questId) when the mini‑game is won.
/// This delegates to QuestManager to award rewards and persist progress.
/// </summary>
public abstract class MiniGameBase : MonoBehaviour
{
    public PlayerProfile playerProfile;

    protected void CompleteMiniGame(string questId)
    {
        if (playerProfile == null)
        {
            Debug.LogWarning("MiniGameBase: playerProfile is not assigned.");
            return;
        }
        var ok = QuestManager.CompleteQuest(playerProfile, questId);
        if (ok)
        {
            TelemetryManager.LogEvent($"minigame_complete:{questId}");
        }
    }
}