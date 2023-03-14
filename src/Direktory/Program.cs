
using Direktory.Sync;

using System.CommandLine;

namespace Direktory;
public class Program
{
    static void Main(string[] args)
    {
        new RootCommand("Directory tool.")
            .AddCommandEx(new SyncCommand())
            .Invoke(args);
    }
   
}
