using MacroDeck.Localization;

namespace JustBlair.ShareX.ShareX;

/// <summary>The task groups ShareX itself uses in its hotkey settings. Each one is a Macro Deck action.</summary>
internal enum ShareXCategory
{
	Capture,
	Record,
	Upload,
	Tools,
	General
}

/// <summary>One ShareX task. <paramref name="Name"/> is the <c>HotkeyType</c> member ShareX expects after
/// a dash on its command line; <paramref name="AcceptsFile"/> marks the tasks ShareX opens a file with when
/// a path follows the task name.</summary>
internal sealed record ShareXTask(string Name, ShareXCategory Category, LocalizedText Label, bool AcceptsFile = false);
