#!/usr/bin/env bash
# Run Godot headless tests (expects GUT addon installed in GodotProject)
set -e
ROOT_DIR="$(pwd)"
if ! command -v godot >/dev/null 2>&1; then
  echo "godot CLI not found — install Godot or add 'godot' to PATH"
  exit 0
fi
# Try the common GUT CLI entrypoint; if not present, instruct the user
# ensure GUT addon is present (clone if missing)
if [ ! -d "$ROOT_DIR/GodotProject/addons/gut" ]; then
  echo "GUT addon not found; cloning into project..."
  git clone --depth 1 https://github.com/bitwes/Gut.git "$ROOT_DIR/GodotProject/addons/gut" || true
fi
failed=0
for PROJECT in "GodotProject" "GodotProject3D"; do
  if [ -f "$ROOT_DIR/$PROJECT/addons/gut/gut_cmd_line.gd" ]; then
    echo "running GUT tests in $PROJECT..."
    godot --headless --path "$ROOT_DIR/$PROJECT" -s addons/gut/gut_cmd_line.gd
    status=$?
    if [ $status -ne 0 ]; then
      echo "tests in $PROJECT failed (exit $status)" >&2
      failed=1
    fi
  else
    echo "GUT addon not detected in $PROJECT. Skipping (run tests from the Godot editor)."
  fi
done
if [ $failed -ne 0 ]; then
  exit 1
fi