extends "res://addons/gut/gut.gd"

# ensure new verify/consent buttons exist and update profile fields

test "ProfileSettings3D age/consent buttons" do
    var scene = load("res://Scenes/ProfileSettings3D.tscn").instantiate()
    add_child(scene)
    var verify = scene.get_node_or_null("VerifyButton")
    var consent = scene.get_node_or_null("ConsentButton")
    assert_not_null(verify)
    assert_not_null(consent)
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
end
