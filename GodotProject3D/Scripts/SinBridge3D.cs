using Godot;
using System;

public partial class SinBridge3D : Node3D
{
    // simple 3D prototype: tap/click to alternate a cube
    private MeshInstance3D leftCube;
    private MeshInstance3D rightCube;
    private Label3D counterLabel;
    private Label3D statusLabel;
    private int hits;

    public override void _Ready()
    {
        leftCube = GetNode<MeshInstance3D>("BridgeParent/CubeLeft");
        rightCube = GetNode<MeshInstance3D>("BridgeParent/CubeRight");
        counterLabel = GetNode<Label3D>("Counter");
        statusLabel = GetNodeOrNull<Label3D>("Status");
        hits = 0;

        // automatically play demonstration sequence
        _ = PlaySequence();
    }

    public async System.Threading.Tasks.Task PlaySequence()
    {
        if (statusLabel != null) statusLabel.Text = "Starting...";
        await ToSignal(GetTree().CreateTimer(0.4f), "timeout");
        if (statusLabel != null) statusLabel.Text = "Bridge appears";
        await ToSignal(GetTree().CreateTimer(0.9f), "timeout");
        if (statusLabel != null) statusLabel.Text = "Barrier covered";
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        if (statusLabel != null) statusLabel.Text = "Walking to Kingdom";
        await ToSignal(GetTree().CreateTimer(1.2f), "timeout");
        if (statusLabel != null) statusLabel.Text = "Arrived — glowing";
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        if (statusLabel != null) statusLabel.Text = "Done";
    }

    // Public trigger for demo or buttons
    public void Play()
    {
        _ = PlaySequence();
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
