namespace DirectoryCompare;

public class SearchPatterns
{
    public string[] Patterns { get; private set; } = new[] { "*.*" };

    public SearchPatterns()
    {
    }

    public SearchPatterns(string patterns)
    {
        Initialize(patterns.Split(";"));
    }

    public SearchPatterns(string[] patterns)
    {
        Initialize(patterns);
    }

    private void Initialize(string[] patterns)
    {
        Patterns = patterns;
    }
}