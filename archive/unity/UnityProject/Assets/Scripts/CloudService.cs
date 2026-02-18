using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// CloudService prototype: communicates with local backend to verify parental consent and sync profile.
/// - Uses simple REST endpoints in /backend/server.js (included in workspace).
/// - Cloud sync requires parental consent; consentId must be provided on the PlayerProfile (cloudSaveConsentId).
/// </summary>
public static class CloudService
{
    private static string baseUrl = "http://localhost:4000"; // change for production

    public static IEnumerator VerifyParentalConsent(string parentId, string childId, System.Action<bool> callback)
    {
        var payload = JsonUtility.ToJson(new { parentId = parentId, childId = childId });
        using (var www = UnityWebRequest.Post($"{baseUrl}/consent/verify", payload))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("VerifyParentalConsent failed: " + www.error);
                callback?.Invoke(false);
                yield break;
            }
            var resp = www.downloadHandler.text;
            // expect {"consent":true}
            var ok = resp.Contains("true");
            callback?.Invoke(ok);
        }
    }

    public static IEnumerator SyncProfile(string playerId, string jsonProfile, string consentId, System.Action<bool> callback)
    {
        var payload = JsonUtility.ToJson(new { playerId = playerId, consentId = consentId, profile = jsonProfile });
        using (var www = UnityWebRequest.Post($"{baseUrl}/sync/profile", payload))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("SyncProfile failed: " + www.error);
                callback?.Invoke(false);
                yield break;
            }
            callback?.Invoke(true);
        }
    }
}