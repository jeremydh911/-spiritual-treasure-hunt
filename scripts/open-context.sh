#!/usr/bin/env bash
# Open canonical assistant context files in VS Code (requires `code` CLI)
set -e
code WHATS_LEFT.md ASSISTANT_CONTEXT.md GodotProject/README.md || {
  echo "Unable to open with 'code' CLI — open these files manually: WHATS_LEFT.md, ASSISTANT_CONTEXT.md, GodotProject/README.md"
  exit 0
}