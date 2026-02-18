using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editor helper: GameObject → UI → "Sin → Jesus Bridge"
/// Creates a Canvas (if one doesn't exist) and a ready‑to‑use SinBridgeUI GameObject.
/// </summary>
public static class CreateSinBridgeMenu
{
    [MenuItem("GameObject/UI/Sin → Jesus Bridge", false, 10)]
    public static void CreateSinBridge()
    {
        // Ensure a Canvas exists or create one
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            // add EventSystem if missing
            if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var es = new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
            }
        }

        // Create container GameObject
        var parent = canvas.transform;
        var go = new GameObject("SinBridgeUI", typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        // Attach SinBridgeUI component and build the default UI
        var sbComp = go.AddComponent<SinBridgeUI>();
        sbComp.Build();

        Selection.activeGameObject = go;
    }
}
