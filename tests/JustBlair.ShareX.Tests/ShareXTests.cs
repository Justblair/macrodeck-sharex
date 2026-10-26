using JustBlair.ShareX.Migration;
using JustBlair.ShareX.ShareX;
using MacroDeck.Sdk.Migration;
using NUnit.Framework;

namespace JustBlair.ShareX.Tests;

[TestFixture]
public sealed class ShareXTests
{
	[Test]
	public void Task_names_are_unique_and_plain_words()
	{
		var names = ShareXTasks.All.Select(t => t.Name).ToList();

		Assert.That(names, Is.Unique);
		Assert.That(names, Has.All.Matches<string>(n => n.All(char.IsAsciiLetterOrDigit)));
	}

	[Test]
	public void Every_category_has_tasks()
	{
		foreach (var category in Enum.GetValues<ShareXCategory>())
		{
			Assert.That(ShareXTasks.In(category), Is.Not.Empty, category.ToString());
		}
	}

	[Test]
	public void A_task_runs_silently_with_a_dash_in_front()
	{
		var command = ShareXCommand.ForTask(ShareXTasks.Find("RectangleRegion")!, null);

		Assert.That(command, Is.EqualTo(new[] { "-silent", "-RectangleRegion" }));
	}

	[Test]
	public void A_file_follows_a_task_that_takes_one_and_is_dropped_otherwise()
	{
		Assert.That(ShareXCommand.ForTask(ShareXTasks.Find("ImageEditor")!, @"C:\a b\c.png"),
			Is.EqualTo(new[] { "-silent", "-ImageEditor", @"C:\a b\c.png" }));
		Assert.That(ShareXCommand.ForTask(ShareXTasks.Find("PrintScreen")!, @"C:\c.png"),
			Is.EqualTo(new[] { "-silent", "-PrintScreen" }));
	}

	[Test]
	public void A_workflow_is_passed_as_one_argument()
	{
		Assert.That(ShareXCommand.ForWorkflow("Capture region to clipboard"),
			Is.EqualTo(new[] { "-silent", "-workflow", "Capture region to clipboard" }));
	}

	[TestCase("jbcarreon123.ShareXPlugin.Actions.OCRAction, ShareX Plugin", "OCR")]
	[TestCase("jbcarreon123.ShareXPlugin.Actions.ScreenRecorderGIFAction", "ScreenRecorderGIF")]
	[TestCase("Something.Else", null)]
	[TestCase("", null)]
	public void The_old_class_name_gives_the_task(string typeName, string? expected)
	{
		Assert.That(MacroDeck2Migration.TaskNameFrom(typeName), Is.EqualTo(expected));
	}

	[TestCase("jbcarreon123.ShareXPlugin.Actions.OCRAction", "tools", "OCR")]
	[TestCase("jbcarreon123.ShareXPlugin.Actions.PrintScreenAction", "capture", "PrintScreen")]
	[TestCase("jbcarreon123.ShareXPlugin.Actions.StopScreenRecordingAction", "record", "StopScreenRecording")]
	[TestCase("jbcarreon123.ShareXPlugin.Actions.ExitShareXAction", "general", "ExitShareX")]
	public void An_old_button_maps_to_its_category_with_no_warnings(string typeName, string actionId, string task)
	{
		var result = MacroDeck2Migration.Migrate(Old(typeName))!;

		Assert.That(result.IntegrationId, Is.EqualTo("uk.justblair.sharex"));
		Assert.That(result.ActionId, Is.EqualTo(actionId));
		Assert.That(result.Parameters["task"].GetString(), Is.EqualTo(task));
		Assert.That(result.Warnings, Is.Empty);
	}

	[Test]
	public void A_dropped_capture_mode_becomes_a_region_capture_with_a_warning()
	{
		var result = MacroDeck2Migration.Migrate(Old("jbcarreon123.ShareXPlugin.Actions.RectangleLightAction"))!;

		Assert.That(result.ActionId, Is.EqualTo("capture"));
		Assert.That(result.Parameters["task"].GetString(), Is.EqualTo("RectangleRegion"));
		Assert.That(result.Warnings, Has.Count.EqualTo(1));
	}

	[Test]
	public void A_task_ShareX_dropped_keeps_its_name_and_warns()
	{
		var result = MacroDeck2Migration.Migrate(Old("jbcarreon123.ShareXPlugin.Actions.TweetMessageAction"))!;

		Assert.That(result.Parameters["task"].GetString(), Is.EqualTo("TweetMessage"));
		Assert.That(result.Warnings, Has.Count.EqualTo(1));
	}

	[Test]
	public void Every_old_plugin_action_migrates()
	{
		// The 61 action classes of the Macro Deck 2 plugin.
		string[] old =
		[
			"AbortScreenRecording", "ActiveMonitor", "ActiveWindow", "AutoCapture", "BorderlessWindow", "ClipboardUpload",
			"ClipboardUploadWithContentViewer", "ClipboardViewer", "ColorPicker", "CustomRegion", "DNSChanger", "DisableHotkeys",
			"DragDropUpload", "ExitShareX", "FileUpload", "FolderUpload", "HashCheck", "ImageCombiner", "ImageEditor", "ImageEffects",
			"ImageSplitter", "ImageThumbnailer", "ImageViewer", "IndexFolder", "InspectWindow", "LastRegion", "MonitorTest", "OCR",
			"OpenHistory", "OpenImageHistory", "OpenMainWindow", "OpenScreenshotsFolder", "PauseScreenRecording", "PrintScreen",
			"QRCode", "QRCodeDecodeFromScreen", "RectangleLight", "RectangleRegion", "RectangleTransparent", "Ruler",
			"ScreenColorPicker", "ScreenRecorder", "ScreenRecorderActiveWindow", "ScreenRecorderCustomRegion", "ScreenRecorderGIF",
			"ScreenRecorderGIFActiveWindow", "ScreenRecorderGIFCustomRegion", "ScrollingCapture", "ShortenURL", "StartAutoCapture",
			"StartScreenRecorder", "StartScreenRecorderGIF", "StopScreenRecording", "StopUploads", "ToggleActionsToolbar",
			"ToggleTrayMenu", "TweetMessage", "UploadText", "UploadURL", "VideoConverter", "VideoThumbnailer"
		];
		string[] dropped = ["DNSChanger", "TweetMessage", "RectangleLight", "RectangleTransparent"];

		Assert.That(old, Has.Length.EqualTo(61));
		foreach (var name in old.Except(dropped))
		{
			Assert.That(ShareXTasks.Find(name), Is.Not.Null, name);
			Assert.That(MacroDeck2Migration.Migrate(Old($"jbcarreon123.ShareXPlugin.Actions.{name}Action"))!.Warnings, Is.Empty, name);
		}
	}

	[TestCase("jbcarreon123.ShareXPlugin.GUI.PluginConfig")]
	[TestCase("jbcarreon123.ShareXPlugin.Actions.NotARealTaskAction")]
	public void Something_that_is_not_a_ShareX_task_is_not_claimed(string typeName)
	{
		Assert.That(MacroDeck2Migration.Migrate(Old(typeName)), Is.Null);
	}

	private static ForeignAction Old(string typeName) =>
		new(typeName, "jbcarreon123.ShareXPlugin", "ShareX button", "", "");
}
