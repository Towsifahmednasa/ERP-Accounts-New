#!/usr/bin/env bash
# Source-protection check (brief section 31): recomputes SHA-256 of every file under D:\Projects\Unicorn\Accounts
# (excluding .git/.vs/graphify-out) and compares with the baseline taken before migration work started (2026-10-07).
# Exit 0 = legacy source untouched. Run from Git Bash: bash docs/migration/tools/verify-legacy-unchanged.sh
set -e
here="$(cd "$(dirname "$0")" && pwd)"
cd "D:/Projects/Unicorn/Accounts"
find . -type f -not -path './.git/*' -not -path './.vs/*' -not -path './graphify-out/*' -not -path './CSTL_Pro_to_Pay/CSTL_Pro_to_Pay/graphify-out/*' -print0 | sort -z | xargs -0 sha256sum > "$TEMP/legacy-now.sha256"
if diff -q "$here/legacy-baseline.sha256" "$TEMP/legacy-now.sha256" > /dev/null; then
  echo "OK: legacy source unchanged ($(wc -l < "$here/legacy-baseline.sha256") files)"
else
  echo "CHANGED files:"; diff "$here/legacy-baseline.sha256" "$TEMP/legacy-now.sha256" | grep '^[<>]' | awk '{print $1, $3}'; exit 1
fi
