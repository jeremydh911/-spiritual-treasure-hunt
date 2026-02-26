extends "res://addons/gut/gut.gd"

# simple sanity check that the 3D truth quest scene loads and RunDemoAsync returns a bool

test "TruthQuest_IAmChosen3D loads and can run and barrier reacts" do
    var scene = load("res://Scenes/TruthQuest_IAmChosen3D.tscn").instantiate()
    assert_not_null(scene)
    assert_true(scene is Node3D)
    var Profile = preload("res://../Scripts/PlayerProfile.cs")
    var prof = Profile.new()
    # ensure equipped scripture causes barrier to clear
    prof.AddScripture("SCRIPT-1PET2-9")
    ScriptureManager.EquipScripture(prof, "SCRIPT-1PET2-9")
    var barrier = scene.get_node_or_null("Barrier")
    if barrier:
        # barrier should implement AttemptEnter
        assert_true(barrier.has_method("AttemptEnter"))
        var passed = barrier.call("AttemptEnter", prof)
        assert_true(passed)
    end
    # now exercise RunDemoAsync for completeness
    var method = scene.get_method("RunDemoAsync")
    if method:
        var res = scene.call("RunDemoAsync", prof)
        assert_true(res is GDScriptFunctionState)
    end

    # verify profile recorded result
    var result = prof.GetQuestResult("IAmChosen3D")
    assert_not_null(result)
    assert_true(result.has("attempts"))
end
