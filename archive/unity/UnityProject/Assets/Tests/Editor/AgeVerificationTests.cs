using NUnit.Framework;
using UnityEngine;

public class AgeVerificationTests
{
    [Test]
    public void AgeVerificationApi_ComputesAge_Correctly()
    {
        var dob20 = System.DateTime.UtcNow.AddYears(-20).ToString("yyyy-MM-dd");
        var dob16 = System.DateTime.UtcNow.AddYears(-16).ToString("yyyy-MM-dd");

        Assert.IsTrue(AgeVerificationApi.IsAdultDob(dob20));
        Assert.IsFalse(AgeVerificationApi.IsAdultDob(dob16));
        Assert.GreaterOrEqual(AgeVerificationApi.ComputeAge(dob20), 20);
    }

    [Test]
    public void PlayerProfile_SetDob_AllowsIsAdultByAge()
    {
        var go = new GameObject("pp");
        var p = go.AddComponent<PlayerProfile>();
        p.dob = System.DateTime.UtcNow.AddYears(-19).ToString("yyyy-MM-dd");
        Assert.IsTrue(p.IsAdultByAge(18));

        p.dob = System.DateTime.UtcNow.AddYears(-15).ToString("yyyy-MM-dd");
        Assert.IsFalse(p.IsAdultByAge(18));

        Object.DestroyImmediate(go);
    }
}