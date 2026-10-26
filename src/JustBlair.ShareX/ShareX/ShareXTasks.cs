using MacroDeck.Localization;

namespace JustBlair.ShareX.ShareX;

/// <summary>Every task ShareX accepts as <c>-&lt;name&gt;</c> on its command line, in ShareX's own order.
/// Generated from the <c>HotkeyType</c> enum in ShareX/Enums.cs; names must match it exactly.</summary>
internal static class ShareXTasks
{
	public static IReadOnlyList<ShareXTask> All { get; } =
	[
		new("FileUpload", ShareXCategory.Upload, Strings.Tasks.FileUpload(), AcceptsFile: true),
		new("FolderUpload", ShareXCategory.Upload, Strings.Tasks.FolderUpload()),
		new("ClipboardUpload", ShareXCategory.Upload, Strings.Tasks.ClipboardUpload()),
		new("ClipboardUploadWithContentViewer", ShareXCategory.Upload, Strings.Tasks.ClipboardUploadWithContentViewer()),
		new("UploadText", ShareXCategory.Upload, Strings.Tasks.UploadText()),
		new("UploadURL", ShareXCategory.Upload, Strings.Tasks.UploadURL()),
		new("DragDropUpload", ShareXCategory.Upload, Strings.Tasks.DragDropUpload()),
		new("ShortenURL", ShareXCategory.Upload, Strings.Tasks.ShortenURL()),
		new("StopUploads", ShareXCategory.Upload, Strings.Tasks.StopUploads()),
		new("PrintScreen", ShareXCategory.Capture, Strings.Tasks.PrintScreen()),
		new("ActiveWindow", ShareXCategory.Capture, Strings.Tasks.ActiveWindow()),
		new("CustomWindow", ShareXCategory.Capture, Strings.Tasks.CustomWindow()),
		new("ActiveMonitor", ShareXCategory.Capture, Strings.Tasks.ActiveMonitor()),
		new("RectangleRegion", ShareXCategory.Capture, Strings.Tasks.RectangleRegion()),
		new("CustomRegion", ShareXCategory.Capture, Strings.Tasks.CustomRegion()),
		new("LastRegion", ShareXCategory.Capture, Strings.Tasks.LastRegion()),
		new("ScrollingCapture", ShareXCategory.Capture, Strings.Tasks.ScrollingCapture()),
		new("AutoCapture", ShareXCategory.Capture, Strings.Tasks.AutoCapture()),
		new("StartAutoCapture", ShareXCategory.Capture, Strings.Tasks.StartAutoCapture()),
		new("StopAutoCapture", ShareXCategory.Capture, Strings.Tasks.StopAutoCapture()),
		new("ScreenRecorder", ShareXCategory.Record, Strings.Tasks.ScreenRecorder()),
		new("ScreenRecorderActiveWindow", ShareXCategory.Record, Strings.Tasks.ScreenRecorderActiveWindow()),
		new("ScreenRecorderCustomRegion", ShareXCategory.Record, Strings.Tasks.ScreenRecorderCustomRegion()),
		new("StartScreenRecorder", ShareXCategory.Record, Strings.Tasks.StartScreenRecorder()),
		new("ScreenRecorderGIF", ShareXCategory.Record, Strings.Tasks.ScreenRecorderGIF()),
		new("ScreenRecorderGIFActiveWindow", ShareXCategory.Record, Strings.Tasks.ScreenRecorderGIFActiveWindow()),
		new("ScreenRecorderGIFCustomRegion", ShareXCategory.Record, Strings.Tasks.ScreenRecorderGIFCustomRegion()),
		new("StartScreenRecorderGIF", ShareXCategory.Record, Strings.Tasks.StartScreenRecorderGIF()),
		new("StopScreenRecording", ShareXCategory.Record, Strings.Tasks.StopScreenRecording()),
		new("PauseScreenRecording", ShareXCategory.Record, Strings.Tasks.PauseScreenRecording()),
		new("AbortScreenRecording", ShareXCategory.Record, Strings.Tasks.AbortScreenRecording()),
		new("ColorPicker", ShareXCategory.Tools, Strings.Tasks.ColorPicker()),
		new("ScreenColorPicker", ShareXCategory.Tools, Strings.Tasks.ScreenColorPicker()),
		new("Ruler", ShareXCategory.Tools, Strings.Tasks.Ruler()),
		new("MouseHighlighter", ShareXCategory.Tools, Strings.Tasks.MouseHighlighter()),
		new("PinToScreen", ShareXCategory.Tools, Strings.Tasks.PinToScreen(), AcceptsFile: true),
		new("PinToScreenFromScreen", ShareXCategory.Tools, Strings.Tasks.PinToScreenFromScreen()),
		new("PinToScreenFromClipboard", ShareXCategory.Tools, Strings.Tasks.PinToScreenFromClipboard()),
		new("PinToScreenFromFile", ShareXCategory.Tools, Strings.Tasks.PinToScreenFromFile(), AcceptsFile: true),
		new("PinToScreenCloseAll", ShareXCategory.Tools, Strings.Tasks.PinToScreenCloseAll()),
		new("ImageEditor", ShareXCategory.Tools, Strings.Tasks.ImageEditor(), AcceptsFile: true),
		new("ImageBeautifier", ShareXCategory.Tools, Strings.Tasks.ImageBeautifier(), AcceptsFile: true),
		new("ImageEffects", ShareXCategory.Tools, Strings.Tasks.ImageEffects(), AcceptsFile: true),
		new("ImageViewer", ShareXCategory.Tools, Strings.Tasks.ImageViewer(), AcceptsFile: true),
		new("BackgroundRemover", ShareXCategory.Tools, Strings.Tasks.BackgroundRemover()),
		new("ImageComparer", ShareXCategory.Tools, Strings.Tasks.ImageComparer()),
		new("IconConverter", ShareXCategory.Tools, Strings.Tasks.IconConverter()),
		new("ImageCombiner", ShareXCategory.Tools, Strings.Tasks.ImageCombiner()),
		new("ImageSplitter", ShareXCategory.Tools, Strings.Tasks.ImageSplitter()),
		new("ImageResizer", ShareXCategory.Tools, Strings.Tasks.ImageResizer()),
		new("ImageConverter", ShareXCategory.Tools, Strings.Tasks.ImageConverter()),
		new("ImageWatermark", ShareXCategory.Tools, Strings.Tasks.ImageWatermark()),
		new("ImageThumbnailer", ShareXCategory.Tools, Strings.Tasks.ImageThumbnailer()),
		new("VideoConverter", ShareXCategory.Tools, Strings.Tasks.VideoConverter(), AcceptsFile: true),
		new("VideoTrimmer", ShareXCategory.Tools, Strings.Tasks.VideoTrimmer(), AcceptsFile: true),
		new("VideoThumbnailer", ShareXCategory.Tools, Strings.Tasks.VideoThumbnailer()),
		new("AnalyzeImage", ShareXCategory.Tools, Strings.Tasks.AnalyzeImage()),
		new("OCR", ShareXCategory.Tools, Strings.Tasks.OCR(), AcceptsFile: true),
		new("QRCode", ShareXCategory.Tools, Strings.Tasks.QRCode(), AcceptsFile: true),
		new("QRCodeDecodeFromScreen", ShareXCategory.Tools, Strings.Tasks.QRCodeDecodeFromScreen()),
		new("QRCodeScanRegion", ShareXCategory.Tools, Strings.Tasks.QRCodeScanRegion()),
		new("HashCheck", ShareXCategory.Tools, Strings.Tasks.HashCheck(), AcceptsFile: true),
		new("Metadata", ShareXCategory.Tools, Strings.Tasks.Metadata(), AcceptsFile: true),
		new("StripMetadata", ShareXCategory.Tools, Strings.Tasks.StripMetadata(), AcceptsFile: true),
		new("IndexFolder", ShareXCategory.Tools, Strings.Tasks.IndexFolder()),
		new("ClipboardViewer", ShareXCategory.Tools, Strings.Tasks.ClipboardViewer()),
		new("BorderlessWindow", ShareXCategory.Tools, Strings.Tasks.BorderlessWindow()),
		new("ActiveWindowBorderless", ShareXCategory.Tools, Strings.Tasks.ActiveWindowBorderless()),
		new("ActiveWindowTopMost", ShareXCategory.Tools, Strings.Tasks.ActiveWindowTopMost()),
		new("InspectWindow", ShareXCategory.Tools, Strings.Tasks.InspectWindow()),
		new("NetworkMonitor", ShareXCategory.Tools, Strings.Tasks.NetworkMonitor()),
		new("MonitorTest", ShareXCategory.Tools, Strings.Tasks.MonitorTest()),
		new("DisableHotkeys", ShareXCategory.General, Strings.Tasks.DisableHotkeys()),
		new("OpenMainWindow", ShareXCategory.General, Strings.Tasks.OpenMainWindow()),
		new("OpenScreenshotsFolder", ShareXCategory.General, Strings.Tasks.OpenScreenshotsFolder()),
		new("OpenHistory", ShareXCategory.General, Strings.Tasks.OpenHistory()),
		new("OpenImageHistory", ShareXCategory.General, Strings.Tasks.OpenImageHistory()),
		new("ToggleActionsToolbar", ShareXCategory.General, Strings.Tasks.ToggleActionsToolbar()),
		new("ToggleTrayMenu", ShareXCategory.General, Strings.Tasks.ToggleTrayMenu()),
		new("ExitShareX", ShareXCategory.General, Strings.Tasks.ExitShareX()),
		new("AnimatedGifTrimmer", ShareXCategory.Tools, Strings.Tasks.AnimatedGifTrimmer(), AcceptsFile: true)
	];

	private static readonly Dictionary<string, ShareXTask> ByName = All.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

	public static ShareXTask? Find(string? name) => name is not null && ByName.TryGetValue(name, out var task) ? task : null;

	public static IEnumerable<ShareXTask> In(ShareXCategory category) => All.Where(t => t.Category == category);
}
