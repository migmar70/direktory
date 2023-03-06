
**Windows Command Prompt**
```
C:\> directory-compare.exe --source C:\\Backup\Documents --target D:\\Documents
C:\> directory-compare.exe --source E:\Documents --target C:\Users\migue\Documents
C:\> directory-compare.exe --source C:\Users\migue\Documents --target E:\Documents --dexclude "IISExpress;My Web Sites;Visual Studio 2017;Visual Studio 2019;Visual Studio 2022;My Music;My Pictures;My Videos"
```

**Git Bash**
```
$ ./directory-compare.exe --source /C/Backup/Documents --target /D/Documents
```