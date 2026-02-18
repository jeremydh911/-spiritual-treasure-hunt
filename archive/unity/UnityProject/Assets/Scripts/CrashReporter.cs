using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class CrashReport
{
    public string timestamp;
    public string scene;
    public string message;
    public string stack;
    public string appVersion;
}

/// <summary>
/// CrashReporter writes anonymized crash reports locally and exposes a gated uploader.
/// IMPORTANT: do NOT include PII (emails, usernames, device IDs) in reports.
/// Default behavior: queue locally; upload only when allowed (TelemetryManager.CrashReportingEnabled).
/// </summary>
public static class CrashReporter
{
    private static string CrashFolder => Path.Combine(Application.persistentDataPath, "CrashReports");

    public static void QueueAnonymizedReport(string message, string stack)
    {
        try
        {
            if (!Directory.Exists(CrashFolder)) Directory.CreateDirectory(CrashFolder);

            var report = new CrashReport
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                message = Sanitize(message),
                stack = stack ?? string.Empty,
                appVersion = Application.version
            };

            var filename = Path.Combine(CrashFolder, $"crash_{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}.json");
            var json = JsonUtility.ToJson(report);
            File.WriteAllText(filename, json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("CrashReporter failed to queue report: " + ex.Message);
        }
    }

    // Basic sanitization: redact email patterns and long numeric sequences. Extend as needed.
    private static string Sanitize(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        var step1 = Regex.Replace(s, @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}", "[redacted]");
        var step2 = Regex.Replace(step1, @"\d{6,}", "[redacted]");
        return step2;
    }

    // Upload is intentionally a stub — implement server upload that accepts ONLY anonymized reports.
    public static void SendPendingReportsIfAllowed()
    {
        if (!TelemetryManager.CrashReportingEnabled) return;
        // For now keep local; production: securely upload reports to server endpoint with TLS,
        // ensure payload contains no PII and retain per the DataRetention policy.
    }
}