#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEngine;

/// <summary>
/// Auto-creates `Assets/Scenes/TruthBarrierDemo.unity` on editor load if missing.
/// Calls the existing `CreateTruthBarrierDemoScene.CreateScene()` helper so the
/// generated scene matches the same structure as the menu action.
/// </summary>
[InitializeOnLoad]
public static class TruthBarrierAutoCreateOnLoad
{
    static TruthBarrierAutoCreateOnLoad()
    {
        // Delay the check until editor initialization completes
        EditorApplication.delayCall += EnsureDemoSceneExists;
    }

    private static void EnsureDemoSceneExists()
    {
        var path = "Assets/Scenes/TruthBarrierDemo.unity";
        if (File.Exists(path)) return;

        Debug.Log("TruthBarrier demo scene not found — creating: " + path);
        // Reuse the menu helper to create and save the scene
        CreateTruthBarrierDemoScene.CreateScene();

        // Force a refresh so the new scene is visible in the Project window
        AssetDatabase.Refresh();
    }
}
#endif