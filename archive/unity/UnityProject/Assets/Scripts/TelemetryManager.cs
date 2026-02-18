using UnityEngine;

/// <summary>
/// TelemetryManager centralizes telemetry/analytics controls.
/// Defaults: Analytics OFF, CrashReporting ON (anonymized) — both are opt‑outable.
/// All telemetry calls should check these flags before sending data.
/// </summary>
public static class TelemetryManager
{
    private const string PREF_ANALYTICS = "Telemetry_AnalyticsEnabled";
    private const string PREF_CRASH = "Telemetry_CrashReportingEnabled";

    // Analytics default: OFF (0)
    public static bool AnalyticsEnabled => PlayerPrefs.GetInt(PREF_ANALYTICS, 0) == 1;

    // Crash reporting default: ON (1) but anonymized and opt‑outable
    public static bool CrashReportingEnabled => PlayerPrefs.GetInt(PREF_CRASH, 1) == 1;

    public static void SetAnalyticsEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(PREF_ANALYTICS, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void SetCrashReportingEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(PREF_CRASH, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // establish defaults if not present
        if (!PlayerPrefs.HasKey(PREF_ANALYTICS)) PlayerPrefs.SetInt(PREF_ANALYTICS, 0);
        if (!PlayerPrefs.HasKey(PREF_CRASH)) PlayerPrefs.SetInt(PREF_CRASH, 1);
    }

    // No-op safe wrappers — call these from game code instead of directly integrating analytics
    public static void LogEvent(string eventName)
    {
        if (!AnalyticsEnabled) return; // analytics are off by default
        // Integrate analytics provider here if/when enabled
    }

    public static void LogException(string message, string stack)
    {
        if (!CrashReportingEnabled) return;
        // Queue an anonymized crash report (CrashReporter handles sanitization)
        CrashReporter.QueueAnonymizedReport(message, stack);
    }

    // Helper for UI and tests
    public static bool IsTelemetryDefaultOff() => !AnalyticsEnabled;
}