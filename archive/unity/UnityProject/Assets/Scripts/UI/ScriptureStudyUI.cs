using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI for exploring (open‑book test) scriptures, marking them as learned, and equipping one as the active 'scripture weapon' or quick reference.
/// - Expects `scriptureItemPrefab` to contain two Text components (reference + snippet) and two Buttons (Learn / Equip).
/// </summary>
public class ScriptureStudyUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Transform listParent;
    public GameObject scriptureItemPrefab;

    private void OnEnable()
    {
        ScriptureManager.OnScriptureLearned += OnScriptureChanged;
        ScriptureManager.OnScriptureEquipped += OnScriptureChanged;
        Refresh();
    }

    private void OnDisable()
    {
        ScriptureManager.OnScriptureLearned -= OnScriptureChanged;
        ScriptureManager.OnScriptureEquipped -= OnScriptureChanged;
    }

    void OnScriptureChanged(string id, PlayerProfile profile)
    {
        if (profile == playerProfile) Refresh();
    }

    public void Refresh()
    {
        if (listParent == null || scriptureItemPrefab == null) return;
        foreach (Transform t in listParent) Destroy(t.gameObject);
        var all = ScriptureManager.GetAll();
        foreach (var s in all)
        {
            var go = Instantiate(scriptureItemPrefab, listParent, false);
            var texts = go.GetComponentsInChildren<Text>();
            if (texts.Length > 0) texts[0].text = s.reference;
            if (texts.Length > 1) texts[1].text = s.text;

            var buttons = go.GetComponentsInChildren<Button>();
            Button learnBtn = buttons.Length > 0 ? buttons[0] : null;
            Button equipBtn = buttons.Length > 1 ? buttons[1] : null;

            bool learned = playerProfile != null && playerProfile.HasScripture(s.id);
            if (learnBtn != null)
            {
                learnBtn.interactable = !learned;
                learnBtn.onClick.RemoveAllListeners();
                learnBtn.onClick.AddListener(() => {
                    if (playerProfile == null) return;
                    var ok = ScriptureManager.LearnScripture(playerProfile, s.id);
                    if (ok) ToastManager.Instance?.ShowToast($"Learned: {s.reference}");
                    Refresh();
                });
            }

            if (equipBtn != null)
            {
                equipBtn.interactable = learned;
                equipBtn.onClick.RemoveAllListeners();
                equipBtn.onClick.AddListener(() => {
                    if (playerProfile == null) return;
                    var ok = ScriptureManager.EquipScripture(playerProfile, s.id);
                    if (ok) ToastManager.Instance?.ShowToast($"Equipped: {s.reference}");
                    Refresh();
                });
            }
        }
    }
}