using System.IO;
using System.Reflection;

namespace Direktory.FindDuplicates;

public class FindDuplicatesHandler
{
    private readonly DirectoryInfo _rootDir;
    private readonly CommaSeparatedList _dexclude;
    private readonly IDictionary<string, FileName> _files = new Dictionary<string, FileName>();
    private readonly FileInfo _output;

    private int _fileCount = 0;
    private int _directoryCount = 1;

    public FindDuplicatesHandler(DirectoryInfo rootDir, CommaSeparatedList dexclude, FileInfo output)
    {
        _rootDir = rootDir;
        _dexclude = dexclude;
        _output = output;
    }

    public void Execute()
    {
        ProcessDirectory(_rootDir);

        _directoryCount -= 1; // Remove root dir

        var streamWriter = _output != null
            ? _output.CreateText()
            : Console.Out;

        Console.SetOut(streamWriter);
        
        try
        {
            Console.WriteLine($"{_directoryCount} Directories. {_fileCount} Files;");

            var files = _files.Values
                .Where(x => x.Paths.Count > 1)
                .Select(y => y)
                .ToList();
            foreach (var file in files)
            {
                foreach (var path in file.Paths)
                {
                    Console.WriteLine(path.FullName);
                }
            }

            Console.WriteLine($"{files.Count} Duplicated Files;");
        }
        finally
        {
            streamWriter.Flush();
            Console.SetOut(streamWriter);
        }
    }

    private void ProcessDirectory(DirectoryInfo rootDir)
    {
        if (_dexclude.Contains(rootDir.Name)
            || (rootDir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden
            || (rootDir.Attributes & FileAttributes.System) == FileAttributes.System)
        {
            return;
        }

        _directoryCount += 1;

        foreach (var sourceFileInfo in rootDir.GetFiles())
        {
            _fileCount += 1;

            var name = sourceFileInfo.Name.ToLower();
            if (string.IsNullOrWhiteSpace(sourceFileInfo.Extension) == false)
            {
                var index = sourceFileInfo.Name.LastIndexOf(sourceFileInfo.Extension);
                name = sourceFileInfo.Name.Substring(0, index).ToLower();
            }

            if (_files.ContainsKey(name) == false)
            {
                _files[name] = new FileName(name);
            }
            _files[name].Add(sourceFileInfo);
        }

        foreach (var directoryInfo in rootDir.GetDirectories())
        {
            ProcessDirectory(directoryInfo);
        }
    }
}
