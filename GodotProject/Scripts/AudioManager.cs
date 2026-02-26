using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class AudioManager : Node
{
    public Dictionary<string, string[]> manifest = new Dictionary<string, string[]>();

    public override void _Ready()
    {
        LoadManifest();
    }

    public void LoadManifest()
    {
        try
        {
            var path = ProjectSettings.GlobalizePath("res://Resources/Audio/manifest.json");
            var json = File.ReadAllText(path);
            var parsed = JSON.Parse(json);
            if (parsed.Result is Godot.Collections.Dictionary dict)
            {
                foreach (var key in dict.Keys)
                {
                    if (dict[key] is Godot.Collections.Array arr)
                    {
                        var list = new List<string>();
                        foreach (var item in arr) list.Add(item.ToString());
                        manifest[key.ToString()] = list.ToArray();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("AudioManager LoadManifest failed: " + ex.Message);
        }
    }

    public string[] GetEffects(string category)
    {
        return manifest.ContainsKey(category) ? manifest[category] : new string[0];
    }
}
