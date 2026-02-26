extends "res://addons/gut/gut.gd"

test "ProfileSettings3D loads and toggles" do
    var scene = load("res://Scenes/ProfileSettings3D.tscn").instantiate()
    assert_not_null(scene)
    var cloud = scene.get_node_or_null("CloudToggle")
    var tel = scene.get_node_or_null("TelemetryToggle")
    var sync = scene.get_node_or_null("SyncButton")
    assert_not_null(cloud)
    assert_not_null(tel)
    assert_not_null(sync)
    # simulate toggle
    cloud.toggled(true)
    tel.toggled(false)
    assert_true(cloud.pressed)
    assert_false(tel.pressed)
end
