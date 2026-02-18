using UnityEngine;

/// <summary>
/// Courage trial prototype: time‑limited obstacle check. On success awards the quest (e.g., Courage tests tied to David/Goliath story).
/// </summary>
public class CourageTrialMiniGame : MiniGameBase
{
    public string questId;
    public float timeLimit = 10f;
    private float timer = 0f;
    private bool running = false;
    private int checkpoints = 0;
    public int requiredCheckpoints = 3;

    private void Update()
    {
        if (!running) return;
        timer += Time.deltaTime;
        if (timer > timeLimit) Fail();
    }

    public void StartTrial()
    {
        running = true; timer = 0f; checkpoints = 0;
    }

    public void ReachCheckpoint()
    {
        if (!running) return;
        checkpoints++;
        if (checkpoints >= requiredCheckpoints) Success();
    }

    private void Success()
    {
        running = false;
        CompleteMiniGame(questId);
    }

    private void Fail()
    {
        running = false;
        // show retry UI (not implemented)
    }
}