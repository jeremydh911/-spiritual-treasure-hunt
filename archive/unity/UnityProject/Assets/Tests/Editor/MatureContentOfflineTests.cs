using NUnit.Framework;
using UnityEngine;

public class MatureContentOfflineTests
{
    [Test]
    public void SaveLocalGuideToDisk_WritesFile_WhenResourceExists()
    {
        var go = new GameObject("mcu");
        var ui = go.AddComponent<MatureContentUI>();

        // We'll call the manager directly to ensure the resource exists
        var txt = MatureContentManager.LoadMatureText("ParentGuide");
        Assert.IsNotNull(txt, "ParentGuide resource should exist in Resources/MatureContent/");

        // Use the UI helper to save the local copy
        var saved = ui.SaveLocalGuideToDisk("ParentGuide");
        Assert.IsTrue(saved, "SaveLocalGuideToDisk should return true when resource exists");

        // Verify file saved in persistentDataPath
        var path = System.IO.Path.Combine(Application.persistentDataPath, "ParentGuide_guide.md");
        Assert.IsTrue(System.IO.File.Exists(path), "Guide file should be saved to persistentDataPath");

        var content = System.IO.File.ReadAllText(path);
        Assert.IsNotEmpty(content);

        Object.DestroyImmediate(go);
    }
}