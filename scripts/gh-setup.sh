#!/usr/bin/env bash
# Run repo setup steps that require `gh` auth:
#  - enable branch protection for main
#  - run create-issues-from-whats-left.sh
#  - create draft release v0.1.0
# Usage: ./scripts/gh-setup.sh [owner/repo]
set -euo pipefail

REPO=${1:-$(git remote get-url origin 2>/dev/null | sed -E 's#(git@github.com:|https://github.com/)([^/]+/[^/.]+).*#\2#' || true)}
if [ -z "$REPO" ]; then
  echo "Usage: $0 owner/repo" >&2
  exit 1
fi

echo "Enabling branch protection on main for $REPO..."
gh api --method PUT \
  /repos/$REPO/branches/main/protection \
  -f required_status_checks='{"strict":true,"contexts":["godot-test"]}' \
  -f enforce_admins=true \
  -f required_pull_request_reviews='{"dismiss_stale_reviews":true,"required_approving_review_count":1}'

echo "Creating issues from WHATS_LEFT.md..."
./scripts/create-issues-from-whats-left.sh "$REPO"

echo "Creating draft release v0.1.0..."
gh release create v0.1.0 -R "$REPO" --draft --title "v0.1.0 — Godot scaffold" --notes "Initial Godot scaffold, content and CI placeholder. See WHATS_LEFT.md for next steps." 

echo "Done. Visit: https://github.com/$REPO"