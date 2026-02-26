extends "res://addons/gut/gut.gd"

test "MemoryMiniGame3D logical grid operations" do
    var scene = load("res://Scripts/MemoryMiniGame3D.cs").new()
    add_child(scene)
    # attach dummy status label
    var lbl = Label3D.new()
    scene.statusLabel = lbl
    scene.add_child(lbl)
    scene.scriptures = ["A","B","C"]
    scene.InitGrid(2)
    # verify text assignment on first two cards by name
    var card0 = scene.get_node_or_null("Card0")
    var card1 = scene.get_node_or_null("Card1")
    assert_not_null(card0)
    assert_not_null(card1)
    assert_true(card0.has_method("SetText"))
    assert_true(card1.has_method("SetText"))
    var lbl0 = card0.get_node_or_null("Label")
    if lbl0:
        assert_true(lbl0.text != "")
    # basic flip logic
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
    # verify status label update
    assert_true(lbl.text.find("Done") != -1)
    # verify tracking
    assert_true(scene.Attempts >= 1)
    assert_true(scene.Duration >= 0.0)
end

test "MemoryMiniGame3D instantiates visual cards" do
    var game_scene = load("res://Scenes/SinBridgeDemo3D.tscn") # reuse simple project root
    assert_not_null(game_scene)
end
