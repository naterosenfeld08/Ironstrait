#!/bin/bash
# Double-click in macOS Finder to sync main for playtesting.
cd "$(dirname "$0")/.." || {
  echo "FAIL: could not find repo root from scripts/pull-main.command"
  echo
  read -r -p "Press Enter to close..."
  exit 1
}

./scripts/pull-main.sh
status=$?

echo
if [ "$status" -eq 0 ]; then
  echo "Done. You can close this window and open Unity."
else
  echo "Sync failed (exit $status). Fix the error above, then try again."
fi
read -r -p "Press Enter to close..."
exit "$status"
