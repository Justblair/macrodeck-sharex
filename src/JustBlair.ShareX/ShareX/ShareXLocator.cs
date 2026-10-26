using System.Diagnostics;
using Microsoft.Win32;

namespace JustBlair.ShareX.ShareX;

/// <summary>Finds ShareX.exe. A path chosen on the button wins; otherwise the copy that is already
/// running, then the installer's registry entries, then the usual install folders (installer, Steam and
/// the Microsoft Store alias).</summary>
internal static class ShareXLocator
{
	public const string ExecutableName = "ShareX.exe";

	/// <summary>Resolves a user-supplied location, which may be ShareX.exe itself or the folder holding it.
	/// Returns null when nothing usable is there.</summary>
	public static string? FromUserPath(string path)
	{
		var trimmed = path.Trim().Trim('"');
		if (File.Exists(trimmed))
		{
			return trimmed;
		}

		var inFolder = Path.Combine(trimmed, ExecutableName);
		return File.Exists(inFolder) ? inFolder : null;
	}

	public static string? Find()
	{
		foreach (var candidate in Candidates())
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(candidate) && File.Exists(candidate))
				{
					return candidate;
				}
			}
			catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or ArgumentException)
			{
				// A bad candidate is just skipped.
			}
		}

		return null;
	}

	private static IEnumerable<string?> Candidates()
	{
		yield return RunningCopy();

		// The registry exists only on Windows; skipping it elsewhere keeps the tests runnable on any OS.
		var hives = OperatingSystem.IsWindows() ? new[] { Registry.CurrentUser, Registry.LocalMachine } : [];
		foreach (var hive in hives)
		{
			yield return RegistryFolder(hive, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ShareX_is1", "InstallLocation");
			yield return RegistryFolder(hive, @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\ShareX_is1", "InstallLocation");
			yield return RegistryValue(hive, @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ShareX.exe", null);
		}

		yield return InFolder(Environment.SpecialFolder.ProgramFiles, "ShareX");
		yield return InFolder(Environment.SpecialFolder.ProgramFilesX86, "ShareX");
		yield return InFolder(Environment.SpecialFolder.ProgramFilesX86, @"Steam\steamapps\common\ShareX");
		yield return InFolder(Environment.SpecialFolder.LocalApplicationData, @"Microsoft\WindowsApps");
	}

	private static string? RunningCopy()
	{
		foreach (var process in Process.GetProcessesByName("ShareX"))
		{
			using (process)
			{
				try
				{
					var path = process.MainModule?.FileName;
					if (path is not null)
					{
						return path;
					}
				}
				catch (Exception exception) when (exception is System.ComponentModel.Win32Exception or InvalidOperationException)
				{
					// An elevated ShareX cannot be inspected from here; fall through to the other places.
				}
			}
		}

		return null;
	}

	private static string? RegistryFolder(RegistryKey hive, string key, string value)
	{
		var folder = RegistryValue(hive, key, value);
		return folder is null ? null : Path.Combine(folder.Trim('"'), ExecutableName);
	}

	private static string? RegistryValue(RegistryKey hive, string key, string? value)
	{
		try
		{
			using var opened = hive.OpenSubKey(key);
			return (opened?.GetValue(value) as string)?.Trim('"');
		}
		catch (Exception exception) when (exception is System.Security.SecurityException or UnauthorizedAccessException or IOException)
		{
			return null;
		}
	}

	private static string? InFolder(Environment.SpecialFolder root, string relative)
	{
		var rootPath = Environment.GetFolderPath(root);
		return string.IsNullOrEmpty(rootPath) ? null : Path.Combine(rootPath, relative, ExecutableName);
	}
}
