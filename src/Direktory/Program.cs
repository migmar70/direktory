
using Direktory.DirectorySize;
using Direktory.FindDuplicates;
using Direktory.Sync;

using System.CommandLine;

namespace Direktory;
public class Program
{
    static void Main(string[] args)
    {
        new RootCommand("Directory tool.")
            .AddCommandEx(new SyncCommand())
            .AddCommandEx(new FindDuplicatesCommand())
            .AddCommandEx(new DirectorySizeCommand())
            .Invoke(args);
    }
   
}
