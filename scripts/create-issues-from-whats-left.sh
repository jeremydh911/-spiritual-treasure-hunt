#!/usr/bin/env bash
# Create GitHub issues from WHATS_LEFT.md checklist (requires `gh` and auth)
# Usage: ./scripts/create-issues-from-whats-left.sh [owner/repo]
set -euo pipefail

REPO=${1:-$(git remote get-url origin 2>/dev/null | sed -E 's#(git@github.com:|https://github.com/)([^/]+/[^/.]+).*#\2#' || true)}
if [ -z "$REPO" ]; then
  echo "Usage: $0 owner/repo" >&2
  exit 1
fi

echo "Creating labels (if missing) on $REPO..."
# Priority labels
gh label list -R "$REPO" | grep -F "priority: A" >/dev/null 2>&1 || gh label create "priority: A" --color FF0000 --description "High priority (finish before public beta)" -R "$REPO"
gh label list -R "$REPO" | grep -F "priority: B" >/dev/null 2>&1 || gh label create "priority: B" --color FFA500 --description "Important" -R "$REPO"
gh label list -R "$REPO" | grep -F "priority: C" >/dev/null 2>&1 || gh label create "priority: C" --color 00BFFF --description "Backlog / polish" -R "$REPO"
# Area labels
for L in Backend Godot Content CI Testing UX Policy; do
  gh label list -R "$REPO" | grep -F "$L" >/dev/null 2>&1 || gh label create "$L" --color 0E8A16 -R "$REPO"
done

# Helper: create issue function
create_issue() {
  local title="$1" body="$2" labels="$3"
  gh issue create -R "$REPO" -t "$title" -b "$body" -l "$labels"
}

echo "Creating top-priority (A) issues..."
create_issue "A2 — Parental consent & cloud save enforcement (server + client)" "Area: Backend\nPriority: A\nEstimate: 2–4 days\nNext action: implement server consent checks in /sync/profile; surface consent status in PlayerProfile.CanUseCloudSave() and SaveManager." "priority: A,Backend" 

create_issue "A3 — Church admin override & governance (admin UI + audit)" "Area: Backend\nPriority: A\nEstimate: 2–3 days\nNext action: add adultModeDisabledByChurch admin endpoint and audit entries." "priority: A,Backend"

create_issue "A4 — Content theological vetting workflow & status dashboard" "Area: Content\nPriority: A\nEstimate: 3 days\nNext action: add UI and metadata filter to mark vetStatus → vetted + export for denominational reviewers." "priority: A,Content"

create_issue "A5 — COPPA & GDPR compliance checklist + automated tests" "Area: Policy/QA\nPriority: A\nEstimate: 1–2 days\nNext action: add COPPA checks to CI (no analytics for child accounts) and document retention rules." "priority: A,Policy,CI"

echo "Creating important (B) issues..."
create_issue "B1 — Godot: flesh out MemoryMiniGame UI and add GUT tests" "Area: Godot / Gameplay\nPriority: B\nEstimate: 2–3 days\nNext action: implement UI controls and user input handlers in MemoryMiniGame; add GUT tests." "priority: B,Godot,Testing"

create_issue "B3 — Add Godot automated runtime checks + CI job" "Area: Testing / CI\nPriority: B\nEstimate: 2 days\nNext action: add GUT + GitHub Action to run TruthQuest_IAmChosenTest and TruthsTest (godot-test)." "priority: B,CI,Testing"

create_issue "B4 — Mature Mode: finalize UI/UX + printable guide download (offline first)" "Area: UX / Content\nPriority: B\nEstimate: 1–2 days\nNext action: add SaveLocalGuideToDisk calls and ensure offline fallback exists." "priority: B,Content,UX"

create_issue "B5 — Denominational vet approval integration (per‑church flags)" "Area: Content / Backend\nPriority: B\nEstimate: 2–4 days\nNext action: add denomApprovals[] to content schema and admin endpoints to set approvals." "priority: B,Content,Backend"

echo "Creating backlog / polish (C) issues..."
create_issue "C1 — Full audio/VO pipeline" "Area: Content / Audio\nPriority: C\nNext action: create audio manifest and placeholder clips in GodotProject/Resources." "priority: C,Content"

create_issue "C2 — Art pass (kingdom UI, bridge, character silhouettes)" "Area: Content / Art\nPriority: C\nNext action: prepare artist briefs for each demo scene." "priority: C,Content"

create_issue "C3 — Accessibility features (text size, narration toggle, color contrast)" "Area: UX\nPriority: C\nNext action: add accessibility toggles in Settings and test with screen readers." "priority: C,UX"

create_issue "C4 — Telemetry: opt‑in gating and privacy review" "Area: Policy / Telemetry\nPriority: C\nNext action: finalize events and gate telemetry behind consent; add COPPA guardrails." "priority: C,Policy"

create_issue "C5 — Release: mobile export scripts + store listing checklist" "Area: Release\nPriority: C\nNext action: draft Play Store / App Store metadata; prepare build scripts." "priority: C,CI"

echo "Creating content tracking issues..."
create_issue "Finalize Truths authoring & vetStatus → vetted" "Area: Content\nNext action: editorial review to mark vetStatus as vetted." "Content,priority: C"
create_issue "Localize core Truths (EN→ES/FR)" "Area: Content/Localization\nNext action: add i18n keys to truths_index.json." "Content,priority: C"
create_issue "Move Phase-2 angel quests into phase2_quests.json and mark vetStatus" "Area: Content\nNext action: QA review for phase-2 quests." "Content,priority: C"

echo "All issues created. Review them on GitHub: https://github.com/$REPO/issues"
