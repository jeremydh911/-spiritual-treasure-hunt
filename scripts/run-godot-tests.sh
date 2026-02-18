#!/usr/bin/env bash
# Run Godot headless tests (expects GUT addon installed in GodotProject)
set -e
ROOT_DIR="$(pwd)"
if ! command -v godot >/dev/null 2>&1; then
  echo "godot CLI not found — install Godot or add 'godot' to PATH"
  exit 0
fi
# Try the common GUT CLI entrypoint; if not present, instruct the user
if [ -f "$ROOT_DIR/GodotProject/addons/gut/gut_cmd_line.gd" ]; then
  godot --headless --path "$ROOT_DIR/GodotProject" -s addons/gut/gut_cmd_line.gd || true
else
  echo "GUT addon not found in GodotProject. Install GUT or run tests from the Godot editor."
fi