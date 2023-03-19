namespace Direktory;

public class CommaSeparatedList
{
    private readonly IDictionary<string, string> _patterns = new Dictionary<string, string>();

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
            _patterns.Add(pattern.ToLower(), pattern);
        }
    }

    public bool Contains(string item) => _patterns.ContainsKey(item.ToLower());
}