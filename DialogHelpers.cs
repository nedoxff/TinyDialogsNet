using System.Globalization;

namespace TinyDialogsNet;

/// <summary>
///     Describes a file filter used in open/save file dialogs.
/// </summary>
/// <param name="Name">The name of the filter (i.e. "Image files")</param>
/// <param name="Patterns">The wildcards that match the required files (i.e. "*.png")</param>
public record FileFilter(string Name, IEnumerable<string> Patterns = default);

internal static class DialogHelpers
{
    public static string ToNativeString(this NotificationIconType type)
    {
        return type switch
        {
            NotificationIconType.Information => "info",
            NotificationIconType.Warning => "warning",
            NotificationIconType.Error => "error",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static string ToNativeString(this MessageBoxIconType type)
    {
        return type switch
        {
            MessageBoxIconType.Information => "info",
            MessageBoxIconType.Warning => "warning",
            MessageBoxIconType.Error => "error",
            MessageBoxIconType.Question => "question",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static string ToNativeString(this MessageBoxDialogType type)
    {
        return type switch
        {
            MessageBoxDialogType.Ok => "ok",
            MessageBoxDialogType.OkCancel => "okcancel",
            MessageBoxDialogType.YesNo => "yesno",
            MessageBoxDialogType.YesNoCancel => "yesnocancel",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static MessageBoxButton ToButton(this int nativeResponse, MessageBoxDialogType type)
    {
        return nativeResponse switch
        {
            0 when type is MessageBoxDialogType.OkCancel or MessageBoxDialogType.YesNoCancel => MessageBoxButton.Cancel,
            0 when type is MessageBoxDialogType.YesNo => MessageBoxButton.No,
            1 when type is MessageBoxDialogType.Ok => MessageBoxButton.Ok,
            1 when type is MessageBoxDialogType.YesNo or MessageBoxDialogType.YesNoCancel => MessageBoxButton.Yes,
            2 when type is MessageBoxDialogType.YesNoCancel => MessageBoxButton.No,
            _ => throw new ArgumentOutOfRangeException(nameof(nativeResponse), nativeResponse, null)
        };
    }

    public static int ToNativeValue(this MessageBoxButton button, MessageBoxDialogType type)
    {
        return button switch
        {
            MessageBoxButton.Cancel when type is MessageBoxDialogType.OkCancel or MessageBoxDialogType.YesNoCancel => 0,
            MessageBoxButton.No when type is MessageBoxDialogType.YesNo => 1,
            MessageBoxButton.Ok when type is MessageBoxDialogType.Ok => 1,
            MessageBoxButton.Yes when type is MessageBoxDialogType.YesNo or MessageBoxDialogType.YesNoCancel => 1,
            MessageBoxButton.No when type is MessageBoxDialogType.YesNoCancel => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    public static string ToNativeName(this StringVariable variable)
    {
        return variable switch
        {
            StringVariable.Version => "tinyfd_version",
            StringVariable.Needs => "tinyfd_needs",
            StringVariable.Response => "tinyfd_response",
            _ => throw new ArgumentOutOfRangeException(nameof(variable), variable, null)
        };
    }

    public static string ToNativeName(this IntegerVariable variable)
    {
        return variable switch
        {
            IntegerVariable.Verbose => "tinyfd_verbose",
            IntegerVariable.Silent => "tinyfd_silent",
            IntegerVariable.AllowCursesDialogs => "tinyfd_allowCursesDialogs",
            IntegerVariable.ForceConsole => "tinyfd_forceConsole",
            IntegerVariable.AssumeGraphicDisplay => "tinyfd_assumeGraphicDisplay",
            _ => throw new ArgumentOutOfRangeException(nameof(variable), variable, null)
        };
    }

    public static byte[] HexToRgb(string hex)
    {
        hex = hex.Replace("#", "");

        if (!int.TryParse(hex, NumberStyles.AllowHexSpecifier, null, out _))
            throw new ArgumentException(
                "The hex color string must be a valid #RRGGBB string. The alpha channel (#RRGGBBAA) is not supported.",
                nameof(hex));

        var r = byte.Parse(hex[..2], NumberStyles.AllowHexSpecifier);
        var g = byte.Parse(hex[2..4], NumberStyles.AllowHexSpecifier);
        var b = byte.Parse(hex[4..6], NumberStyles.AllowHexSpecifier);
        return new[] { r, g, b };
    }
}