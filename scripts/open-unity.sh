#!/usr/bin/env bash
# DEPRECATED — Unity support is deprecated in this repo; use GodotProject/ for active development.
# This helper remains for archival purposes only.
# Open the Unity project (tries Unity Hub, falls back to editor executable)
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
UNITY_PROJECT="$ROOT_DIR/UnityProject"

if [ ! -d "$UNITY_PROJECT" ]; then
  echo "UnityProject folder not found: $UNITY_PROJECT"
  exit 1
fi

echo "→ Opening Unity project: $UNITY_PROJECT"

# 1) Try unityhub CLI
if command -v unityhub >/dev/null 2>&1; then
  echo "Using unityhub CLI..."
  unityhub open -p "$UNITY_PROJECT" || unityhub -p "$UNITY_PROJECT" || true
  exit 0
fi

# 2) Try macOS 'open' with Unity Hub
if open -a "Unity Hub" "$UNITY_PROJECT" >/dev/null 2>&1; then
  echo "Requested Unity Hub to open the project."
  exit 0
fi

# 3) Try to find a Unity editor executable (look for 2022.3 installs)
unity_exec=$(ls -1 /Applications/Unity/Hub/Editor/2022.3*/*/Unity.app/Contents/MacOS/Unity 2>/dev/null | tail -n1 || true)
if [ -x "${unity_exec:-}" ]; then
  echo "Opening with Unity editor executable: $unity_exec"
  "$unity_exec" -projectPath "$UNITY_PROJECT" &
  disown
  exit 0
fi

# 4) Try generic Unity app
if open -a "Unity" "$UNITY_PROJECT" >/dev/null 2>&1; then
  echo "Requested Unity app to open the project."
  exit 0
fi

# Fallback: instructions
cat <<EOF
Could not automatically open the Unity project (Unity Hub / Unity editor not found).
Install Unity Hub (https://unity.com/download) or open the project manually:

  open -a "Unity Hub" "$UNITY_PROJECT"
  /Applications/Unity/Hub/Editor/2022.3.X/Unity.app/Contents/MacOS/Unity -projectPath "$UNITY_PROJECT"

Also see: Tools → Spiritual Demo → Create SinBridge / TruthBarrier Demo Scene (creates demo scenes in Assets/Scenes).
EOF
exit 2
