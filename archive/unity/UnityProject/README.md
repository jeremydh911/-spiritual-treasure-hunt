UnityProject placeholder

**Required Unity Editor: 2022.3 LTS** — see `ProjectSettings/ProjectVersion.txt` for the pinned editor version.

This folder contains initial C# ScriptableObject and MonoBehaviour stubs to be imported into a Unity project.

Files of interest
- Assets/Scripts/QuestSpec.cs — ScriptableObject model for quests
- Assets/Scripts/PlayerProfile.cs — runtime player profile model
- Assets/Scripts/CosmeticItem.cs — cosmetic asset model
- Assets/Scripts/CosmeticManager.cs — helper with visibility logic
- Assets/Scripts/VirtueItem.cs — ScriptableObject model for virtues
- Assets/Scripts/TelemetryManager.cs, CrashReporter.cs — privacy controls
- Assets/Scripts/UI/* — FruitCollectionUI, FruitItemUI, RainbowLayerUI, ArmorPanelUI, VirtueHallUI, ActivityCenterUI, PastorContactUI, MatureContentUI, QuestNotificationUI (UI controllers)

Next steps
- Open this folder in Unity Editor, create ScriptableObject assets for quests, and wire up to UI prefabs.
- Create a UI Canvas with placeholders for `FruitItem` prefab (Image + Text + Buttons) and assign to `FruitCollectionUI` in the inspector.
- For testing mini‑games: add `MiniGameControllerExample` to a GameObject, assign a `PlayerProfile` and a `questId` (e.g. "WIS-001"), then call `SimulateWin()` from the inspector to complete the quest.
- Add `ToastManager` and `QuestNotificationUI` to a Canvas to see in‑game toasts for quest/virtue awards.
- I can add sample scenes and prefabs on request.

Telemetry & privacy
- Analytics are disabled by default in the project; use `TelemetryManager` to gate any telemetry.
- CrashReporter queues anonymized crash files locally; uploads are gated and opt‑outable.
- Follow `Design/PrivacyPolicy.md` and `Design/DataMinimalism.md` when adding any telemetry or cloud features.