using System.Diagnostics;
using MacroDeck.Sdk.Actions;
using Serilog;

namespace JustBlair.ShareX.ShareX;

/// <summary>Finds ShareX and starts it with a prepared command line, turning every failure into an
/// action result the button can show.</summary>
internal sealed class ShareXLauncher(ILogger logger)
{
	private readonly ILogger _logger = logger.ForContext<ShareXLauncher>();

	public ActionResult Run(IReadOnlyList<string> arguments, string? sharexPath)
	{
		string? executable;
		if (sharexPath is not null)
		{
			executable = ShareXLocator.FromUserPath(sharexPath);
			if (executable is null)
			{
				_logger.Warning("No ShareX.exe at the location set on the button: {Path}.", sharexPath);
				return ActionResult.Failed(ActionErrorCodes.InvalidParameter, Strings.Errors.ShareXPathInvalid(sharexPath));
			}
		}
		else
		{
			executable = ShareXLocator.Find();
			if (executable is null)
			{
				_logger.Warning("ShareX.exe could not be found automatically.");
				return ActionResult.Failed(ActionErrorCodes.NotFound, Strings.Errors.ShareXNotFound());
			}
		}

		var startInfo = new ProcessStartInfo(executable)
		{
			UseShellExecute = false,
			WorkingDirectory = Path.GetDirectoryName(executable) ?? string.Empty
		};
		foreach (var argument in arguments)
		{
			startInfo.ArgumentList.Add(argument);
		}

		try
		{
			using var process = Process.Start(startInfo);
			_logger.Information("Ran {Executable} {Arguments}.", executable, string.Join(' ', arguments));
			return ActionResult.Success();
		}
		catch (Exception exception) when (exception is System.ComponentModel.Win32Exception or InvalidOperationException or IOException)
		{
			_logger.Error(exception, "Starting {Executable} failed.", executable);
			return ActionResult.Failed(ActionErrorCodes.ProviderError, Strings.Errors.LaunchFailed());
		}
	}

	/// <summary>The optional file a task opens. A path that does not exist is refused here, because ShareX
	/// drops the whole command without a word when it cannot find the file.</summary>
	public static ActionResult? CheckFile(string? filePath) =>
		filePath is null || File.Exists(filePath)
			? null
			: ActionResult.Failed(ActionErrorCodes.InvalidParameter, Strings.Errors.FileNotFound(filePath));
}
