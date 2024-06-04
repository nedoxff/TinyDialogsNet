#pragma warning disable CS1591
namespace TinyDialogsNet;

public enum NotificationIconType
{
    Information,
    Warning,
    Error
}

public enum MessageBoxDialogType
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel
}

public enum MessageBoxButton
{
    Cancel,
    No,
    Yes,
    Ok
}

public enum MessageBoxIconType
{
    Information,
    Warning,
    Error,
    Question
}

public enum InputBoxType
{
    Text,
    Password
}

public enum StringVariable
{
    Version,
    Needs,
    Response
}

public enum IntegerVariable
{
    Verbose,
    Silent,
    AllowCursesDialogs,
    ForceConsole,
    AssumeGraphicDisplay
}