# INSTALL (macOS)

Quick setup instructions for this repository on macOS — Unity 2022.3 LTS + Node/npm + running the backend and demo scenes.

## Requirements
- macOS
- Unity Hub + Unity Editor 2022.3 LTS
- Node.js (LTS) and npm
- (recommended) Homebrew for easy installs

---

## 1) Install Homebrew (optional but recommended)
Copy/paste:

/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

## 2) Install Godot 4.2 (Mono-enabled)
- Preferred: download Godot 4.2 (Mono) from https://godotengine.org/download
- Homebrew: `brew install --cask godot`

Open Godot and load the `GodotProject/` folder.  
Tip: install the Mono-enabled Godot build if you need C# support.

---

## 3) Install Node.js & npm
Copy/paste (Homebrew):

brew install node

Or install Node LTS from https://nodejs.org/

## 4) Start the backend server
You can run the backend with the helper script or manually.

Copy/paste (helper):

./scripts/start-backend.sh

Or manually:

cd backend
npm install
npm start

The backend runs on the port configured in `backend/server.js` (see `backend/README.md`).

Note: core learning content (quests, truths, scripture references and printable guides) is available locally in the app and does NOT require internet access — the backend is only needed for cloud features (parental consent, profile sync, downloadable PDFs).

---

## 5) Open the Godot project
Copy/paste (helper):

./scripts/open-godot.sh

Manual options:

open -a Godot "$(pwd)/GodotProject"

or use the `godot` CLI if installed: `godot --path "$(pwd)/GodotProject" -e`

---

## 6) Run Godot tests
- Use GUT (Godot Unit Test) or the Godot test runner (headless).
- Preferred: install the GUT addon in `GodotProject/` and run tests from the editor (Project → Tools → Run Tests).
- CI/headless: run `./scripts/run-godot-tests.sh` (calls Godot headless GUT runner).

---

## 7) Open and run demo scenes
Demo scenes (open in Unity):
- `Assets/Scenes/SinBridgeDemo.unity`
- `Assets/Scenes/TruthBarrierDemo.unity`

If a demo scene is missing, use the Editor menu:

Tools → Spiritual Demo → Create SinBridge Demo Scene
Tools → Spiritual Demo → Create TruthBarrier Demo Scene

Then open the scene and press Play.

---

## Troubleshooting
- `npm` not found → `brew install node` or install from nodejs.org
- Unity Hub/editor not found → install from https://unity.com/download or `brew install --cask unity-hub`
- Backend port conflicts → check `backend/server.js` and environment variables

---

## Quick summary
- Start backend: `./scripts/start-backend.sh`
- Open Unity: `./scripts/open-unity.sh`
- Demo scenes & Tools menu: `Tools → Spiritual Demo → Create ... Demo Scene`

Happy testing 🎮
