using Godot;
using System;

public partial class MatureGuideUI : Control
{
    private RichTextLabel textLabel;
    private Button saveButton;
    private Label statusLabel;
    [Export] public string guideKey = "ParentGuide";

    public override void _Ready()
    {
        textLabel = GetNodeOrNull<RichTextLabel>("VBoxContainer/RichTextLabel");
        saveButton = GetNodeOrNull<Button>("VBoxContainer/SaveButton");
        statusLabel = GetNodeOrNull<Label>("VBoxContainer/StatusLabel");

        if (textLabel != null)
        {
            var txt = MatureContentManager.LoadMatureText(guideKey);
            textLabel.BbcodeText = txt ?? "Guide not found";
        }

        saveButton?.Connect("pressed", new Callable(this, nameof(OnSavePressed)));
    }

    private void OnSavePressed()
    {
        bool ok = MatureContentManager.SaveLocalGuideToDisk(guideKey);
        if (statusLabel != null)
            statusLabel.Text = ok ? "Saved to device." : "Failed to save.";
    }
}
