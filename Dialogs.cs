namespace TinyDialogsNet;

using System.Globalization;
using System.Runtime.InteropServices;

public static class Dialogs
{
	#region Wrapper types

	public enum PopupIconType
	{
		Warning,
		Error,
		Info,
	}

	public enum MessageBoxIconType
	{
		Warning,
		Error,
		Info,
		Question
	}

	public enum MessageBoxButtons
	{
		Ok,
		OkCancel,
		YesNo,
		YesNoCancel
	}

	public enum MessageBoxDefaultButton
	{
		CancelNo = 0,
		OkYes = 1,
		NoInYnc = 2
	}

	public enum InputBoxType
	{
		Default,
		Password
	}

	#endregion

	#region Wrapper methods

	public static string InputBox(InputBoxType type, string title = "", string message = "") =>
		Marshal.PtrToStringAuto(tinyfd_inputBox(title, message, type == InputBoxType.Default ? "" : null));

	public static int MessageBox(MessageBoxButtons buttons, MessageBoxIconType iconType,
		MessageBoxDefaultButton defaultButton, string title = "", string message = "") => tinyfd_messageBox(title,
		message, buttons.ToString().ToLower(),
		iconType.ToString().ToLower(), (int) defaultButton);

	public static void Beep() => tinyfd_beep();

	public static int NotifyPopup(PopupIconType iconType, string title = "", string message = "") =>
		tinyfd_notifyPopup(title, message, iconType.ToString().ToLower());

	public static string SaveFileDialog(string title = "", string defaultPath = "", string filter = "",
		string filterName = "")
	{
		IntPtr result;
		if (string.IsNullOrEmpty(filter))
			result = tinyfd_saveFileDialog(title, defaultPath, 0, null, null);
		else
			result = tinyfd_saveFileDialog(title, defaultPath, 1, new[] {filter},
				filterName);
		return Marshal.PtrToStringAuto(result);
	}

	public static IEnumerable<string> OpenFileDialog(string title = "", string defaultPath = "",
		IEnumerable<string> filterPatterns = null,
		string filterName = "", bool allowMultipleSelects = false)
	{
		IntPtr result;
		if (filterPatterns is null)
			result = tinyfd_openFileDialog(title, defaultPath, 0, Array.Empty<string>(), "",
				allowMultipleSelects ? 1 : 0);
		else
		{
			var array = filterPatterns.ToArray();
			result = tinyfd_openFileDialog(title, defaultPath, array.Length, array, filterName,
				allowMultipleSelects ? 1 : 0);
		}

		return Marshal.PtrToStringAuto(result)?.Split("|");
	}

	public static string SelectFolderDialog(string title = "", string defaultPath = "")
	{
		var result = tinyfd_selectFolderDialog(title, defaultPath);
		return Marshal.PtrToStringAuto(result);
	}

	public static string ColorChooser(string title = "", string defaultHex = "")
	{
		var rgb = HexToRgb(defaultHex).ToArray();
		var result = tinyfd_colorChooser(title, defaultHex, rgb, rgb);
		return Marshal.PtrToStringAnsi(result);
	}

	#endregion

	#region Imports

	private const string EntryName = "tinyfiledialogs";

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl)]
	private static extern void tinyfd_beep();

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern int tinyfd_notifyPopup(string title, string message, string iconType);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern int tinyfd_messageBox(string title, string message, string dialogType, string iconType,
		int defaultButton);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern IntPtr tinyfd_inputBox(string title, string message, string defaultInput);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern IntPtr tinyfd_saveFileDialog(string title, string defaultPath, int numOfFilterPatterns,
		string[] filters, string filterDescription);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern IntPtr tinyfd_openFileDialog(string title, string defaultPath, int numOfFilterPatterns,
		string[] filterPatterns, string filterDescription, int allowMultipleSelects);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern IntPtr tinyfd_colorChooser(string title, string defaultHex, byte[] defaultRgb,
		byte[] oResultRgb);

	[DllImport(EntryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private static extern IntPtr tinyfd_selectFolderDialog(string title, string defaultPath);

	#endregion

	#region Helper methods

	public static IEnumerable<byte> HexToRgb(string hex)
	{
		hex = hex.Replace("#", "");
		var r = byte.Parse(hex[..2], NumberStyles.AllowHexSpecifier);
		var g = byte.Parse(hex[2..4], NumberStyles.AllowHexSpecifier);
		var b = byte.Parse(hex[4..6], NumberStyles.AllowHexSpecifier);
		return new[] {r, g, b};
	}

	public static string RgbToHex(byte[] rgb) => $"#{rgb[0]:x}{rgb[1]:x}{rgb[2]:x}";

	#endregion
}