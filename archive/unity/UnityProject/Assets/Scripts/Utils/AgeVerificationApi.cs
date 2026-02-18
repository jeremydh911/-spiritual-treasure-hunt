using System;

/// <summary>
/// Small utility for computing age from a DOB (ISO yyyy-MM-dd) and determining
/// whether a DOB meets the minimum-age requirement (default 18).
/// Used by UI and unit tests (client-side logic only).
/// </summary>
public static class AgeVerificationApi
{
    public static int ComputeAge(string dob)
    {
        if (string.IsNullOrEmpty(dob)) return 0;
        if (!DateTime.TryParse(dob, out var dt)) return 0;
        var today = DateTime.UtcNow;
        int age = today.Year - dt.Year;
        if (dt > today.AddYears(-age)) age--;
        return age;
    }

    public static bool IsAdultDob(string dob, int minYears = 18)
    {
        return ComputeAge(dob) >= minYears;
    }
}