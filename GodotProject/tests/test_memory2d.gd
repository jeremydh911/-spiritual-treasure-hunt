extends "res://addons/gut/gut.gd"

# verify that the 2D MemoryMiniGame scene creates buttons and responds to presses

test "MemoryMiniGame2D interactive" do
    var scene = load("res://Scenes/MemoryMiniGame.tscn").instantiate()
    add_child(scene)
    assert_not_null(scene)

    # give the game a status label so we can observe completion text
    var lbl = Label.new()
    scene.statusLabel = lbl
    scene.add_child(lbl)

    var game = scene as MemoryMiniGame
    assert_not_null(game)

    # start small grid for deterministic behavior
    game.InitGrid(1)
    yield(get_tree(), "idle_frame")

    var card0 = scene.get_node_or_null("Card0")
    var card1 = scene.get_node_or_null("Card1")
    assert_not_null(card0)
    assert_not_null(card1)
    assert_true(card0 is Button)
    assert_true(card1 is Button)

    # before pressing, text should be "?"
    assert_eq(card0.text, "?")
    assert_eq(card1.text, "?")

    # press both cards and ensure flips update UI
    card0.pressed()
    card1.pressed()
    yield(get_tree(), "idle_frame")

    assert_true(game.Attempts >= 1)
    assert_true(card0.text != "?")
    assert_true(card1.text != "?")

    # the small grid is just one pair; after both presses game should be complete
    assert_true(game.IsComplete())
    assert_true(lbl.text.find("Done") != -1)
    assert_true(game.Duration >= 0.0)
end
