#!/usr/bin/env bash
# Fast-forward local main to origin/main for playtesting.
# Prefer closing Unity before running so the Editor reloads a clean tree.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

echo "==> Ironstrait playtest sync"
echo "    Repo: $ROOT"
echo

if ! command -v git >/dev/null 2>&1; then
  echo "FAIL: git not found. Install Xcode Command Line Tools (xcode-select --install) or Git."
  exit 1
fi

if ! git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
  echo "FAIL: not a git repository: $ROOT"
  exit 1
fi

echo "==> git fetch origin"
if ! git fetch origin; then
  echo
  echo "FAIL: git fetch failed (network or remotes)."
  exit 1
fi

echo "==> git checkout main"
if ! git checkout main; then
  echo
  echo "FAIL: could not checkout main. Stash or commit local work, then retry."
  exit 1
fi

echo "==> git pull --ff-only origin main"
if git pull --ff-only origin main; then
  echo
  echo "SUCCESS: main is up to date at $(git rev-parse --short HEAD)."
  echo "Open the project in Unity (close Unity first if it was open) and press Play."
  exit 0
fi

echo
echo "FAIL: could not fast-forward main."
echo "      Resolve local commits/changes on main, then retry."
exit 1
