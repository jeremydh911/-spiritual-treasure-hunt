using Godot;
using System.Threading.Tasks;

public partial class ProfileSettings3D : Node3D
{
    public PlayerProfile profile;
    private CheckBox cloudToggle;
    private CheckBox telemetryToggle;
    private Button syncButton;
    private Button verifyButton;
    private Button consentButton;
    private Label3D statusLabel;

    public override void _Ready()
    {
        cloudToggle = GetNodeOrNull<CheckBox>("CloudToggle");
        telemetryToggle = GetNodeOrNull<CheckBox>("TelemetryToggle");
        syncButton = GetNodeOrNull<Button>("SyncButton");
        verifyButton = GetNodeOrNull<Button>("VerifyButton");
        consentButton = GetNodeOrNull<Button>("ConsentButton");
        statusLabel = GetNodeOrNull<Label3D>("Status");

        // assume profile is a sibling or child
        profile = GetNodeOrNull<PlayerProfile>("../PlayerProfile") ?? GetNodeOrNull<PlayerProfile>("PlayerProfile");
        if (profile != null)
        {
            cloudToggle?.SetPressed(profile.cloudSaveEnabled);
            telemetryToggle?.SetPressed(profile.telemetryEnabled);
        }

        cloudToggle?.Connect("toggled", new Callable(this, nameof(OnCloudToggled)));
        telemetryToggle?.Connect("toggled", new Callable(this, nameof(OnTelemetryToggled)));
        syncButton?.Connect("pressed", new Callable(this, nameof(OnSyncPressed)));
        verifyButton?.Connect("pressed", new Callable(this, nameof(OnVerifyPressed)));
        consentButton?.Connect("pressed", new Callable(this, nameof(OnConsentPressed)));
    }

    private void OnCloudToggled(bool pressed)
    {
        if (profile != null)
        {
            profile.cloudSaveEnabled = pressed;
            SaveManager.SaveLocalProfile(profile);
            UpdateStatus("Cloud save " + (pressed?"enabled":"disabled"));
        }
    }

    private void OnTelemetryToggled(bool pressed)
    {
        if (profile != null)
        {
            profile.telemetryEnabled = pressed;
            SaveManager.SaveLocalProfile(profile);
            UpdateStatus("Telemetry " + (pressed?"enabled":"disabled"));
        }
    }

    private async void OnSyncPressed()
    {
        if (profile != null)
        {
            UpdateStatus("Syncing...");
            bool ok = await SaveManager.SyncProfileAsync(profile);
            UpdateStatus(ok ? "Sync succeeded" : "Sync failed");
        }
    }

    private void OnVerifyPressed()
    {
        if (profile != null)
        {
            int age = profile.GetAge();
            bool ok = age >= 18;
            profile.ageVerified = ok;
            profile.ageVerifiedAt = System.DateTime.UtcNow.ToString("o");
            UpdateStatus(ok ? "Age verified" : "Underage");
        }
    }

    private void OnConsentPressed()
    {
        if (profile != null)
        {
            // simulate granting consent by generating ID
            profile.cloudSaveConsentId = $"consent_{System.DateTime.UtcNow.Ticks}";
            UpdateStatus("Parental consent granted");
        }
    }

    private void UpdateStatus(string text)
    {
        if (statusLabel != null)
            statusLabel.Text = text;
    }
}
