using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace TinyDialogsNet;

[SuppressMessage("Globalization", "CA2101")]
internal static class NativeDialogs
{
    private const string EntryName = "tinyfiledialogs";

    public static string StringFromPointer(IntPtr ptr)
    {
        return OperatingSystem.IsWindows() ? Marshal.PtrToStringUni(ptr) : Marshal.PtrToStringAnsi(ptr);
    }

    #region Windows only (utf16)

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_notifyPopupW(string title, string message, string iconType);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_messageBoxW(string title, string message, string dialogType, string iconType,
        int defaultButton);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_inputBoxW(string title, string message, string placeholder);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_saveFileDialogW(string title, string defaultPathAndFile,
        int numOfFilterPatterns, string[] filterPatterns, string singleFilterDescription);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_openFileDialogW(string title, string defaultPathAndFile,
        int numOfFilterPatterns, string[] filterPatterns, string singleFilterDescription, int allowMultipleSelections);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_selectFolderDialogW(string title, string defaultPathAndFile);

    [DllImport(EntryName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_colorChooserW(string title, string defaultHexRgb, byte[] defaultRgb,
        byte[] resultRgb);

    #endregion

    #region Cross-platform except windows (utf8)

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_notifyPopup(string title, string message, string iconType);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_messageBox(string title, string message, string dialogType, string iconType,
        int defaultButton);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_inputBox(string title, string message, string placeholder);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_saveFileDialog(string title, string defaultPathAndFile, int numOfFilterPatterns,
        string[] filterPatterns, string singleFilterDescription);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_openFileDialog(string title, string defaultPathAndFile, int numOfFilterPatters,
        string[] filterPatterns, string singleFilterDescription, int allowMultipleSelections);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_selectFolderDialog(string title, string defaultPathAndFile);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_colorChooser(string title, string defaultHexRgb, byte[] defaultRgb,
        byte[] resultRgb);

    #endregion

    #region Platform-independent

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr tinyfd_getGlobalChar(string variableName);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_getGlobalInt(string variableName);

    [DllImport(EntryName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern int tinyfd_setGlobalInt(string variableName, int value);

    [DllImport(EntryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void tinyfd_beep();

    #endregion
}