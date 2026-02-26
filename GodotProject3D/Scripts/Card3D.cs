using Godot;

public partial class Card3D : RigidBody3D
{
    private Label3D label;

    [Export] public string textValue = "";

    public override void _Ready()
    {
        label = GetNodeOrNull<Label3D>("Label");
        if (label != null)
            label.Text = textValue;
    }

    public void SetText(string t)
    {
        textValue = t;
        if (label != null)
            label.Text = t;
    }
}
