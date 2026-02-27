extends "res://addons/gut/gut.gd"

# UI test for content vetting dashboard

test "ContentVetDashboard marks item vetted" do
    var scene = load("res://Scenes/ContentVetDashboard.tscn").instantiate()
    assert_not_null(scene)

    # add to tree so _Ready runs
    get_tree().get_root().add_child(scene)
    yield(get_tree(), "idle_frame")

    var list = scene.get_node("VBox/ItemList")
    var vet = scene.get_node("VBox/HBox/VetButton")

    # there should be at least one entry
    assert_true(list.get_item_count() > 0)

    list.select(0)
    assert_false(vet.disabled)

    # simulate press; because backend may not be running in this test env
    vet.pressed()
    # wait a bit for potential async to complete
    yield(get_tree().create_timer(0.2), "timeout")

    # after pressing, the text should include "(vetted)" regardless of backend
    var txt = list.get_item_text(0)
    assert_true(txt.ends_with("(vetted)"))
end
