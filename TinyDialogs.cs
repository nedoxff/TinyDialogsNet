using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace TinyDialogsNet;

/// <summary>
/// The entrypoint to all dialogs provided by the wrapper.
/// </summary>
public static class TinyDialogs
{
    private const char MultipleFileSeparator = '|';

    /// <summary>
    ///     Produces a short beep sound effect (when possible).
    /// </summary>
    public static void Beep()
    {
        NativeDialogs.tinyfd_beep();
    }

    /// <summary>
    ///     Shows a notification popup.
    /// </summary>
    /// <param name="icon">The icon of the notification. See <see cref="NotificationIconType" /> for values.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The body of the notification.</param>
    // Intentional, as the return value is only useful for tinyfd_query, which is not included in this library.
    [SuppressMessage("Performance", "CA1806")]
    public static void NotifyPopup(NotificationIconType icon, string title, string message)
    {
        if (OperatingSystem.IsWindows()) NativeDialogs.tinyfd_notifyPopupW(title, message, icon.ToNativeString());
        else NativeDialogs.tinyfd_notifyPopup(title, message, icon.ToNativeString());
    }

    /// <summary>
    ///     Shows a message box.
    /// </summary>
    /// <param name="title">The title of the message box.</param>
    /// <param name="message">The body of the message box.</param>
    /// <param name="type">The buttons displayed in the message box. See <see cref="MessageBoxDialogType" /> for values.</param>
    /// <param name="icon">The icon of the message box. See <see cref="MessageBoxIconType" /> for values.</param>
    /// <param name="defaultButton">The button which will be focused by default.</param>
    /// <remarks>The <see cref="title" /> parameter cannot contain new line or tab characters. </remarks>
    /// <returns>The button which the user clicked.</returns>
    /// <exception cref="ArgumentException">If the title string contained invalid characters.</exception>
    public static MessageBoxButton MessageBox(string title, string message,
        MessageBoxDialogType type,
        MessageBoxIconType icon, MessageBoxButton defaultButton)
    {
        if (title.Contains('\n') || title.Contains('\t'))
            throw new ArgumentException("The title cannot contain new line or tab characters.", nameof(title));

        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_messageBoxW(title, message, type.ToNativeString(), icon.ToNativeString(),
                defaultButton.ToNativeValue(type))
            : NativeDialogs.tinyfd_messageBox(title, message, type.ToNativeString(), icon.ToNativeString(),
                defaultButton.ToNativeValue(type));

        return response.ToButton(type);
    }

    /// <summary>
    ///     Shows an input box (prompt).
    /// </summary>
    /// <param name="type">Defines what information should be inputted. Can be regular text or a password.</param>
    /// <param name="title">The title of the input box.</param>
    /// <param name="message">The body of the input box.</param>
    /// <param name="placeholder">The placeholder.</param>
    /// <remarks>The placeholder will always be empty if <see cref="type" /> is <see cref="InputBoxType.Password" />.</remarks>
    /// <returns>
    ///     If canceled, returns an empty string and "true" for the "canceled" parameter. Otherwise, returns the text the
    ///     user inputted.
    /// </returns>
    public static (bool Canceled, string Text) InputBox(InputBoxType type, string title, string message,
        string placeholder = "")
    {
        var nativePlaceholder = type == InputBoxType.Password ? null : placeholder;
        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_inputBoxW(title, message, nativePlaceholder)
            : NativeDialogs.tinyfd_inputBox(title, message, nativePlaceholder);

        return response == IntPtr.Zero ? (true, "") : (false, NativeDialogs.StringFromPointer(response));
    }

    /// <summary>
    ///     Shows a save file dialog.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="defaultPath">The default file/directory which will be shown on startup.</param>
    /// <param name="filter">The filter which will control the output path.</param>
    /// <remarks>
    ///     A forward slash ("/") may be added to <see cref="defaultPath" /> to show the default directory instead of a
    ///     file.
    /// </remarks>
    /// <returns>
    ///     If canceled, returns an empty path and "true" for the "canceled" parameter. Otherwise, returns the path the
    ///     user selected.
    /// </returns>
    public static (bool Canceled, string Path) SaveFileDialog(string title, string defaultPath = "",
        FileFilter filter = default)
    {
        var patternCount = filter?.Patterns?.Count() ?? 0;
        var patterns = filter?.Patterns?.ToArray() ?? Array.Empty<string>();

        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_saveFileDialogW(title, defaultPath, patternCount, patterns, filter?.Name)
            : NativeDialogs.tinyfd_saveFileDialog(title, defaultPath, patternCount, patterns, filter?.Name);

        return response == IntPtr.Zero ? (true, "") : (false, NativeDialogs.StringFromPointer(response));
    }

    /// <summary>
    ///     Shows an open file dialog.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="defaultPath">The default file/directory which will be shown on startup.</param>
    /// <param name="allowMultipleSelections">Defines whether the user can pick multiple files in the dialog.</param>
    /// <param name="filter">The filter which will control the output path.</param>
    /// <remarks>
    ///     A forward slash ("/") may be added to <see cref="defaultPath" /> to show the default directory instead of a
    ///     file.
    /// </remarks>
    /// <returns>
    ///     If canceled, returns an empty array and "true" for the "canceled" parameter. Otherwise, returns a list of
    ///     paths the user selected.
    /// </returns>
    public static (bool Canceled, IEnumerable<string> Paths) OpenFileDialog(string title, string defaultPath = "",
        bool allowMultipleSelections = false, FileFilter filter = default)
    {
        var patternCount = filter?.Patterns?.Count() ?? 0;
        var patterns = filter?.Patterns?.ToArray() ?? Array.Empty<string>();

        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_openFileDialogW(title, defaultPath, patternCount, patterns, filter?.Name,
                allowMultipleSelections ? 1 : 0)
            : NativeDialogs.tinyfd_openFileDialog(title, defaultPath, patternCount, patterns, filter?.Name,
                allowMultipleSelections ? 1 : 0);

        return response == IntPtr.Zero
            ? (true, Array.Empty<string>())
            : (false, NativeDialogs.StringFromPointer(response).Split(MultipleFileSeparator));
    }

    /// <summary>
    ///     Shows a select folder dialog.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="defaultPath">The default directory which will be shown on startup.</param>
    /// <returns>
    ///     If canceled, returns an empty path and "true" for the "canceled" parameter. Otherwise, returns the path the
    ///     user selected.
    /// </returns>
    public static (bool Canceled, string Path) SelectFolderDialog(string title, string defaultPath = "")
    {
        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_selectFolderDialogW(title, defaultPath)
            : NativeDialogs.tinyfd_selectFolderDialog(title, defaultPath);

        return response == IntPtr.Zero ? (true, "") : (false, NativeDialogs.StringFromPointer(response));
    }

    /// <summary>
    ///     Shows a color chooser dialog.
    /// </summary>
    /// <param name="title">The title of the dialog.</param>
    /// <param name="defaultColor">
    ///     The default color which will be shown on startup. If not specified, black (#000000) will be
    ///     used.
    /// </param>
    /// <remarks>
    ///     The <see cref="defaultColor" /> parameter must be a valid #RRGGBB string, otherwise this method will throw an
    ///     <see cref="ArgumentException" />.
    /// </remarks>
    /// <returns>
    ///     If canceled, returns an empty string and "true" for the "canceled" parameter. Otherwise, returns the color the
    ///     user selected in #RRGGBB format.
    /// </returns>
    /// <exception cref="ArgumentException">If the <see cref="defaultColor" /> parameter was in an invalid format.</exception>
    public static (bool Canceled, string Color) ColorChooser(string title, string defaultColor = "#000000")
    {
        var rgb = DialogHelpers.HexToRgb(defaultColor);

        var response = OperatingSystem.IsWindows()
            ? NativeDialogs.tinyfd_colorChooserW(title, defaultColor, rgb, rgb)
            : NativeDialogs.tinyfd_colorChooser(title, defaultColor, rgb, rgb);

        return response == IntPtr.Zero ? (true, "") : (false, NativeDialogs.StringFromPointer(response));
    }

    /// <summary>
    ///     Gets a native tinyfiledialog string variable.
    /// </summary>
    /// <param name="variable">The type of the variable to get.</param>
    /// <returns>The value of the variable.</returns>
    public static string GetGlobalStringVariable(StringVariable variable)
    {
        // The getGlobalChar method doesn't have a Windows variant, so the returned string is always UTF-8.
        return Marshal.PtrToStringAnsi(NativeDialogs.tinyfd_getGlobalChar(variable.ToNativeName()));
    }

    /// <summary>
    ///     Gets a native tinyfiledialog integer variable.
    /// </summary>
    /// <param name="variable">The type of the variable to get.</param>
    /// <returns>The value of the variable.</returns>
    public static int GetGlobalIntegerVariable(IntegerVariable variable)
    {
        return NativeDialogs.tinyfd_getGlobalInt(variable.ToNativeName());
    }

    /// <summary>
    ///     Sets the value of a native tinyfiledialog integer variable.
    /// </summary>
    /// <param name="variable">The type of the variable to change.</param>
    /// <param name="value">The new value of the variable.</param>
    // Intentional, as the returned integer value is the same as the "value" parameter.
    [SuppressMessage("Performance", "CA1806")]
    public static void SetGlobalIntegerVariable(IntegerVariable variable, int value)
    {
        NativeDialogs.tinyfd_setGlobalInt(variable.ToNativeName(), value);
    }
}