#!/bin/bash
# Helper script for exporting the Godot project to iOS. Requires Xcode and Godot export templates.

PROJECT_DIR="$(pwd)"
GODOT_BIN="godot"
PRESET="iOS"
OUTPUT_DIR="${PROJECT_DIR}/build/ios"

mkdir -p "${OUTPUT_DIR}"

"${GODOT_BIN}" --export "${PRESET}" "${OUTPUT_DIR}/SpiritualTreasure.zip"

if [ $? -eq 0 ]; then
  echo "iOS export succeeded. Unzip Xcode project in ${OUTPUT_DIR}."
else
  echo "iOS export failed" >&2
  exit 1
fi
