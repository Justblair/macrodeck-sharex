using JustBlair.ShareX.Actions;
using JustBlair.ShareX.Migration;
using JustBlair.ShareX.ShareX;
using MacroDeck.Sdk;
using MacroDeck.Sdk.Actions;
using MacroDeck.Sdk.Migration;
using Serilog;

namespace JustBlair.ShareX;

/// <summary>
/// Port of jbcarreon123's Macro Deck 2 ShareX plugin. ShareX is driven entirely through its command line,
/// so the integration holds no connection or state: it lists the actions and offers the Macro Deck 2
/// migration.
/// </summary>
public sealed class PluginIntegration : IPluginIntegration, IMigrationProvider
{
	private readonly ILogger _logger;

	public PluginIntegration(ILogger logger)
	{
		_logger = logger.ForContext<PluginIntegration>();
		var launcher = new ShareXLauncher(logger);

		Actions =
		[
			new RunTaskAction(ShareXCategory.Capture, launcher),
			new RunTaskAction(ShareXCategory.Record, launcher),
			new RunTaskAction(ShareXCategory.Upload, launcher),
			new RunTaskAction(ShareXCategory.Tools, launcher),
			new RunTaskAction(ShareXCategory.General, launcher),
			new RunWorkflowAction(launcher)
		];
	}

	public IReadOnlyList<IActionDefinition> Actions { get; }

	public IReadOnlyList<IIntegrationMigration> Migrations { get; } = [new MacroDeck2Migration()];

	/// <summary>Runs again on reconnects and config changes, so it only reports.</summary>
	public Task InitializeAsync(IIntegrationContext context)
	{
		var found = ShareXLocator.Find();
		if (found is null)
		{
			_logger.Warning("ShareX was not found automatically. Buttons will need its location set.");
		}
		else
		{
			_logger.Information("Using ShareX at {Path}.", found);
		}

		return Task.CompletedTask;
	}

	public Task ShutdownAsync() => Task.CompletedTask;
}
