# TinyDialogsNet

TinyDialogsNet is a C# wrapper around [tinyfiledialogs](https://sourceforge.net/projects/tinyfiledialogs), which is a
C/C++ library for displaying dialogs with a simple interface in mind. This library aims to replicate the same easiness
of use, which includes abstracting any UTF string conversions and Interop stuff away from the user.

## Supported platforms

Since tinyfiledialogs is a C/C++ library, it comes with a set of native library files which are contained in
the `runtimes` folder. Such files are included for the following platforms:

| OS      | x86 | x64 | arm64 |
|---------|:---:|:---:|:-----:|
| Windows |  ✅  |  ✅  |   ❌   |
| Linux   |  ✅  |  ✅  |   ❌   |
| OSX     |  ❌  |  ✅  |   ✅   |

## Installation

You can:

- Build the library from source and include it in your project manually.
    - `git clone https://github.com/nedoxff/TinyDialogsNet`
    - `cd TinyDialogsNet`
    - `dotnet build --configuration Release` (you'll need .NET8 for this)
    - The output will be in `bin/Release/net6.0`, `bin/Release/net7.0` and `bin/Release/net8.0` (don't forget
      the `runtimes` folder!)
- [Install the library from NuGet.](https://www.nuget.org/packages/TinyDialogsNet/)

## Usage

The library provides support for these dialogs:

- Notification popup
    ```csharp
    // NotificationIconType also has Error & Information properties
    TinyDialogs.NotifyPopup(NotificationIconType.Information, "Title", "Message");
    ```
- Message box
    ```csharp
    var response = TinyDialogs.MessageBox("Title", "Message", MessageBoxDialogType.YesNo,  // which buttons to show
                                                              MessageBoxIconType.Question, // which icon to show
                                                              MessageBoxButton.Yes);       // the default button
    // response is a MessageBoxButton which has Ok, Yes, No & Cancel properties
    ```
- Input box
    ```csharp
    // InputBoxType also has the Password property
    var (canceled, text) = TinyDialogs.InputBox(InputBoxType.Text, "Title", "Description", "Placeholder");
    ```
- Save file dialog
    ```csharp
    var filter = new FileFilter("Image files", ["*.jpg", "*.png"]);
    var (canceled, path) = TinyDialogs.SaveFileDialog("Title", "Default path", filter);
    ```
- Open file dialog
    ```csharp
    var filter = new FileFilter("Image files", ["*.jpg", "*.png"]);
    var allowMultipleSelections = true;
    var (canceled, paths) = TinyDialogs.OpenFileDialog("Title", "Default path", allowMultipleSelections, filter);
    ```
- Select folder dialog
    ```csharp
    var (canceled, path) = TinyDialogs.SelectFolderDialog("Title", "Default path");
    ```
- Color chooser dialog
    ```csharp
    var (canceled, color) = TinyDialogs.ColorChooser("Title", "Default color (#RRGGBB)");
    ```
- Beeper
    ```csharp
    TinyDialogs.Beep();
    ```

One can also get and set native properties of the library:

```csharp
var version = TinyDialogs.GetGlobalStringVariable(StringVariable.Version);
Console.WriteLine($"Using tinyfiledialogs v{version}");

// "0" if false, "1" if true
var verbose = TinyDialogs.GetGlobalIntegerVariable(IntegerVariable.Verbose);
TinyDialogs.SetGlobalIntegerVariable(IntegerVariable.Silent, 1);
```

*Additional documentation can be found in the XML documentation
of [TinyDialogs.cs](https://github.com/nedoxff/TinyDialogsNet/blob/master/TinyDialogs.cs).*

## Contributing

If you find a bug or have a suggestion on how the library can be improved, [please create an issue](https://github.com/nedoxff/TinyDialogsNet/issues)! I'll try to check
them regularly.

## Thank you

- [vareille](https://github.com/vareille), who
  made [the original tinyfiledialogs library](https://sourceforge.net/projects/tinyfiledialogs/) and whose C# examples
  file was the main inspiration and reference for this project.

## License

TinyDialogsNet is distributed under the MIT license. The original tinyfiledialogs library is distributed under the zlib
license.