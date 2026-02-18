#!/usr/bin/env bash
# Start the backend (installs deps if needed)
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
BACKEND_DIR="$ROOT_DIR/backend"

if ! command -v npm >/dev/null 2>&1; then
  echo "npm not found. Install Node.js (https://nodejs.org/) or: brew install node"
  exit 1
fi

if [ ! -d "$BACKEND_DIR" ]; then
  echo "backend directory not found at: $BACKEND_DIR"
  exit 1
fi

echo "→ Installing backend dependencies (if required)..."
cd "$BACKEND_DIR"
npm install

echo "→ Starting backend (npm start). Use Ctrl-C to stop."
npm start
