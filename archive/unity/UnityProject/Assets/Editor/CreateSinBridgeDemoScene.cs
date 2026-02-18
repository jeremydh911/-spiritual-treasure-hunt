#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Editor helper: creates a simple `SinBridge` demo scene and saves a prefab for the UI.
/// Menu: Tools → Spiritual Demo → Create SinBridge Demo Scene
/// </summary>
public static class CreateSinBridgeDemoScene
{
    [MenuItem("Tools/Spiritual Demo/Create SinBridge Demo Scene")]
    public static void CreateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Use the existing menu helper to create the SinBridge UI under a Canvas
        CreateSinBridgeMenu.CreateSinBridge();

        var sinBridgeGO = GameObject.Find("SinBridgeUI");
        if (sinBridgeGO == null)
        {
            Debug.LogError("Failed to create SinBridgeUI via CreateSinBridgeMenu.CreateSinBridge()");
            return;
        }

        // Ensure folders exist
        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Prefabs");

        // Save as prefab (editor-only helper)
        var prefabPath = "Assets/Prefabs/SinBridgeUI.prefab";
        UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(sinBridgeGO, prefabPath, UnityEditor.InteractionMode.AutomatedAction);

        // Save the scene to Assets/Scenes
        var scenePath = "Assets/Scenes/SinBridgeDemo.unity";
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, scenePath);

        Selection.activeGameObject = sinBridgeGO;
        Debug.Log($"SinBridge demo scene and prefab created: {scenePath} / {prefabPath}");
    }
}
#endif