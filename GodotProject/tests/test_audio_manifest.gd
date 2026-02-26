extends "res://addons/gut/gut.gd"

test "Audio manifest loads correctly" do
    var mgr = AudioManager.new()
    add_child(mgr)
    mgr.LoadManifest()
    var narr = mgr.GetEffects("narration")
    assert_true(narr.size() == 2)
    var aff = mgr.GetEffects("affirmation")
    assert_true(aff.size() == 2)
end
