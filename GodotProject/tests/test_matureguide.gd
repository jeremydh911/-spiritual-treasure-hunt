extends "res://addons/gut/gut.gd"

# verify that MatureContentManager can save a guide and that UI button triggers saving

test "SaveLocalGuideToDisk writes file and UI button works" do
    var userPath = ProjectSettings.globalize_path("user://")
    var outPath = userPath.plus_file("ParentGuide_guide.md")
    # ensure no leftover file
    var f = File.new()
    if f.file_exists(outPath): f.remove(outPath)

    # direct manager call
    assert_true(MatureContentManager.SaveLocalGuideToDisk("ParentGuide"))
    assert_true(f.file_exists(outPath))
    f.remove(outPath)

    # test UI press
    var scene = load("res://Scenes/MatureGuide.tscn").instantiate()
    add_child(scene)
    var btn = scene.get_node("VBoxContainer/SaveButton")
    assert_not_null(btn)
    btn.pressed()
    yield(get_tree(), "idle_frame")
    assert_true(f.file_exists(outPath))
    # cleanup
    f.remove(outPath)
end
