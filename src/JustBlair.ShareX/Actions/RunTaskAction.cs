using JustBlair.ShareX.ShareX;
using MacroDeck.Localization;
using MacroDeck.Sdk.Actions;

namespace JustBlair.ShareX.Actions;

/// <summary>Runs one ShareX task chosen from a category. One class serves every category, mirroring how
/// ShareX groups its own hotkeys, so the action list stays short where the Macro Deck 2 plugin had one
/// action per task.</summary>
internal sealed class RunTaskAction : IActionDefinition
{
	private readonly ShareXLauncher _launcher;

	public RunTaskAction(ShareXCategory category, ShareXLauncher launcher)
	{
		_launcher = launcher;

		(Id, Name, Description) = category switch
		{
			ShareXCategory.Capture => (ActionIds.Capture, Strings.Actions.Capture.Name(), Strings.Actions.Capture.Description()),
			ShareXCategory.Record => (ActionIds.Record, Strings.Actions.Record.Name(), Strings.Actions.Record.Description()),
			ShareXCategory.Upload => (ActionIds.Upload, Strings.Actions.Upload.Name(), Strings.Actions.Upload.Description()),
			ShareXCategory.Tools => (ActionIds.Tools, Strings.Actions.Tools.Name(), Strings.Actions.Tools.Description()),
			_ => (ActionIds.General, Strings.Actions.General.Name(), Strings.Actions.General.Description())
		};

		var tasks = ShareXTasks.In(category).ToList();
		var parameters = new List<ActionParameter>
		{
			ActionParameter.Choice(ActionParameters.Task,
				[.. tasks.Select(t => new ActionParameterOption { Value = t.Name, Label = t.Label })],
				label: Strings.Fields.Task.Label(),
				defaultValue: tasks[0].Name,
				required: true)
		};

		var fileTasks = tasks.Where(t => t.AcceptsFile).Select(t => t.Name).ToArray();
		if (fileTasks.Length > 0)
		{
			parameters.Add(ActionParameter.File(ActionParameters.File,
					label: Strings.Fields.File.Label(),
					description: Strings.Fields.File.Description())
				.OnlyWhen(ActionParameters.Task, fileTasks));
		}

		parameters.Add(ActionParameters.ShareXLocation());
		Parameters = parameters;
	}

	public string Id { get; }

	public LocalizedText Name { get; }

	public LocalizedText Description { get; }

	public IReadOnlyList<ActionParameter> Parameters { get; }

	public IActionExecutor CreateExecutor() => new Executor(_launcher);

	private sealed class Executor(ShareXLauncher launcher) : IActionExecutor
	{
		public Task<ActionResult> ExecuteAsync(ActionExecutionContext context)
		{
			var name = ActionParameters.Read(context, ActionParameters.Task);
			if (name is null)
			{
				return Task.FromResult(ActionResult.Failed(ActionErrorCodes.InvalidParameter,
					MacroDeckStrings.Validation.Required(Strings.Fields.Task.Label())));
			}

			// Any known task runs, whichever category it belongs to: a migrated button may carry one that
			// ShareX has since moved. A name ShareX does not have would be ignored by ShareX in silence.
			var task = ShareXTasks.Find(name);
			if (task is null)
			{
				return Task.FromResult(ActionResult.Failed(ActionErrorCodes.InvalidParameter, Strings.Errors.UnknownTask(name)));
			}

			var file = task.AcceptsFile ? ActionParameters.Read(context, ActionParameters.File) : null;
			var result = ShareXLauncher.CheckFile(file)
				?? launcher.Run(ShareXCommand.ForTask(task, file), ActionParameters.Read(context, ActionParameters.ShareXPath));

			return Task.FromResult(result);
		}
	}
}
