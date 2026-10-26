using MacroDeck.Plugin.Testing;
using NUnit.Framework;

namespace JustBlair.ShareX.Tests;

/// <summary>Behaviour through <see cref="PluginTestHarness"/>: the plugin's own handlers run, nothing
/// crosses a socket, and no test here starts ShareX.</summary>
[TestFixture]
public sealed class PluginIntegrationTests
{
	private static PluginTestHarness CreateHarness() =>
		PluginTestHarness.Create(builder => builder
			.UseLocalization(Strings.LocalizationCatalog)
			.RegisterIntegration<PluginIntegration>());

	[Test]
	public async Task The_plugin_builds_and_initializes()
	{
		await using var harness = CreateHarness();

		Assert.DoesNotThrowAsync(harness.InitializeIntegrationsAsync);
	}

	[TestCase("capture")]
	[TestCase("record")]
	[TestCase("upload")]
	[TestCase("tools")]
	[TestCase("general")]
	public async Task Every_task_action_refuses_an_unknown_task(string actionId)
	{
		await using var harness = CreateHarness();
		await harness.InitializeIntegrationsAsync();

		var outcome = await harness.Actions.ExecuteAsync(actionId, new Dictionary<string, object?> { ["task"] = "DNSChanger" });

		Assert.That(outcome.Succeeded, Is.False);
	}

	[Test]
	public async Task A_task_action_refuses_a_missing_task()
	{
		await using var harness = CreateHarness();
		await harness.InitializeIntegrationsAsync();

		var outcome = await harness.Actions.ExecuteAsync("capture", new Dictionary<string, object?>());

		Assert.That(outcome.Succeeded, Is.False);
	}

	[Test]
	public async Task A_file_that_does_not_exist_is_refused_before_ShareX_is_started()
	{
		await using var harness = CreateHarness();
		await harness.InitializeIntegrationsAsync();

		var outcome = await harness.Actions.ExecuteAsync("tools", new Dictionary<string, object?>
		{
			["task"] = "ImageEditor",
			["file"] = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".png")
		});

		Assert.That(outcome.Succeeded, Is.False);
	}

	[Test]
	public async Task A_ShareX_location_with_no_ShareX_is_refused()
	{
		await using var harness = CreateHarness();
		await harness.InitializeIntegrationsAsync();

		var outcome = await harness.Actions.ExecuteAsync("capture", new Dictionary<string, object?>
		{
			["task"] = "PrintScreen",
			["sharexPath"] = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
		});

		Assert.That(outcome.Succeeded, Is.False);
	}

	[Test]
	public async Task Run_workflow_needs_a_name()
	{
		await using var harness = CreateHarness();
		await harness.InitializeIntegrationsAsync();

		var outcome = await harness.Actions.ExecuteAsync("run-workflow", new Dictionary<string, object?>());

		Assert.That(outcome.Succeeded, Is.False);
	}
}
