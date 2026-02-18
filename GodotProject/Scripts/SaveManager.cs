using Godot;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public static class SaveManager
{
    private static string LocalDir => ProjectSettings.GlobalizePath("user://");

    public static void SaveLocalProfile(PlayerProfile profile)
    {
        try
        {
            var p = Path.Combine(LocalDir, $"profile_{profile.playerId}.json");
            File.WriteAllText(p, JsonSerializer.Serialize(profile));
        }
        catch (Exception e)
        {
            GD.PrintErr("SaveLocalProfile failed:", e.Message);
        }
    }

    // Demo sync: calls backend /sync/profile with consentId when available
    public static async Task<bool> SyncProfileAsync(PlayerProfile profile, string serverUrl = "http://localhost:4000")
    {
        if (!profile.CanUseCloudSave()) return false;
        try
        {
            using var client = new HttpClient();
            var payload = new { playerId = profile.playerId, consentId = profile.cloudSaveConsentId, profile };
            var body = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(new Uri(new Uri(serverUrl), "/sync/profile"), body);
            return resp.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            GD.PrintErr("SyncProfileAsync failed:", e.Message);
            return false;
        }
    }
}
