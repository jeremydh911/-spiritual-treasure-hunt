# What’s Left to Do — Consolidated TODO

This file collects outstanding work across engine ports, content, backend, policy, QA, and release tasks. Use it as a single prioritized checklist for the next sprints.

---

## How to use
- High priority (A) → finish before public beta.
- Medium (B) → ship in early releases / sprint backlog.
- Low (C) → polish, future work.

Each line shows: task, area, priority, rough estimate, and the exact next action.

---

## Top priority (A)
- [x] A1 — Production age‑verification integration (backend + UI) — **implemented (demo adapter + webhook + tests)**
  - Area: Backend / Security
  - Priority: A
  - Estimate: 3–5 days
  - Notes: provider adapter scaffold added (`backend/providers/ageProvider.js`), `/verify/age/provider` and `/verify/age/webhook` endpoints implemented, Unity client helper added, and backend tests created.

- [ ] A2 — Parental consent & cloud save enforcement (server + client)
  - Area: Backend / SaveManager
  - Priority: A
  - Estimate: 2–4 days
  - Next action: implement server consent checks in `/sync/profile`, surface consent status in `PlayerProfile.CanUseCloudSave()` and `SaveManager`.

- [ ] A3 — Church admin override & governance (admin UI + audit)
  - Area: Backend / Admin
  - Priority: A
  - Estimate: 2–3 days
  - Next action: add `adultModeDisabledByChurch` admin endpoint and audit entries.

- [ ] A4 — Content theological vetting workflow & status dashboard
  - Area: Content / Editorial
  - Priority: A
  - Estimate: 3 days
  - Next action: add UI and metadata filter to mark `vetStatus` → `vetted` + export for denominational reviewers.

- [x] A5 — COPPA & GDPR compliance checklist + automated tests
  - Area: Policy / QA
  - Priority: A
  - Estimate: 1–2 days
  - Notes: telemetry endpoint added with COPPA guard, backend tests expanded, privacy policy updated with retention rules.

---

## Important (B)
- [ ] B1 — Godot: flesh out MemoryMiniGame UI (interactive) and add GUT tests
  - Area: Godot / Gameplay
  - Priority: B
  - Estimate: 2–3 days
  - Next action: implement UI controls and user input handlers in `MemoryMiniGame`.

- [x] B2 — Unity support deprecated; Godot is primary engine
  - Area: Demo / Migration
  - Status: Completed — Unity artifacts retained for archival (`UnityProject/`). Active development is Godot-only.
  - Next action: optionally delete or archive `UnityProject/` (ask me to add an archive script).

- [ ] B3 — Tests: add Godot automated runtime checks + CI job for Godot (GUT or headless run)
  - Area: Testing / CI
  - Priority: B
  - Estimate: 2 days
  - Next action: add GUT + CI workflow to run `TruthQuest_IAmChosenTest` and `TruthsTest`.
  - Area: Testing / CI
  - Priority: B
  - Estimate: 2 days
  - Next action: add GUT + CI workflow to run `TruthQuest_IAmChosenTest` and `TruthsTest`.

- [x] B4 — Mature Mode: finalize UI/UX + printable guide download offline first
  - Area: UI / Content
  - Priority: B
  - Estimate: 1–2 days
  - Next action: added `MatureGuideUI` with download button and `SaveLocalGuideToDisk` (tests added).

- [ ] B5 — Denominational vet approval integration (data + per‑church flags)
  - Area: Content / Backend
  - Priority: B
  - Estimate: 2–4 days
  - Next action: add `denomApprovals[]` to content schema and admin endpoints to set approvals.

- [ ] B6 — 3D engine prototype & asset pipeline
  - Area: Gameplay / Engine
  - Priority: B
  - Estimate: 3–5 days
  - Next action: build simple 3D scenes (`SinBridgeDemo3D`, `MemoryMiniGame3D`) and verify Godot 3D workflows.

---

## Backlog / polish (C)
- [ ] C1 — Full audio/VO pipeline (narration + affirmation clips)
  - Next action: create audio manifest and placeholder clips in `Assets/Audio/` and `GodotProject/Resources/`.

- [ ] C2 — Art pass (kingdom UI, bridge, character silhouettes)
  - Next action: prepare artist briefs for each demo scene.

- [x] C3 — Accessibility features (text size, narration toggle, color contrast)
  - Next action: accessibility toggles added to 3D settings UI with corresponding GUT tests.

- [x] C4 — Telemetry: opt‑in gating and privacy review
  - Next action: backend stamps consent timestamp and enforces COPPA/opt-in; tests added.

- [x] C5 — Release: mobile export scripts + store listing checklist
  - Next action: helper export scripts (`scripts/export-android.sh`, `export-ios.sh`) and metadata template added; fill with real graphics & copy.

---

## Content tasks (tracking)
- [ ] Finalize Truths authoring & vetStatus → `vetted` (Content/Truths) — Next: editorial review.
- [ ] Localize core Truths (EN → ES/FR) — Next: add i18n keys to `truths_index.json`.
- [ ] Move Phase‑2 angel quests into `phase2_quests.json` and mark `vetStatus` — Next: QA review.

---

## CI & automation
- [ ] Remove Unity CI support; add Godot CI job to run headless GUT tests — Next: add `godot-test` workflow.
- [ ] Add Godot CI job to run headless test scenes (GUT or custom runner) — Next: add `godot-test` workflow.

---

## Quick commands / dev shortcuts
- Start backend (demo): `./scripts/start-backend.sh`  
- Open Godot project: `./scripts/open-godot.sh` or open Godot and load `GodotProject/`  
- Open assistant context files: `./scripts/open-context.sh`  
- Run Godot headless tests (GUT): `./scripts/run-godot-tests.sh`

---

## Immediate recommended next steps (pick one)
- [ ] Implement production age‑verification (A1) — high impact and required before shipping Mature Mode.  
- [ ] Finish Godot `MemoryMiniGame` UI (B1) — completes the learning flow for `I Am Chosen`.  
- [ ] Add Godot CI + run proof tests (B3) — ensures Godot scaffold is testable on CI.

---

If you want, I will: (A) create issues from this checklist, (B) start implementing A1 now, or (C) implement the Godot mini‑game UI next. Tell me which option to run. 
