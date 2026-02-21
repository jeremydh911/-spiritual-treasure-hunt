using Godot;
using System;

public partial class SinBridge3D : Node3D
{
    // simple 3D prototype: tap/click to alternate a cube
    private MeshInstance3D leftCube;
    private MeshInstance3D rightCube;
    private Label3D counterLabel;
    private int hits;

    public override void _Ready()
    {
        leftCube = GetNode<MeshInstance3D>("BridgeParent/CubeLeft");
        rightCube = GetNode<MeshInstance3D>("BridgeParent/CubeRight");
        counterLabel = GetNode<Label3D>("Counter");
        hits = 0;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb && mb.Pressed)
        {
            float x = GetViewport().GetMousePosition().X;
            float width = GetViewport().Size.X;
            if (x < width / 2)
                RotateCube(leftCube);
            else
                RotateCube(rightCube);

            hits++;
            counterLabel.Text = $"Hits: {hits}";
        }
    }

    private void RotateCube(MeshInstance3D cube)
    {
        var rot = cube.Rotation;
        rot.y += Mathf.Pi / 2;
        cube.Rotation = rot;
    }
}
