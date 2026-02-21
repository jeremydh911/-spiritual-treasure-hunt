Spiritual Treasure Hunt — RPG Game (prototype)

Overview
- Short: a cross-platform (iOS/Android) Unity-based educational RPG with mini‑games and quests that teach biblical virtues and lead players toward spiritual growth.
- Target age: 7–18 (Mature Mode 18+ via age verification)
- Core themes: virtues (Love, Joy, Peace, etc.), Scripture-based quests (Genesis→Revelation), community/asynchronous features, "whimsy not magic" design.
- Alternative engine support: a Godot 4.2 (C#) scaffold is available at `GodotProject/` for migration or parallel development. A separate 3D prototype lives in `GodotProject3D/` with basic scenes (SinBridgeDemo3D, MemoryMiniGame3D) to explore a fully three‑dimensional look and feel.
- Assistant context: see `ASSISTANT_CONTEXT.md` (commit it to the repo and open it in the editor — you won't need to drag files into chat).

What's in this workspace (initial scaffold)
- Design/: game design docs, policies, and quest/spec templates
- Content/BibleIndex.json: canonical book mapping for quest coverage
- Content/Quests/: Phase‑1 quest JSON samples
- UnityProject/: archived Unity artifacts (deprecated). Use `GodotProject/` for active development

Next steps you can ask for
- I scaffolded UI prefabs, sample scenes, and mini‑game prototypes (MemoryMatch, Rhythm, HelpingNPC, CourageTrial).
- Backend prototype included: parental consent flows, demo auth, profile persistence, audit logging, and downloadable guides.

Offline first: all core learning content (quests, identity truths, Scripture study, and printable guides) is available locally via the `Content/` and `GodotProject/Resources/` folders — the app does not require internet to teach and store learning material.
- Content features added: Activities center (Service, Mentoring, Sabbath, Artistic Expression), Virtues, Fruits, Spiritual Armor, Angels, and Mature‑Mode guides.
- I can expand content (map 30 Phase‑1 quests across the Bible) or harden the backend for production.

How to proceed locally
- Required engine: Godot 4.2 (Mono-enabled). Open the `GodotProject/` folder in Godot Editor.
- Open the `GodotProject/` folder in Godot to generate C# project files and run demo scenes.
- Review `Design/QuestSpec.md` and `Content/Quests/phase1_quests.json` for content structure and sample quests.

Privacy & telemetry (defaults)
- Analytics: OFF by default for all child accounts; analytics events must be explicitly enabled and reviewed.
- Crash reports: anonymized crash summaries are enabled by default and are opt‑outable in Settings.
- Cloud saves and any PII: only with parental consent; no advertising or third‑party trackers in child builds.
- See `Design/PrivacyPolicy.md` and `Design/DataMinimalism.md` for implementation details.

Questions? Tell me which module (content, UI/UX, backend, or Unity systems) you want implemented next.