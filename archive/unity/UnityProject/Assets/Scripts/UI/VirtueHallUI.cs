using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minimal Virtue Hall UI controller — lists discoverable and owned virtues.
/// Wire to a scroll list + prefab in the Unity editor.
/// </summary>
public class VirtueHallUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Transform listParent;
    public GameObject virtueItemPrefab; // prefab should include Text for name and description

    private void OnEnable()
    {
        QuestManager.OnVirtueAwarded += OnVirtueAwardedHandler;
        Refresh();
    }

    private void OnDisable()
    {
        QuestManager.OnVirtueAwarded -= OnVirtueAwardedHandler;
    }

    private void OnVirtueAwardedHandler(string virtueId, PlayerProfile profile)
    {
        if (playerProfile == null) return;
        if (profile != null && profile.playerId == playerProfile.playerId)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        foreach (Transform t in listParent) Destroy(t.gameObject);
        var virtues = Resources.LoadAll<VirtueItem>("Virtues");
        // Ensure Stewardship is shown prominently as a kingdom responsibility
        var list = new System.Collections.Generic.List<VirtueItem>(virtues);
        var steward = list.Find(x => x.id == "Stewardship");
        if (steward != null)
        {
            list.Remove(steward);
            list.Insert(0, steward);
        }

        foreach (var v in list)
        {
            var go = Instantiate(virtueItemPrefab, listParent, false);
            var texts = go.GetComponentsInChildren<Text>();
            var title = v.displayName + (v.id == "Stewardship" ? " — Kingdom responsibility" : "");
            if (texts.Length > 0) texts[0].text = title;
            if (texts.Length > 1) texts[1].text = v.description;

            var owned = playerProfile != null && playerProfile.ownedVirtues != null && System.Array.Exists(playerProfile.ownedVirtues, x => x == v.id);
            var btn = go.GetComponentInChildren<Button>();
            btn.interactable = !owned; // claim if not owned (will check quest completion)
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnClaimButton(v.id, btn));
        }
    }

    private void OnClaimButton(string virtueId, UnityEngine.UI.Button btn)
    {
        if (playerProfile == null) return;
        var ok = QuestManager.TryClaimVirtue(playerProfile, virtueId);
        if (ok)
        {
            Refresh();
            Debug.Log($"Virtue {virtueId} claimed and awarded.");
        }
        else
        {
            // Could show UI toast — here we log and optionally show a small message in the button label
            Debug.Log($"Cannot claim virtue {virtueId}. Complete the associated quest first.");
        }
    }
}