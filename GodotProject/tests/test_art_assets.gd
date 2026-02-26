extends "res://addons/gut/gut.gd"

test "Art placeholders exist" do
    var files = [
        "res://Resources/Art/kingdom_ui_placeholder.png",
        "res://Resources/Art/card_placeholder.png",
        "res://Resources/Art/stone_placeholder.png"
    ]
    for f in files:
        var r = ResourceLoader.load(f)
        assert_not_null(r, "failed to load " + f)
    end
end
