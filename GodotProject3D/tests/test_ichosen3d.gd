extends "res://addons/gut/gut.gd"

# simple sanity check that the 3D truth quest scene loads and RunDemoAsync returns a bool

test "TruthQuest_IAmChosen3D loads and can run" do
    var scene = load("res://Scenes/TruthQuest_IAmChosen3D.tscn").instantiate()
    assert_not_null(scene)
    assert_true(scene is Node3D)
    # create a dummy profile
    var Profile = preload("res://../Scripts/PlayerProfile.cs")
    var prof = Profile.new()
    # call RunDemoAsync but don't wait too long
    var method = scene.get_method("RunDemoAsync")
    if method:
        var res = scene.call("RunDemoAsync", prof)
        assert_true(res is GDScriptFunctionState)
    end
end
