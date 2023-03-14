using System.CommandLine;

namespace Direktory.Sync;

public class SyncCommand : Command
{
    public SyncCommand(string name = "sync", string description = "Directory sync command")
        : base(name, description)
    {
        var sourceDirOption = new Option<DirectoryInfo>(aliases: new[] { "--source", "-s" }, description: "Source directory.");
        var targetDirOption = new Option<DirectoryInfo>(aliases: new[] { "--target", "-t" }, description: "Target directory.");
        var createTargetDirOption = new Option<bool>(aliases: new[] { "--create-target", "-c" }, description: "Create target directory.");
        var dexcludeOption = new Option<string>(name: "--dexclude", description: "Comma-separated list of dictectories to exclude.");

        AddOption(sourceDirOption);
        AddOption(targetDirOption);
        AddOption(createTargetDirOption);
        AddOption(dexcludeOption);

        this.SetHandler(
            Handlr,
            sourceDirOption,
            targetDirOption,
            createTargetDirOption,
            dexcludeOption);
    }
    static public void Handlr(
        DirectoryInfo sourceDir,
        DirectoryInfo targetDir,
        bool createTarget,
        string dexclude)
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
        new SyncCommandHandler(
            sourceDir,
            targetDir,
            new CommaSeparatedList(dexclude))
        .Execute();
    }
}
