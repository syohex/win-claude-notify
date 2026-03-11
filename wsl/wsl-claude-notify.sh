#!/usr/bin/env bash
set -euo pipefail

INPUT=$(cat)

NAME=$(/mnt/c/Windows/System32/cmd.exe /C "echo %USERNAME%" 2>/dev/null | tr -d '\r')
if [[ -z "$NAME" ]]; then
  exit 0
fi

TOOLDIR="/mnt/c/Users/$NAME/.dotnet/tools"
EXE="${TOOLDIR}/win-claude-notify.exe"

if [[ ! -x "$EXE" ]]; then
  exit 0
fi

echo "$INPUT" | "$EXE"
