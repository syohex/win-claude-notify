# win-claude-notify

Windows向け Claude Code hooksの通知ツール

## 依存関係

- [BurntToast](https://github.com/Windos/BurntToast) — Windows 10/11 向けトースト通知 PowerShell モジュール

### BurntToast のインストール

PowerShell (pwsh) で以下を実行:

```powershell
Install-Module -Name BurntToast -Scope CurrentUser
```

## インストール

```powershell
.\install.ps1
```

`%USERPROFILE%\.dotnet\tools\win-claude-notify.exe` にインストールされます。

## Claude Code settings.json の設定例

### Windows

`%USERPROFILE%\.claude\settings.json`:

```json
{
  "hooks": {
    "Stop": [
      {
        "type": "command",
        "command": "win-claude-notify"
      }
    ],
    "Notification": [
      {
        "type": "command",
        "command": "win-claude-notify"
      }
    ]
  }
}
```

### WSL

`~/.claude/settings.json`:

```json
{
  "hooks": {
    "Stop": [
      {
        "type": "command",
        "command": "wsl-claude-notify.sh"
      }
    ],
    "Notification": [
      {
        "type": "command",
        "command": "wsl-claude-notify.sh"
      }
    ]
  }
}
```

WSL の場合、`wsl/wsl-claude-notify.sh` がホスト側の `win-claude-notify.exe` を呼び出して通知を表示します。
