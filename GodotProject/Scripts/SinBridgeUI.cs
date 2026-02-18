using Godot;
using System;
using System.Threading.Tasks;

public partial class SinBridgeUI : Node2D
{
    Label statusLabel;

    public override void _Ready()
    {
        statusLabel = GetNodeOrNull<Label>("Status") ?? new Label();
        // start automatically for demo
        _ = PlaySequence();
    }

    public async Task PlaySequence()
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
}
