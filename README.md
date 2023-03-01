# TinyDialogsNet
A C# wrapper around [tinyfiledialog](https://sourceforge.net/projects/tinyfiledialogs).

# Why?
I felt like rewritting the test file provided in the tinyfiledialog repo, so I did just that.

# What does it have?
Currently it has a:
- Beeper
- Input box
- NotifyPopup
- SaveFileDialog
- OpenFileDialog
- SelectFolderDialog
- ColorChooser

# Is it platform dependant?
Yes, but you can still use the AnyCPU configuration.

# Supported OSes
- Windows (x86/x64)
- Linux (x64, not tested)
- OSX (not tested)

# Is there a NuGet package?
[Yes!](https://www.nuget.org/packages/TinyDialogsNet/)

# Examples (Windows 10)

### InputBox
```cs
var input = Dialogs.InputBox(Dialogs.InputBoxType.Default, "Question", "Please enter the login..");
// Change InputBoxType to Password to hide entered characters
```
![image](https://user-images.githubusercontent.com/65343244/222249819-61b1ca52-aa06-422c-8913-c2ed994bca31.png)

### NotifyPopup
```cs
Dialogs.NotifyPopup(Dialogs.PopupIconType.Warning, "Warning!", "You have an unregistered copy of Windows.");
```
![image](https://user-images.githubusercontent.com/65343244/222250752-eac4b85c-364e-4195-bcd2-0135c23b5567.png)

### MessageBox
```cs
var response = Dialogs.MessageBox(Dialogs.MessageBoxButtons.YesNo, Dialogs.MessageBoxIconType.Error,
    Dialogs.MessageBoxDefaultButton.OkYes, "Unhandled exception!", "Would you like to submit a crash report?");
// Example of reading the response:
// if(response == (int)Dialogs.MessageBoxButtons.Ok)) {...}
// Please check Dialogs.cs for more icons/buttons
```
![image](https://user-images.githubusercontent.com/65343244/222251186-8945df44-0490-4035-a02e-1f06d165396d.png)

### ColorChooser
```cs
var hexColor = Dialogs.ColorChooser("Please select a color..", "#FFFFFF");
```

![image](https://user-images.githubusercontent.com/65343244/222257290-03475a9d-fd57-4f6b-979c-62aed6b50586.png)

### About dialogs
Please note that: 
- the filters are optional
- the returned value may be null if the user pressed cancel

### OpenFileDialog

```cs
var files = Dialogs.OpenFileDialog("Please select an image..", "", new[] { "*.jpg" }, "JPEG images", true);
```

![image](https://user-images.githubusercontent.com/65343244/222258241-89b2a6a1-9111-4255-9878-cf163b3ee362.png)

### SaveFileDialog
```cs
var path = Dialogs.SaveFileDialog("Please select where to save the report..", "", "*.log", "Log files");
```

![image](https://user-images.githubusercontent.com/65343244/222258517-3eed87ff-666a-4e75-af4b-5d3534088778.png)

### SelectFolderDialog

```cs
var folder = Dialogs.SelectFolderDialog("Please select the folder with images..");
```

![image](https://user-images.githubusercontent.com/65343244/222258820-18765de3-fff6-475d-a800-2f14d3243ecc.png)
