Migration notes — Unity → Godot (C#)

Scope
- This scaffold ports core runtime systems to Godot C# (PlayerProfile, QuestManager,
  ScriptureManager, MatureContentManager) and provides demo scenes for key
  gameplay examples (SinBridge, TruthBarrier).

Key differences to expect
- Scene / node model: Godot uses Nodes and Scenes (TSCN files) instead of Unity
  GameObjects/Prefabs. UI is Control nodes instead of Unity UI components.
- Signals replace UnityEvents; use `EmitSignal` / `Connect` in Godot.
- Animation/coroutines: use `CreateTimer` + `ToSignal` or _Process loops.
- C# API differences: UnityEngine.* APIs are replaced with Godot.* equivalents.

Files to migrate next (recommended order)
1. UI screens (VirtueHall, ActivityCenter)
2. Mini‑games (MemoryMatch, Rhythm) — port to Godot Scenes
3. TelemetryManager / CrashReporter (adapt to Godot or stub for offline builds)
4. Backend integration (uses same REST endpoints)

Testing & CI
- Godot tests can be added using the GodotUnitTesting framework (GUT) or custom
  test scenes. We'll add tests as we port systems.

Notes on content
- All learning content remains offline in repo `Content/` and `GodotProject/Resources`.
- Backend remains optional for parental consent and cloud sync.
