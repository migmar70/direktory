namespace DirectoryCompare;

public class CommaSeparatedList
{
    public ISet<string> List { get; } = new HashSet<string>();

    public CommaSeparatedList()
    {
    }

    public CommaSeparatedList(string patterns)
    {
        if (string.IsNullOrWhiteSpace(patterns) == false)
        {
            Initialize(patterns.Split(";"));
        }
    }

    public CommaSeparatedList(string[] patterns)
    {
        Initialize(patterns);
    }

    private void Initialize(string[] patterns)
    {
        foreach (string pattern in patterns)
        {
            List.Add(pattern);
        }
    }
}