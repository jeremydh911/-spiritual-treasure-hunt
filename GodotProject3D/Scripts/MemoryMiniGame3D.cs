using Godot;
using System;
using System.Collections.Generic;

public partial class MemoryMiniGame3D : Node3D
{
    [Export] private PackedScene cardScene;
    private List<Node3D> cards = new();
    private Node3D flipped;

    public override void _Ready()
    {
        // build 4 cards in a square
        for (int i = 0; i < 4; i++)
        {
            var card = (Node3D)cardScene.Instantiate();
            card.Position = new Vector3((i%2)*2 - 1, 0, (i/2)*2 - 1);
            AddChild(card);
            cards.Add(card);
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
                FlipCard(hit);
            }
        }
    }

    private void FlipCard(Node3D card)
    {
        if (flipped == null)
        {
            flipped = card;
            card.RotateX(Mathf.Pi);
        }
        else if (flipped == card)
        {
            // ignore
        }
        else
        {
            card.RotateX(Mathf.Pi);
            flipped = null;
        }
    }
}
