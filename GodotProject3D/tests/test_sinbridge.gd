extends "res://addons/gut/gut.gd"

test "SinBridge3D scene loads" do
    var scene = load("res://Scenes/SinBridgeDemo3D.tscn")
    var inst = scene.instantiate()
    assert_not_null(inst)
    assert_true(inst is Node3D)
end
