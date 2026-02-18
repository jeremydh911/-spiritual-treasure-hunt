#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editor helper: creates and saves a simple `TruthBarrier` demo scene at
/// `Assets/Scenes/TruthBarrierDemo.unity`. Run from: Tools/Spiritual Demo/Create TruthBarrier Demo Scene
/// </summary>
public static class CreateTruthBarrierDemoScene
{
    [MenuItem("Tools/Spiritual Demo/Create TruthBarrier Demo Scene")]
    public static void CreateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Root controller
        var root = new GameObject("TruthBarrierDemoRoot");
        var demo = root.AddComponent<TruthBarrierDemo>();

        // Player/profile
        var playerGO = new GameObject("DemoPlayer");
        playerGO.transform.SetParent(root.transform);
        var profile = playerGO.AddComponent<PlayerProfile>();
        profile.playerId = "demo_player_truthbarrier";
        profile.ownedScriptures = new string[] { "SCRIPT-ROM8-1" };
        ScriptureManager.EquipScripture(profile, "SCRIPT-ROM8-1");
        demo.demoProfile = profile;

        // Barrier
        var barrierGO = new GameObject("DemoBarrier");
        barrierGO.transform.SetParent(root.transform);
        var barrier = barrierGO.AddComponent<TruthBarrier>();
        barrier.barrierActive = true;
        demo.barrier = barrier;

        // Simple UI (Canvas + Text)
        var canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(root.transform);
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var textGO = new GameObject("StatusText");
        textGO.transform.SetParent(canvasGO.transform);
        var text = textGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.text = "Ready — run the demo (auto executes on Start)";
        var rt = text.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.1f, 0.45f);
        rt.anchorMax = new Vector2(0.9f, 0.55f);
        rt.sizeDelta = new Vector2(0, 30);
        demo.statusText = text;

        // Save the scene file
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        var path = "Assets/Scenes/TruthBarrierDemo.unity";
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, path);

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = root;
        Debug.Log("TruthBarrier demo scene created: " + path);
    }
}
#endif