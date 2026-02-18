using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sends a pastor/contact request to the backend for parental/church follow-up.
/// </summary>
public class PastorContactUI : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public InputField messageField;
    public Button sendButton;

    private void OnEnable()
    {
        if (sendButton != null) sendButton.onClick.AddListener(OnSend);
    }

    private void OnDisable()
    {
        if (sendButton != null) sendButton.onClick.RemoveListener(OnSend);
    }

    private void OnSend()
    {
        if (playerProfile == null) { ToastManager.Instance?.ShowToast("Assign player profile first"); return; }
        var msg = messageField != null ? messageField.text : "";
        if (string.IsNullOrEmpty(msg)) { ToastManager.Instance?.ShowToast("Please enter a message"); return; }
        StartCoroutine(PostContact(playerProfile.playerId, msg));
    }

    private IEnumerator PostContact(string playerId, string message)
    {
        var payload = JsonUtility.ToJson(new { playerId = playerId, message = message });
        using (var www = UnityEngine.Networking.UnityWebRequest.Post("http://localhost:4000/contact/pastor", payload))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(payload));
            www.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                ToastManager.Instance?.ShowToast("Failed to send request");
            }
            else
            {
                ToastManager.Instance?.ShowToast("Request sent. Your church leader will follow up.");
                messageField.text = "";
            }
        }
    }
}