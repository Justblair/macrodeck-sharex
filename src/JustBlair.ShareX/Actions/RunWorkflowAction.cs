using JustBlair.ShareX.ShareX;
using MacroDeck.Localization;
using MacroDeck.Sdk.Actions;

namespace JustBlair.ShareX.Actions;

/// <summary>Runs one of the user's own ShareX hotkeys by name (<c>-workflow</c>), with every setting that
/// hotkey has in ShareX: its destinations, after-capture steps and so on. New in this port.</summary>
internal sealed class RunWorkflowAction(ShareXLauncher launcher) : IActionDefinition
{
	public string Id => ActionIds.RunWorkflow;

	public LocalizedText Name => Strings.Actions.RunWorkflow.Name();

	public LocalizedText Description => Strings.Actions.RunWorkflow.Description();

	public IReadOnlyList<ActionParameter> Parameters { get; } =
	[
		ActionParameter.Text(ActionParameters.Workflow,
			label: Strings.Fields.Workflow.Label(),
			description: Strings.Fields.Workflow.Description(),
			placeholder: Strings.Fields.Workflow.Placeholder(),
			required: true),
		ActionParameters.ShareXLocation()
	];

	public IActionExecutor CreateExecutor() => new Executor(launcher);

	private sealed class Executor(ShareXLauncher launcher) : IActionExecutor
	{
		public Task<ActionResult> ExecuteAsync(ActionExecutionContext context)
		{
			var workflow = ActionParameters.Read(context, ActionParameters.Workflow);
			return Task.FromResult(workflow is null
				? ActionResult.Failed(ActionErrorCodes.InvalidParameter, MacroDeckStrings.Validation.Required(Strings.Fields.Workflow.Label()))
				: launcher.Run(ShareXCommand.ForWorkflow(workflow), ActionParameters.Read(context, ActionParameters.ShareXPath)));
		}
	}
}
