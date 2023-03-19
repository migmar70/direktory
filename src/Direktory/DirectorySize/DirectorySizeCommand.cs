using System.CommandLine;

namespace Direktory.DirectorySize;

public class DirectorySizeCommand : Command
{
    public DirectorySizeCommand(string name = "dsize", string description = "Calculate directory size.")
    : base(name, description)
    {
        var rootPathArgument = new Argument<DirectoryInfo>(
            name: "root",
            description: "Root path");

        AddArgument(rootPathArgument);

        var dexcludeOption = new Option<string[]>(
            name: "--dexclude",
            description: "Comma-separated list of dictectories to exclude.")
        {
            AllowMultipleArgumentsPerToken = true
        };

        AddOption(dexcludeOption);

        var outputOption = new Option<FileInfo>(
            aliases: new[] { "--output", "-o" },
            description: "Output file.");

        AddOption(outputOption);

        this.SetHandler(
            Handlr,
            rootPathArgument,
            dexcludeOption,
            outputOption);
    }
    static public void Handlr(DirectoryInfo rootPathDir, string[] dexclude, FileInfo output)
    {
        if (rootPathDir.Exists == false)
        {
            throw new DirectoryNotFoundException(rootPathDir.FullName);
        }
        new DirectorySizeHandler(
            rootPathDir,
            new CommaSeparatedList(dexclude),
            output)
            .Execute();
    }
}
