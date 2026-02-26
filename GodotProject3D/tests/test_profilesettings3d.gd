extends "res://addons/gut/gut.gd"

# full exercise of profile settings including accessibility options

test "ProfileSettings3D loads and toggles" do
    var scene = load("res://Scenes/ProfileSettings3D.tscn").instantiate()
    assert_not_null(scene)

    var cloud = scene.get_node_or_null("CloudToggle")
    var tel = scene.get_node_or_null("TelemetryToggle")
    var sync = scene.get_node_or_null("SyncButton")
    var verify = scene.get_node_or_null("VerifyButton")
    var consent = scene.get_node_or_null("ConsentButton")
    var textSize = scene.get_node_or_null("TextSizeOption")
    var narr = scene.get_node_or_null("NarrationToggle")
    var contrast = scene.get_node_or_null("ContrastToggle")

    # nodes present
    assert_not_null(cloud)
    assert_not_null(tel)
    assert_not_null(sync)
    assert_not_null(verify)
    assert_not_null(consent)
    assert_not_null(textSize)
    assert_not_null(narr)
    assert_not_null(contrast)

    # basic toggles
    cloud.toggled(true)
    tel.toggled(false)
    assert_true(cloud.pressed)
    assert_false(tel.pressed)

    # verify and consent modify profile
    var prof = PlayerProfile.new()
    prof.dob = "2000-01-01"
    scene.profile = prof
    verify.pressed()
    assert_true(prof.ageVerified)
    consent.pressed()
    assert_true(prof.cloudSaveConsentId != null)

    # accessibility interactions
    assert_eq(textSize.get_item_count(), 3)
    # select large text
    textSize.select(2)
    assert_eq(prof.textSize, PlayerProfile.TextSize.Large)
    narr.toggled(true)
    assert_true(prof.narrationEnabled)
    contrast.toggled(true)
    assert_true(prof.highContrastMode)
end
