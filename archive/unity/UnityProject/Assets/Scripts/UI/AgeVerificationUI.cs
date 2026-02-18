using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Simple demo Age Verification UI.
/// - Validates DOB locally (AgeVerificationApi)
/// - Posts to demo backend /verify/age and updates PlayerProfile on success
/// Production: replace backend call with a verified provider integration.
/// </summary>
public class AgeVerificationUI : MonoBehaviour
{
    public GameObject panelRoot; // panel to show/hide
    public InputField dobInput; // ISO yyyy-MM-dd expected
    public Button submitButton;
    public Text statusText;

    private PlayerProfile currentProfile;

    private void Awake()
    {
        if (submitButton != null) submitButton.onClick.AddListener(OnSubmitClicked);
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    public void ShowForProfile(PlayerProfile profile)
    {
        currentProfile = profile;
        if (panelRoot != null) panelRoot.SetActive(true);
        statusText.text = "";
        if (dobInput != null) dobInput.text = profile?.dob ?? "";
    }

    public void Hide()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        currentProfile = null;
    }

    private void OnSubmitClicked()
    {
        if (dobInput == null || currentProfile == null)
        {
            statusText.text = "Missing input or profile.";
            return;
        }

        var dob = dobInput.text.Trim();
        if (!AgeVerificationApi.IsAdultDob(dob))
        {
            statusText.text = "Local check failed — must be 18+. Change DOB or ask a church admin.";
            return;
        }

        // Optimistically set DOB locally; server verification will confirm
        currentProfile.dob = dob;
        StartCoroutine(VerifyOnServerCoroutine(currentProfile.playerId, dob));
    }

    private IEnumerator VerifyOnServerCoroutine(string playerId, string dob)
    {
        var url = "http://localhost:4000/verify/age";
        var reqObj = new VerifyRequest() { playerId = playerId, dob = dob };
        var json = JsonUtility.ToJson(reqObj);
        var uwr = new UnityEngine.Networking.UnityWebRequest(url, "POST");
        var bytes = Encoding.UTF8.GetBytes(json);
        uwr.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bytes);
        uwr.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            statusText.text = "Verification request failed: " + uwr.error;
            yield break;
        }

        var resp = JsonUtility.FromJson<VerifyResponse>(uwr.downloadHandler.text ?? "{}");
        if (resp == null)
        {
            statusText.text = "Invalid server response.";
            yield break;
        }

        if (resp.ageVerified)
        {
            statusText.text = "Age verified — Mature Mode available if allowed by church settings.";
            // reflect verification on local profile (demo)
            if (currentProfile != null)
            {
                currentProfile.dob = dob;
                currentProfile.ageVerified = true;
                currentProfile.ageVerifiedAt = System.DateTime.UtcNow.ToString("o");
            }
            // send event for telemetry/demo
            TelemetryManager.LogEvent($"age_verified:{playerId}");
            // optionally close the panel after success
            yield return new WaitForSeconds(0.8f);
            Hide();
        }
        else
        {
            statusText.text = "Age not verified (must be 18+).";
        }
    }

    // DEMO: Provider token verification (e.g. Persona). Sends token to backend /verify/age/provider
    public void VerifyWithProvider(string providerName, string token)
    {
        if (currentProfile == null)
        {
            statusText.text = "No profile selected.";
            return;
        }
        StartCoroutine(VerifyWithProviderCoroutine(currentProfile.playerId, providerName, token));
    }

    private System.Collections.IEnumerator VerifyWithProviderCoroutine(string playerId, string providerName, string token)
    {
        var url = "http://localhost:4000/verify/age/provider";
        var reqObj = new ProviderVerifyRequest() { playerId = playerId, providerName = providerName, token = token };
        var json = JsonUtility.ToJson(reqObj);
        var uwr = new UnityEngine.Networking.UnityWebRequest(url, "POST");
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        uwr.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bytes);
        uwr.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            statusText.text = "Provider verification request failed: " + uwr.error;
            yield break;
        }

        var resp = JsonUtility.FromJson<ProviderVerifyResponse>(uwr.downloadHandler.text ?? "{}");
        if (resp == null)
        {
            statusText.text = "Invalid server response from provider verification.";
            yield break;
        }

        if (resp.ageVerified)
        {
            statusText.text = "Age verified via provider.";
            if (currentProfile != null)
            {
                currentProfile.ageVerified = true;
                currentProfile.ageVerifiedAt = System.DateTime.UtcNow.ToString("o");
            }
            TelemetryManager.LogEvent($"age_verified_provider:{providerName}:{playerId}");
            yield return new WaitForSeconds(0.6f);
            Hide();
        }
        else
        {
            statusText.text = "Provider verification failed or token invalid.";
        }
    }

    [System.Serializable]
    private class VerifyRequest { public string playerId; public string dob; }

    [System.Serializable]
    private class VerifyResponse { public bool ageVerified; public int age; }

    [System.Serializable]
    private class ProviderVerifyRequest { public string playerId; public string providerName; public string token; }

    [System.Serializable]
    private class ProviderVerifyResponse { public bool ageVerified; public int age; public string providerId; }
    [System.Serializable]
    private class VerifyRequest { public string playerId; public string dob; }

    [System.Serializable]
    private class VerifyResponse { public bool ageVerified; public int age; }
}