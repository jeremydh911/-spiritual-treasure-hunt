#!/bin/bash
# Helper script to export Godot project for Android using CLI.
# Requires Godot executable with Android export templates installed.

PROJECT_DIR="$(pwd)"
GODOT_BIN="godot"  # adjust if using custom path

# example export preset name configured in project.godot
PRESET="Android"
OUTPUT="${PROJECT_DIR}/build/SpiritualTreasure-android.apk"

mkdir -p "${PROJECT_DIR}/build"

"${GODOT_BIN}" --export "${PRESET}" "${OUTPUT}"

if [ $? -eq 0 ]; then
  echo "Android export succeeded: ${OUTPUT}"
else
  echo "Android export failed" >&2
  exit 1
fi
