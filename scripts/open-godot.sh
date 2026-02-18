#!/usr/bin/env bash
# Open the Godot project (editor CLI or macOS open fallback)
set -e
ROOT_DIR="$(pwd)"
if command -v godot >/dev/null 2>&1; then
  godot --path "$ROOT_DIR/GodotProject" -e || true
else
  open -a Godot "$ROOT_DIR/GodotProject" || echo "Install Godot or add 'godot' to PATH to use CLI."
fi