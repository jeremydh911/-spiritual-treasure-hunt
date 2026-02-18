using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Displays Mature Mode content if allowed and can request printable guides from the backend.
/// </summary>
public class MatureContentUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public Text titleText;
    public Text bodyText;
    public Button downloadGuideButton;
    public Button verifyAgeButton; // new: opens age verification UI (demo)
    public AgeVerificationUI ageVerificationUI; // optional reference to the UI panel
    public string contentKey; // e.g., "Nephilim"

    private void OnEnable()
    {
        Refresh();
        if (downloadGuideButton != null) downloadGuideButton.onClick.AddListener(OnDownloadGuide);
        if (verifyAgeButton != null) verifyAgeButton.onClick.AddListener(OnVerifyAgeClicked);
    }

    private void OnDisable()
    {
        if (downloadGuideButton != null) downloadGuideButton.onClick.RemoveListener(OnDownloadGuide);
        if (verifyAgeButton != null) verifyAgeButton.onClick.RemoveListener(OnVerifyAgeClicked);
    }

    public void Refresh()
    {
        if (!MatureContentManager.IsMatureContentAllowed(playerProfile))
        {
            titleText.text = "Mature content locked";
            bodyText.text = "Mature Mode requires age verification (18+). Verify your age or ask a church admin to enable Mature Mode for your account.";
            downloadGuideButton.interactable = false;
            if (verifyAgeButton != null) verifyAgeButton.interactable = true;
            return;
        }

        var body = MatureContentManager.LoadMatureText(contentKey);
        titleText.text = contentKey;
        bodyText.text = body ?? "Content not found.";
        downloadGuideButton.interactable = true;
        if (verifyAgeButton != null) verifyAgeButton.interactable = false;
    }

    private void OnVerifyAgeClicked()
    {
        if (ageVerificationUI != null && playerProfile != null)
        {
            ageVerificationUI.ShowForProfile(playerProfile);
            return;
        }

        // Fallback: instruct user to enter DOB in profile settings
        titleText.text = "Age verification";
        bodyText.text = "Enter your date of birth in your profile settings to verify age (demo).";
    }

    private void OnDownloadGuide()
    {
        if (playerProfile == null) return;
        StartCoroutine(DownloadGuideCoroutine(contentKey));
    }

    private IEnumerator DownloadGuideCoroutine(string key)
    {
        var url = $"http://localhost:4000/guides/{key}.md";
        using (var www = new UnityEngine.Networking.UnityWebRequest(url))
        {
            www.method = "GET";
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Download failed: " + www.error);

                // FALLBACK: try to save a local copy from Resources so learning content is available offline
                var saved = SaveLocalGuideToDisk(key);
                if (saved)
                {
                    ToastManager.Instance?.ShowToast("Guide saved locally for offline access.");
                    Debug.Log($"Saved local guide for key={key} to persistent path.");
                }
                else
                {
                    ToastManager.Instance?.ShowToast("Guide download failed and no local copy available.");
                }
            }
            else
            {
                var text = www.downloadHandler.text;
                // Save to persistent data path for parent access (simple demo)
                var path = System.IO.Path.Combine(Application.persistentDataPath, $"{key}_guide.md");
                System.IO.File.WriteAllText(path, text);
                ToastManager.Instance?.ShowToast("Guide downloaded: " + path);
                Debug.Log("Guide saved to: " + path);
            }
        }
    }

    /// <summary>
    /// Save a local (Resources) copy of the guide to persistent storage.
    /// Returns true if a local resource existed and was written to disk.
    /// This ensures important learning material is available offline.
    /// </summary>
    public bool SaveLocalGuideToDisk(string key)
    {
        var body = MatureContentManager.LoadMatureText(key);
        if (string.IsNullOrEmpty(body)) return false;
        try
        {
            var path = System.IO.Path.Combine(Application.persistentDataPath, $"{key}_guide.md");
            System.IO.File.WriteAllText(path, body);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Failed to save local guide: " + ex.Message);
            return false;
        }
    }
}