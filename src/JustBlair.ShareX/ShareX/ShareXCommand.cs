namespace JustBlair.ShareX.ShareX;

/// <summary>Builds ShareX's command line. Kept free of process and file-system calls so it can be tested.
/// ShareX is single-instance: a second launch hands its arguments to the copy already running, and a
/// first launch starts ShareX and runs the task. <c>-silent</c> stops a fresh start from also opening the
/// main window; a running copy ignores it.</summary>
internal static class ShareXCommand
{
	public const string Silent = "-silent";

	public static IReadOnlyList<string> ForTask(ShareXTask task, string? filePath) =>
		filePath is not null && task.AcceptsFile
			? [Silent, "-" + task.Name, filePath]
			: [Silent, "-" + task.Name];

	public static IReadOnlyList<string> ForWorkflow(string workflowName) => [Silent, "-workflow", workflowName];
}
