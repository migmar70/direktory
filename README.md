
**Windows Command Prompt**
```
C:\> direktory.exe sync --source C:\Backup\Documents --target D:\Documents
C:\> direktory.exe sync --source E:\Documents --target C:\Users\migue\Documents
C:\> direktory.exe sync --source C:\Users\migue\Documents --target E:\Documents --dexclude "IISExpress;My Web Sites;Visual Studio 2017;Visual Studio 2019;Visual Studio 2022;My Music;My Pictures;My Videos"
C:\> direktory.exe sync --source C:\Users\Migue\Documents --target C:\Backup\Documents --dexclude node_modules bin --create-target
C:\> direktory.exe findups "C:\Users\Migue\Documents\My Music"


C:\github\mmartinez0\direktory\src\publish\direktory.exe findups "C:\Users\Migue\Documents" --dexclude var "miguelmartinez.com" "whatdev" "Ameriprise Financial" --output "C:\temp\duplicated-files.log"

C:\github\mmartinez0\direktory\src\publish\direktory.exe sync --source E:\bitbucket\migmar --target C:\bitbucket\migmar --dexclude node_modules


dsize "C:\Users\Migue\Documents" --output "C:\temp\dsize.log"
```

**Git Bash**
```
$ ./direktory.exe sync --source /C/Backup/Documents --target /D/Documents
$ ./direktory.exe findups /c/Users/migue/Documents --dexclude "My Music" "My Pictures" "My Videos"
```