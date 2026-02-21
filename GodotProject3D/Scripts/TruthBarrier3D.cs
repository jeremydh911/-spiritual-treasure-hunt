using Godot;
using System;

public partial class TruthBarrier3D : Node3D
{
    [Export] public bool barrierActive = true;

    public bool AttemptEnter(PlayerProfile profile)
    {
        if (!barrierActive) return true;

        var eq = profile?.GetEquippedScripture();
        var eff = ScriptureManager.UseScripture(eq);
        if (eff == ScriptureEffect.DispelLies)
        {
            barrierActive = false;
            GD.Print($"TruthBarrier3D: dispelled by {eq}");
            return true;
        }

        return false;
    }
}
