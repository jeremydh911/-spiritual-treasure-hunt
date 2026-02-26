extends "res://addons/gut/gut.gd"

test "SinBridge3D scene loads and has status label" do
    var scene = load("res://Scenes/SinBridgeDemo3D.tscn")
    var inst = scene.instantiate()
    assert_not_null(inst)
    assert_true(inst is Node3D)
    var status = inst.get_node_or_null("Status")
    assert_not_null(status)
    assert_true(status is Label3D)
    # call play to make sure the method exists
    inst.call("Play")
end
