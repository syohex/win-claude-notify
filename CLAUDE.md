# win-claude-notify

## 概要

- Windows専用 claude hooks通知ツール
- claude hooksの input JSONを stdinから読み取り, eventに応じたアイコンとデスクトップ通知を表示する
- 通知機能は `BurntToast` に委ね, 自前で実装はしない
