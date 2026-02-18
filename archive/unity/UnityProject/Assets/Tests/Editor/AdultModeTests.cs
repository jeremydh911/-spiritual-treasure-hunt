using NUnit.Framework;
using UnityEngine;

public class AdultModeTests
{
    [Test]
    public void PlayerProfile_AgeCalculation_Works()
    {
        var go = new GameObject("pp");
        var p = go.AddComponent<PlayerProfile>();
        p.dob = System.DateTime.UtcNow.AddYears(-20).ToString("yyyy-MM-dd");
        Assert.IsTrue(p.IsAdultByAge(18));
        Assert.GreaterOrEqual(p.GetAge(), 20);

        p.dob = System.DateTime.UtcNow.AddYears(-16).ToString("yyyy-MM-dd");
        Assert.IsFalse(p.IsAdultByAge(18));

        Object.DestroyImmediate(go);
    }

    [Test]
    public void MatureContentManager_UsesAgeVerification_And_ChurchOverride()
    {
        var go = new GameObject("pp2");
        var p = go.AddComponent<PlayerProfile>();
        // Production: require explicit age verification
        p.ageVerified = true;
        p.adultModeDisabledByChurch = false;

        Assert.IsTrue(MatureContentManager.IsMatureContentAllowed(p));

        // church override blocks access
        p.adultModeDisabledByChurch = true;
        Assert.IsFalse(MatureContentManager.IsMatureContentAllowed(p));

        // underage / unverified is blocked even if adultModeEnabled true
        p.adultModeDisabledByChurch = false;
        p.ageVerified = false;
        p.adultModeEnabled = true; // legacy flag shouldn't bypass age verification
        Assert.IsFalse(MatureContentManager.IsMatureContentAllowed(p));

        Object.DestroyImmediate(go);
    }
}