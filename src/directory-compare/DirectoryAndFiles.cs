using System.Xml.Linq;

namespace DirectoryCompare;

internal class DirectoryAndFiles
{
    public DirectoryInfo SourceDir { get; }
    public DirectoryInfo TargetDir { get; }
    public FileInfo SourceFileInfo { get; }
    public SyncOperation SyncOperation { get; }
    public IList<FileAndOperation> FileAndOperations { get; } = new List<FileAndOperation>();

    public DirectoryAndFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
    {
        SourceDir = sourceDir;
        TargetDir = targetDir;
        SyncOperation = TargetDir.Exists
            ? SyncOperation.None
            : SyncOperation.CreateTarget;
    }

    public void Add(FileInfo sourceFileInfo)
    {
        var fileAndOperation = new FileAndOperation(sourceFileInfo, new FileInfo(Path.Join(TargetDir.FullName, sourceFileInfo.Name)));

        FileAndOperations.Add(fileAndOperation);
    }

    public void Print()
    {
        Console.WriteLine($"D {SyncOperation.GetChar()} {SourceDir.FullName}");
        foreach (var f in FileAndOperations)
        {
            Console.WriteLine($"F {f.SyncOperation.GetChar()} {f.SourceFileInfo.FullName}");
        }
    }

    public void Synchronize()
    {
        if (SyncOperation != SyncOperation.None)
        {
            Console.WriteLine($"D {SyncOperation.GetChar()} {SourceDir.FullName}");
            TargetDir.Create();
            Directory.SetCreationTime(TargetDir.FullName, SourceDir.CreationTime);
        }
        foreach (var fileAndOperation in FileAndOperations)
        {
            fileAndOperation.Synchronize();
        }
    }
}
