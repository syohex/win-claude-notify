#!/usr/bin/env bash
set -euo pipefail

TOOL_NAME="win-claude-notify"
INSTALL_DIR="$HOME/.dotnet/tools"
PROJECT_DIR="$(cd "$(dirname "$0")" && pwd)"

echo "Building ${TOOL_NAME} (Release)..."
dotnet publish "$PROJECT_DIR" -c Release -r win-x64 --self-contained -o "$PROJECT_DIR/publish"

echo "Installing to ${INSTALL_DIR}..."
mkdir -p "$INSTALL_DIR"
cp "$PROJECT_DIR/publish/${TOOL_NAME}.exe" "$INSTALL_DIR/${TOOL_NAME}.exe"

echo "Done. Installed ${INSTALL_DIR}/${TOOL_NAME}.exe"
