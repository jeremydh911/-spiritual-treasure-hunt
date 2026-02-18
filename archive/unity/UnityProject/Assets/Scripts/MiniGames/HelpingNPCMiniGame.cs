using UnityEngine;

/// <summary>
/// Helping NPC mini-game prototype: simple sequence of tasks (click to help). On completion awards the quest.
/// </summary>
public class HelpingNPCMiniGame : MiniGameBase
{
    public string questId;
    public int tasks = 3;
    private int completed = 0;

    public void DoTask()
    {
        completed++;
        if (completed >= tasks) OnWin();
    }

    private void OnWin()
    {
        CompleteMiniGame(questId);
    }
}