using MacroDeck.Sdk.Actions;

namespace JustBlair.ShareX.Actions;

/// <summary>Parameter names are bound to users' buttons, so they live in one place and never change.</summary>
internal static class ActionParameters
{
	public const string Task = "task";
	public const string File = "file";
	public const string ShareXPath = "sharexPath";
	public const string Workflow = "workflow";

	/// <summary>Optional on every action and normally left blank: ShareX is found automatically. Needed
	/// only for a portable copy or one installed somewhere unusual.</summary>
	public static ActionParameter ShareXLocation() =>
		ActionParameter.File(ShareXPath,
			label: Strings.Fields.ShareXPath.Label(),
			description: Strings.Fields.ShareXPath.Description(),
			fileExtensions: [".exe"]);

	public static string? Read(ActionExecutionContext context, string name) =>
		Values.ReadString(context.Parameters.GetValueOrDefault(name));
}
