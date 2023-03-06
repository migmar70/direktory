
using System.CommandLine;

namespace DirectoryCompare;
public class Program
{
    static void Main(string[] args)
    {
        var sourceDirOption = new Option<DirectoryInfo>(aliases: new[] { "--source", "-s" }, description: "Source directory.");
        var targetDirOption = new Option<DirectoryInfo>(aliases: new[] { "--target", "-t" }, description: "Target directory.");
        var createTargetDirOption = new Option<bool>(aliases: new[] { "--create-target", "-c" }, description: "Create target directory.");
        var excludeDirOption = new Option<SearchPatterns>(name: "--dexclude", description: "Comma-separated list of dictectories to exclude.");

        var rootCommand = new RootCommand("Compare two directories.");
        rootCommand.AddOption(sourceDirOption);
        rootCommand.AddOption(targetDirOption);
        rootCommand.AddOption(createTargetDirOption);
        rootCommand.AddOption(excludeDirOption);
        rootCommand.SetHandler(
            Handler, 
            sourceDirOption, 
            targetDirOption, 
            createTargetDirOption,
            excludeDirOption);
        rootCommand.Invoke(args);
    }

    static void Handler(
        DirectoryInfo sourceDir, 
        DirectoryInfo targetDir, 
        bool createTarget,
        SearchPatterns excludeDir)
    {
        if (sourceDir.Exists == false)
        {
            throw new DirectoryNotFoundException(sourceDir.FullName);
        }
        if (targetDir.Exists == false)
        {
            if (createTarget == false)
            {
                throw new DirectoryNotFoundException(targetDir.FullName);
            }
            Directory.CreateDirectory(targetDir.FullName);
        }
        var program = new Program(
            sourceDir, 
            targetDir, 
            excludeDir);
        program.Run();
    }

    private readonly DirectoryInfo _sourceDir;
    private readonly DirectoryInfo _targetDir;
    private readonly SearchPatterns _excludeDir;

    private readonly IList<DirectoryAndFiles> _tree = new List<DirectoryAndFiles>();

    private int _fileCount = 0;

    public Program(
        DirectoryInfo sourceDir, 
        DirectoryInfo targetDir, 
        SearchPatterns excludeDir)
    {
        _sourceDir = sourceDir;
        _targetDir = targetDir;
        _excludeDir = excludeDir;
    }

    public void Run()
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
        var directoryAndFiles = new DirectoryAndFiles(sourceDir, targetDir);

        _tree.Add(directoryAndFiles);

        foreach(var sourceFileInfo in sourceDir.GetFiles())
        {
            _fileCount += 1;
            directoryAndFiles.Add(sourceFileInfo);
        }

        foreach (var directoryInfo in sourceDir.GetDirectories())
        {
            if (directoryInfo.Name == "node_modules")
            {
                continue;
            }
            ProcessDirectory(directoryInfo, new DirectoryInfo(Path.Combine(targetDir.FullName, directoryInfo.Name)));
        }
    }

    
}
