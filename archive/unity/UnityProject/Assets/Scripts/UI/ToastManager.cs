using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Very small toast system for editor/testing. Add to a Canvas and wire 'panel' and 'toastText'.
/// Use ToastManager.Instance.ShowToast("message", 2.5f);
/// </summary>
public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }
    public GameObject panel;
    public Text toastText;
    public float defaultDuration = 2.5f;

    private Coroutine _current;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    public void ShowToast(string message, float duration = -1f)
    {
        if (duration <= 0) duration = defaultDuration;
        if (_current != null) StopCoroutine(_current);
        _current = StartCoroutine(DoToast(message, duration));
    }

    private IEnumerator DoToast(string message, float duration)
    {
        if (panel == null || toastText == null)
        {
            Debug.Log(message);
            yield break;
        }
        toastText.text = message;
        panel.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
        _current = null;
    }
}