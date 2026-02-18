Godot project scaffold (C# - Godot 4.2)

This folder contains a Godot 4.2 project scaffold using C# so we can port the core
systems from the Unity prototype and validate demo scenes (SinBridge, TruthBarrier).

Quick start
- Install Godot 4.2 (Mono-enabled) and open this folder as a project.
- Godot will generate C# project files automatically on first open.
- Open `res://scenes/SinBridgeDemo.tscn` or `res://scenes/TruthBarrierDemo.tscn` and press Play.

Notes
- Core data (quests, truths, scriptures, mature guides) are read from the repo's
  `Content/` and `GodotProject/Resources/` folders — offline‑first.
- The goal of this scaffold is to reuse C# logic from Unity where possible and
  provide parity for gameplay demos and content loaders.

Files added
- `Scripts/` — C# ports for PlayerProfile, QuestManager, ScriptureManager, MatureContentManager, demo UI.
- `Scenes/` — demo scenes for `SinBridge`, `TruthBarrier`, and `TruthQuest_IAmChosen`.
- `Scenes/TruthQuest_IAmChosenTest.tscn` — runtime QA test for the "I Am Chosen" flow (writes PASS/FAIL to `user://`).

Next steps
- Run the demo scenes and validate behaviour.
- Port additional UI and mini‑games on demand.
