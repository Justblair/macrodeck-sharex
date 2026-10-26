using System.Text.Json;
using JustBlair.ShareX.Actions;
using JustBlair.ShareX.ShareX;
using MacroDeck.Localization;
using MacroDeck.Sdk.Migration;

namespace JustBlair.ShareX.Migration;

/// <summary>Carries buttons from jbcarreon123's Macro Deck 2 ShareX plugin over to this one. That plugin
/// had one action class per task, named <c>&lt;Task&gt;Action</c>, so the class name is the ShareX task
/// and maps straight onto this plugin's category action with that task chosen.</summary>
internal sealed class MacroDeck2Migration : IIntegrationMigration
{
	public const string PluginId = "uk.justblair.sharex";

	/// <summary>The old plugin's identifiers. How Macro Deck 3 names a Macro Deck 2 action source is not
	/// documented, so both the package id and the display name are claimed.</summary>
	public static readonly IReadOnlyList<string> OldSources = ["jbcarreon123.ShareXPlugin", "ShareX Plugin"];

	/// <summary>Tasks ShareX has dropped since the old plugin was written, and what replaces them where
	/// something does.</summary>
	private static readonly Dictionary<string, string?> Removed = new(StringComparer.OrdinalIgnoreCase)
	{
		["RectangleLight"] = "RectangleRegion",
		["RectangleTransparent"] = "RectangleRegion",
		["DNSChanger"] = null,
		["TweetMessage"] = null
	};

	public MigrationSource Source => MigrationSource.MacroDeck2;

	public IReadOnlyList<string> ClaimedActionSources => OldSources;

	// The old plugin's only setting was the ShareX folder. This port finds ShareX by itself, so there is
	// no setting to bring across.
	public IReadOnlyList<string> ClaimedSettingsSources => [];

	public Task<ActionMigrationResult?> MigrateActionAsync(ForeignAction action, CancellationToken cancellationToken)
		=> Task.FromResult(Migrate(action));

	public Task<IReadOnlyList<MigratedConfiguration>> MigrateConfigurationAsync(ForeignPluginSettings settings, CancellationToken cancellationToken)
		=> Task.FromResult<IReadOnlyList<MigratedConfiguration>>([]);

	/// <summary>Null for an action that is not one of the old plugin's task actions, which tells the host
	/// this plugin cannot migrate it.</summary>
	internal static ActionMigrationResult? Migrate(ForeignAction action)
	{
		var oldTask = TaskNameFrom(action.TypeName);
		if (oldTask is null)
		{
			return null;
		}

		var warnings = new List<LocalizedText>();
		var taskName = oldTask;

		if (Removed.TryGetValue(oldTask, out var replacement))
		{
			if (replacement is not null)
			{
				warnings.Add(Strings.Migration.TaskReplaced(oldTask: oldTask, newTask: replacement));
				taskName = replacement;
			}
			else
			{
				warnings.Add(Strings.Migration.TaskRemoved(oldTask));
			}
		}

		var task = ShareXTasks.Find(taskName);
		if (task is null && warnings.Count == 0)
		{
			// Not a task this plugin or ShareX knows.
			return null;
		}

		var parameters = new Dictionary<string, JsonElement>
		{
			[ActionParameters.Task] = JsonSerializer.SerializeToElement(task?.Name ?? taskName)
		};

		// A dropped task has no category; App control is where the button stays editable until the user
		// picks something else.
		return new ActionMigrationResult(PluginId, ActionIdFor(task?.Category ?? ShareXCategory.General), action.DisplayName ?? taskName, parameters, warnings);
	}

	/// <summary>"jbcarreon123.ShareXPlugin.Actions.OCRAction, ShareX Plugin" becomes "OCR". The assembly
	/// part is optional, in case the type name arrives without it.</summary>
	internal static string? TaskNameFrom(string? typeName)
	{
		if (string.IsNullOrWhiteSpace(typeName))
		{
			return null;
		}

		var type = typeName.Split(',')[0].Trim();
		var className = type[(type.LastIndexOf('.') + 1)..];
		const string suffix = "Action";
		return className.EndsWith(suffix, StringComparison.Ordinal) && className.Length > suffix.Length
			? className[..^suffix.Length]
			: null;
	}

	private static string ActionIdFor(ShareXCategory category) => category switch
	{
		ShareXCategory.Capture => ActionIds.Capture,
		ShareXCategory.Record => ActionIds.Record,
		ShareXCategory.Upload => ActionIds.Upload,
		ShareXCategory.Tools => ActionIds.Tools,
		_ => ActionIds.General
	};
}
