namespace Direktory.DirectorySize;
public class DirectorySizeHandler
{
    private readonly DirectoryInfo _rootDir;
    private readonly CommaSeparatedList _dexclude;
    private readonly FileInfo _output;
    private readonly IList<DirectorySize> _sizeList = new List<DirectorySize>();

    private int _fileCount = 0;
    private int _directoryCount = 1;

    public DirectorySizeHandler(DirectoryInfo rootDir, CommaSeparatedList dexclude, FileInfo output)
    {
        _rootDir = rootDir;
        _dexclude = dexclude;
        _output = output;
    }

    public void Execute()
    {
        ProcessDirectory(_rootDir, 1);

        _directoryCount -= 1; // Remove root dir

        var streamWriter = _output != null
            ? _output.CreateText()
            : Console.Out;

        Console.SetOut(streamWriter);

        try
        {
            Console.WriteLine($"{_directoryCount} Directories. {_fileCount} Files;");
            var length = default(long);
            var list = _sizeList.OrderByDescending(x => x.Length).ToList();
            foreach(var item in list)
            {
                var text = string.Format("{0,16}", SizeSuffix(item.Length));
                Console.WriteLine($"{text} {item.Name} ({item.Length})");
                length += item.Length;
            }
            Console.WriteLine($"{SizeSuffix(length)} ({length} bytes)");
        }
        finally
        {
            streamWriter.Flush();
            Console.SetOut(streamWriter);
        }
    }

    private long ProcessDirectory(DirectoryInfo rootDir, int depth)
    {
        if (_dexclude.Contains(rootDir.Name)
            || (rootDir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden
            || (rootDir.Attributes & FileAttributes.System) == FileAttributes.System)
        {
            return -1;
        }

        _directoryCount += 1;

        var length = default(long);

        foreach (var sourceFileInfo in rootDir.GetFiles())
        {
            _fileCount += 1;
            length += sourceFileInfo.Length;
        }

        foreach (var directoryInfo in rootDir.GetDirectories())
        {
            var sum = ProcessDirectory(directoryInfo, depth + 1);
            if (sum == -1)
            {
                continue;
            }
            if (depth == 1)
            {
                _sizeList.Add(new DirectorySize { Name = directoryInfo.Name, Length = sum });
            }
            length += sum;
        }
        return length;
    }

    static readonly string[] SizeSuffixes =
                       { "  ", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
    static string SizeSuffix(long value, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0)
        { 
            throw new ArgumentOutOfRangeException("decimalPlaces");
        }
        if (value < 0)
        { 
            return "-" + SizeSuffix(-value, decimalPlaces); 
        }
        if (value == 0) 
        { 
            return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
        }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
        {
            mag += 1;
            adjustedSize /= 1024;
        }

        return string.Format("{0:n" + decimalPlaces + "} {1}",
            adjustedSize,
            SizeSuffixes[mag]);
    }
}
