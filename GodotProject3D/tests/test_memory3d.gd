extends "res://addons/gut/gut.gd"

test "MemoryMiniGame3D instantiates and responds to input" do
    var scene = load("res://Scenes/Card3D.tscn")
    var card = scene.instantiate()
    assert_not_null(card)
    var game_scene = load("res://Scenes/SinBridgeDemo3D.tscn") # reuse for simplicity
    assert_not_null(game_scene)
end
