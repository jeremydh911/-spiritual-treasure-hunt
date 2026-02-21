extends "res://addons/gut/gut.gd"

# Ensure hub loads and selectors exist

test "QuestHub3D scene loads with selectors" do
    var hub = load("res://Scenes/QuestHub3D.tscn").instantiate()
    assert_not_null(hub)
    assert_true(hub is Node3D)
    assert_not_null(hub.get_node_or_null("Selector0"))
    assert_not_null(hub.get_node_or_null("Selector1"))
end

test "Launching quest creates child node" do
    var hub = load("res://Scenes/QuestHub3D.tscn").instantiate()
    assert_not_null(hub)
    # populate questScenes with at least one simple scene
    hub.questScenes = [load("res://Scenes/TruthQuest_IAmChosen3D.tscn")]
    hub.LaunchQuest(0)
    var child = hub.get_child( hub.GetChildCount() - 1 )
    assert_not_null(child)
    assert_true(child is Node3D)
end
