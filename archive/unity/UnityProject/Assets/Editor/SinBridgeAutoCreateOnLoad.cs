#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using UnityEngine;

/// <summary>
/// Auto-creates `Assets/Scenes/SinBridgeDemo.unity` (and prefab) on editor load if missing.
/// </summary>
[InitializeOnLoad]
public static class SinBridgeAutoCreateOnLoad
{
    static SinBridgeAutoCreateOnLoad()
    {
        EditorApplication.delayCall += EnsureDemoSceneExists;
    }

    private static void EnsureDemoSceneExists()
    {
        var scenePath = "Assets/Scenes/SinBridgeDemo.unity";
        if (File.Exists(scenePath)) return;

        Debug.Log("SinBridge demo scene not found — creating: " + scenePath);
        CreateSinBridgeDemoScene.CreateScene();
        AssetDatabase.Refresh();
    }
}
#endif