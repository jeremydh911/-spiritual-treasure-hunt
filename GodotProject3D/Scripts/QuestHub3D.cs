using Godot;
using System;
using System.Collections.Generic;

public partial class QuestHub3D : Node3D
{
    [Export] public PackedScene[] questScenes;
    private Node3D currentQuest;

    public override void _Ready()
    {
        // automatically load all quest scenes from a folder
        if (questScenes == null || questScenes.Length == 0)
        {
            var dir = new Directory();
            if (dir.Open("res://Scenes/Quests3D") == Error.Ok)
            {
                var files = dir.GetFiles();
                var list = new List<PackedScene>();
                foreach (var f in files)
                {
                    if (f.EndsWith(".tscn"))
                    {
                        var scene = GD.Load<PackedScene>("res://Scenes/Quests3D/" + f);
                        if (scene != null) list.Add(scene);
                    }
                }
                questScenes = list.ToArray();
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb && mb.Pressed)
        {
            var ray = GetWorld3D().DirectSpaceState.IntersectRay(
                GetViewport().GetCamera3D().ProjectRayOrigin(GetViewport().GetMousePosition()),
                GetViewport().GetCamera3D().ProjectRayNormal(GetViewport().GetMousePosition()) * 1000
            );
            if (ray.Count > 0 && ray.Contains("collider"))
            {
                var hit = ray["collider"] as Node3D;
                if (hit != null && hit.Name.StartsWith("Selector"))
                {
                    int idx = -1;
                    if (int.TryParse(hit.Name.Replace("Selector", ""), out idx))
                    {
                        LaunchQuest(idx);
                    }
                }
            }
        }
    }

    private void LaunchQuest(int index)
    {
        if (index < 0 || index >= questScenes.Length) return;
        // remove previous
        if (currentQuest != null)
        {
            currentQuest.QueueFree();
            currentQuest = null;
        }
        var scene = questScenes[index].Instantiate();
        if (scene is Node3D n3)
        {
            AddChild(n3);
            currentQuest = n3;
        }
    }
}
