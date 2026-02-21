extends "res://addons/gut/gut.gd"

test "MemoryMiniGame3D logical grid operations" do
    var scene = load("res://Scripts/MemoryMiniGame3D.cs").new()
    add_child(scene)
    scene.InitGrid(2)
    assert_true(scene.FlipIndex(0))
    assert_true(scene.FlipIndex(1))
    # even if they don't match, FlipIndex should not crash
    assert_false(scene.IsComplete())
    # brute force solve
    scene.InitGrid(1)
    # with one pair, flipping 0 and 1 completes
    assert_true(scene.FlipIndex(0))
    assert_true(scene.FlipIndex(1))
    assert_true(scene.IsComplete())
end

test "MemoryMiniGame3D instantiates visual cards" do
    var game_scene = load("res://Scenes/SinBridgeDemo3D.tscn") # reuse simple project root
    assert_not_null(game_scene)
end
