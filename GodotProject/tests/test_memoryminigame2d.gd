extends "res://addons/gut/gut.gd"

test "MemoryMiniGame 2D UI buttons" do
    var scene = load("res://Scenes/MemoryMiniGame.tscn").instantiate()
    add_child(scene)
    assert_not_null(scene)
    scene.InitGrid(2)
    # ensure buttons created
    var btn0 = scene.get_node_or_null("Card0")
    var btn1 = scene.get_node_or_null("Card1")
    assert_not_null(btn0)
    assert_not_null(btn1)
    # simulate pressing
    btn0.pressed()
    # after press, text should change
    assert_true(btn0.Text != "?")
end
