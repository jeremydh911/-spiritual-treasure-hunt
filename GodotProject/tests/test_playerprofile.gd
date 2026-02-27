extends "res://addons/gut/gut.gd"

# Verify cloud save consent logic on PlayerProfile

test "PlayerProfile CanUseCloudSave respects age and consent" do
    var prof = PlayerProfile.new()
    # child without consent cannot cloud save even if enabled
    prof.dob = "2015-01-01"
    prof.cloudSaveEnabled = true
    prof.cloudSaveConsentId = ""
    assert_false(prof.CanUseCloudSave())

    # add consent
    prof.cloudSaveConsentId = "c1"
    assert_true(prof.CanUseCloudSave())

    # adult should be allowed regardless of consent id (but must have cloudSaveEnabled)
    prof.dob = "2000-01-01"
    prof.cloudSaveConsentId = ""  # empty
    assert_true(prof.CanUseCloudSave())

    # disable cloud saves and nothing works
    prof.cloudSaveEnabled = false
    assert_false(prof.CanUseCloudSave())
end
