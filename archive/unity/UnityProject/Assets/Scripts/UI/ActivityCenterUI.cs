using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple Activity Center UI: lists discoverable and owned activities.
/// Hook to Content/Activities or ScriptableObjects for production.
/// </summary>
public class ActivityCenterUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Transform listParent;
    public GameObject activityItemPrefab; // prefab with Text for name + description and Claim button

    private void OnEnable()
    {
        QuestManager.OnActivityAwarded += HandleActivityAwarded;
        Refresh();
    }

    private void OnDisable()
    {
        QuestManager.OnActivityAwarded -= HandleActivityAwarded;
    }

    private void HandleActivityAwarded(string activityId, PlayerProfile profile)
    {
        if (profile != playerProfile) return;
        ToastManager.Instance?.ShowToast($"Activity awarded: {activityId}");
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform t in listParent) Destroy(t.gameObject);
        var ta = Resources.Load<TextAsset>("../Content/Activities/activities_index");
        // For prototype, load JSON directly from Content folder via Resources if available, else use ScriptableObjects

        // Fallback: show activities from a small hardcoded list for the prototype
        // StewardshipPractice is prioritized as a core kingdom responsibility
        string[] ids = new string[] { "StewardshipPractice", "Service", "Mentoring", "SabbathRest", "ArtisticExpression", "Accountability" };
        foreach (var id in ids)
        {
            var go = Instantiate(activityItemPrefab, listParent, false);
            var texts = go.GetComponentsInChildren<Text>();
            if (texts.Length > 0) texts[0].text = id + (id == "StewardshipPractice" ? " — Kingdom responsibility" : "");
            if (texts.Length > 1) texts[1].text = id == "StewardshipPractice" ? "Practice faithful care of God’s gifts and lead by example." : "Discover and grow through this activity.";
            var btn = go.GetComponentInChildren<Button>();
            bool owned = playerProfile != null && playerProfile.ownedActivities != null && System.Array.Exists(playerProfile.ownedActivities, x => x == id);
            btn.interactable = !owned;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnClaimActivity(id));
        }
    }

    private void OnClaimActivity(string id)
    {
        if (playerProfile == null) return;
        playerProfile.AddActivity(id);
        SaveManager.SaveLocalProfile(playerProfile);
        Refresh();
        ToastManager.Instance?.ShowToast($"Activity added: {id}");
    }
}