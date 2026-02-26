extends "res://addons/gut/gut.gd"

# verify the presence of the new controls and that they mutate the underlying PlayerProfile

test "ProfileSettings3D controls and accessibility" do
    var scene = load("res://Scenes/ProfileSettings3D.tscn").instantiate()
    add_child(scene)
    var verify = scene.get_node_or_null("VerifyButton")
    var consent = scene.get_node_or_null("ConsentButton")
    var textSize = scene.get_node_or_null("TextSizeOption")
    var narr = scene.get_node_or_null("NarrationToggle")
    var contrast = scene.get_node_or_null("ContrastToggle")
    assert_not_null(verify)
    assert_not_null(consent)
    assert_not_null(textSize)
    assert_not_null(narr)
    assert_not_null(contrast)

    # assign dummy profile and set dob
    var prof = PlayerProfile.new()
    prof.dob = "2000-01-01"
    scene.profile = prof
    # press verify
    verify.pressed()
    assert_true(prof.ageVerified)
    # press consent
    consent.pressed()
    assert_true(prof.cloudSaveConsentId != null)

    # accessibility interactions
    assert_eq(textSize.get_item_count(), 3)
    textSize.select(1)
    assert_eq(prof.textSize, PlayerProfile.TextSize.Normal)
    narr.toggled(true)
    assert_true(prof.narrationEnabled)
    contrast.toggled(true)
    assert_true(prof.highContrastMode)
end
