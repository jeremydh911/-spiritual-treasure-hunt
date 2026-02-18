using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple Affirmation UI that shows today's truth and lets the player 'complete' the affirmation.
/// </summary>
public class AffirmationUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Text truthTitleText;
    public Text truthBodyText;
    public Button completeButton;
    public string truthId = "TRU-CHILD"; // default

    private void OnEnable()
    {
        Refresh();
        if (completeButton != null) completeButton.onClick.AddListener(OnComplete);
    }

    private void OnDisable()
    {
        if (completeButton != null) completeButton.onClick.RemoveListener(OnComplete);
    }

    public void Refresh()
    {
        var ta = Resources.Load<TextAsset>("Truths/truths_index");
        if (ta == null) return;
        // Use simple parsing for prototype: find truth by id in the JSON (production: parse JSON properly)
        var json = ta.text;
        // crude extraction for prototype
        truthTitleText.text = truthId;
        truthBodyText.text = "Remember: you belong to God. Say this truth aloud and help someone else.";
        completeButton.interactable = !AffirmationManager.HasCompletedToday(playerProfile);
    }

    private void OnComplete()
    {
        AffirmationManager.CompleteToday(playerProfile, truthId);
        ToastManager.Instance?.ShowToast("Affirmation complete — truth added to your Truths Hall");
        Refresh();
    }
}