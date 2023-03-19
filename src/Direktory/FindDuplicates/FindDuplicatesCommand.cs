using Direktory.Sync;
using System.CommandLine;

namespace Direktory.FindDuplicates;
public class FindDuplicatesCommand : Command
{
    public FindDuplicatesCommand(string name = "findups", string description = "Find duplicate filenames.")
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
        new FindDuplicatesHandler(
            rootPathDir, 
            new CommaSeparatedList(dexclude),
            output)
            .Execute();
    }
}
