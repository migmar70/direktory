using System.CommandLine;

namespace Direktory;

public static class Extensions
{
    public static RootCommand AddCommandEx(this RootCommand rootCommand, Command command)
    {
        rootCommand.Add(command);
        return rootCommand;
    }
}
