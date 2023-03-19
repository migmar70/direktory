namespace Direktory.FindDuplicates;

public class FileName
{
    public string Name { get; set; }
    public ISet<string> Extensions { get; } = new HashSet<string>();
    public ISet<string> Names { get; } = new HashSet<string>();
    public IList<FileInfo> Paths { get; } = new List<FileInfo>();

    public FileName(string name)
    {
        Name = name;
    }
    public void Add(FileInfo fileInfo)
    {
        Paths.Add(fileInfo);
        Extensions.Add(fileInfo.Extension);
        Names.Add(fileInfo.Name);
    }
}
