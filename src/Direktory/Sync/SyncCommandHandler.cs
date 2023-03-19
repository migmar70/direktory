namespace Direktory.Sync;

public class SyncCommandHandler
{
    private readonly DirectoryInfo _sourceDir;
    private readonly DirectoryInfo _targetDir;
    private readonly CommaSeparatedList _dexclude;

    private readonly IList<DirectoryAndFiles> _tree = new List<DirectoryAndFiles>();

    private int _fileCount = 0;

    public SyncCommandHandler(DirectoryInfo sourceDir, DirectoryInfo targetDir, CommaSeparatedList dexclude)
    {
        _sourceDir = sourceDir;
        _targetDir = targetDir;
        _dexclude = dexclude;
    }

    public void Execute()
    {
        ProcessDirectory(_sourceDir, _targetDir);

        foreach (var node in _tree)
        {
            node.Synchronize();
            //break;
        }
        Console.WriteLine($"{_tree.Count} Directories. {_fileCount} Files;");
    }

    private void ProcessDirectory(DirectoryInfo sourceDir, DirectoryInfo targetDir)
    {
        if (_dexclude.Contains(sourceDir.Name))
        {
            return;
        }
        var directoryAndFiles = new DirectoryAndFiles(sourceDir, targetDir);

        _tree.Add(directoryAndFiles);

        foreach (var sourceFileInfo in sourceDir.GetFiles())
        {
            _fileCount += 1;
            directoryAndFiles.Add(sourceFileInfo);
        }

        foreach (var directoryInfo in sourceDir.GetDirectories())
        {
            ProcessDirectory(directoryInfo, new DirectoryInfo(Path.Combine(targetDir.FullName, directoryInfo.Name)));
        }
    }
}
