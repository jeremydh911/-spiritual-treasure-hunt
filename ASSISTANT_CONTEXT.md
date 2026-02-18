# Assistant context (persistent)

Purpose: canonical project settings and files the assistant should consult as the primary context for feature work, design decisions, and policy.

Project rules / preferences
- License model: **Freeware / open-source only** (no paid engines, paid assets, or proprietary libraries).  All new dependencies must be OSS-friendly.
- Engine: **Godot 4.2 (Mono)** is the single supported engine. Unity is deprecated and retained only for archival reference.

Files to always consult (canonical)
- `WHATS_LEFT.md` — prioritized work and roadmap
- `ASSISTANT_CONTEXT.md` — this file
- `GodotProject/README.md` — engine + demo setup
- `Content/` — quests, truths, scriptures, guides
- `Design/PrivacyPolicy.md` — COPPA/GDPR constraints

How to use
- Keep this file in the repo root. You do NOT need to attach it to chat messages.
- Open these files in your editor or tell the assistant: "Please use ASSISTANT_CONTEXT.md" and it will consult them for the current session.
- For cross-session persistence: keep this file committed to the repo and request the assistant load it at the start of a new conversation.

Quick open in VS Code
- Run the workspace Task: `Tasks: Open assistant context files` or run `./scripts/open-context.sh`

Notes
- I can add automation (scripts/VS Code tasks) so these files are opened automatically when you open the workspace. Ask me to add that automation if you want.