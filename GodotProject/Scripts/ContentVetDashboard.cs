using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public partial class ContentVetDashboard : Control
{
    private ItemList list;
    private Button vetButton;
    private Button exportButton;
    private List<Truth> truths = new List<Truth>();

    private class Truth { public string id; public string title; public string vetStatus; }

    public override void _Ready()
    {
        list = GetNode<ItemList>("VBox/ItemList");
        vetButton = GetNode<Button>("VBox/HBox/VetButton");
        exportButton = GetNode<Button>("VBox/HBox/ExportButton");
        vetButton.Connect("pressed", new Callable(this, nameof(OnVetPressed)));
        exportButton.Connect("pressed", new Callable(this, nameof(OnExportPressed)));
        list.Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
        vetButton.Disabled = true;
        LoadLocalTruths();
    }

    private void LoadLocalTruths()
    {
        var path = ProjectSettings.GlobalizePath("res://../Content/Truths/truths_index.json");
        try
        {
            var raw = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
            object arr;
            if (raw != null && raw.TryGetValue("truths", out arr) && arr is JsonElement je && je.ValueKind == JsonValueKind.Array)
            {
                truths.Clear();
                foreach (var elem in je.EnumerateArray())
                {
                    var t = JsonSerializer.Deserialize<Truth>(elem.GetRawText());
                    truths.Add(t);
                }
            }
        }
        catch (Exception e)
        {
            GD.PrintErr("Failed to load truths:", e.Message);
        }
        RefreshList();
    }

    private void RefreshList()
    {
        list.Clear();
        foreach (var t in truths)
        {
            var txt = t.title;
            if (t.vetStatus == "vetted") txt += " (vetted)";
            list.AddItem(txt);
        }
    }

    private void OnItemSelected(int idx)
    {
        vetButton.Disabled = false;
    }

    private async void OnVetPressed()
    {
        int idx = list.GetSelectedItems()[0];
        var t = truths[idx];
        // call backend to mark vetted
        try
        {
            using var client = new HttpClient();
            var payload = new { contentId = t.id, vetStatus = "vetted" };
            var body = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(new Uri("http://localhost:4000/admin/content/vet"), body);
            if (resp.IsSuccessStatusCode)
            {
                t.vetStatus = "vetted";
                RefreshList();
            }
            else
            {
                GD.PrintErr("Vet request failed", resp.StatusCode);
            }
        }
        catch (Exception e)
        {
            GD.PrintErr("Vet request error", e.Message);
        }
    }

    private void OnExportPressed()
    {
        var lines = new List<string>();
        lines.Add("id,title,vetStatus");
        foreach (var t in truths)
            lines.Add($"{t.id},{t.title},{t.vetStatus}");
        var outPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "vet_status.csv");
        try { File.WriteAllLines(outPath, lines); GD.Print("Exported to", outPath); } catch (Exception e) { GD.PrintErr("Export failed", e.Message); }
    }
}
