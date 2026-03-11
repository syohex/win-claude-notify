using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.Json;

// See hook input section https://code.claude.com/docs/en/hooks

// 1. read hook input
string input;
using (var reader = new StreamReader(Console.OpenStandardInput()))
{
    input = reader.ReadToEnd();
}

if (string.IsNullOrWhiteSpace(input))
    return;

// 2. Parse hook_event_name
using var doc = JsonDocument.Parse(input);
if (!doc.RootElement.TryGetProperty("hook_event_name", out var eventNameElement))
    return;

var eventName = eventNameElement.GetString();

// Extract project name from cwd
var projectName = "";
if (doc.RootElement.TryGetProperty("cwd", out var cwdElement))
{
    projectName = Path.GetFileName(cwdElement.GetString() ?? "");
}

// 3. Determine notification content
string title;
string body;
string iconFileName;

switch (eventName)
{
    case "Stop":
        title = "Claude";
        body = "応答が完了しました";
        iconFileName = "check.png";
        break;
    case "Notification":
        title = "Claude";
        body = "入力を待っています";
        iconFileName = "question.png";
        break;
    default:
        return;
}

if (!string.IsNullOrEmpty(projectName))
{
    title = $"Claude - {projectName}";
}

// 4. Ensure icon exists in temp directory
var iconDir = Path.Combine(Path.GetTempPath(), "win-claude-notify");
Directory.CreateDirectory(iconDir);
var iconPath = Path.Combine(iconDir, iconFileName);

if (!File.Exists(iconPath))
{
    GenerateIcon(iconPath, iconFileName == "check.png");
}

// 5. Send notification via BurntToast
var escapedTitle = title.Replace("'", "''");
var escapedBody = body.Replace("'", "''");
var escapedIconPath = iconPath.Replace("'", "''");

var script = $"New-BurntToastNotification -Text '{escapedTitle}','{escapedBody}' -AppLogo '{escapedIconPath}'";

using var process = Process.Start(new ProcessStartInfo
{
    FileName = "pwsh.exe",
    ArgumentList = { "-NoProfile", "-Command", script },
    UseShellExecute = false,
    CreateNoWindow = true,
});
process?.WaitForExit();

static void GenerateIcon(string path, bool isCheck)
{
    const int size = 64;

    using var bitmap = new Bitmap(size, size);
    using var g = Graphics.FromImage(bitmap);

    g.SmoothingMode = SmoothingMode.AntiAlias;

    // Background circle
    var bgColor = isCheck ? Color.FromArgb(76, 175, 80) : Color.FromArgb(33, 150, 243);
    using (var brush = new SolidBrush(bgColor))
    {
        g.FillEllipse(brush, 2, 2, size - 4, size - 4);
    }

    // White symbol
    using var pen = new Pen(Color.White, 5f) { LineJoin = LineJoin.Round, StartCap = LineCap.Round, EndCap = LineCap.Round };

    if (isCheck)
    {
        // Checkmark
        g.DrawLines(pen, new Point[]
        {
            new(16, 33),
            new(27, 44),
            new(48, 20),
        });
    }
    else
    {
        // Question mark
        using var font = new Font("Segoe UI", 32, FontStyle.Bold);
        using var brush = new SolidBrush(Color.White);
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString("?", font, brush, new RectangleF(0, -2, size, size), sf);
    }

    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
}
