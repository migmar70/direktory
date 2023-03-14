using Direktory.Sync;

namespace Direktory;

public class FileAndOperation
{
    public FileInfo SourceFileInfo { get; private set; }
    public FileInfo TargetFileInfo { get; private set; }
    public SyncOperation SyncOperation { get; } = SyncOperation.None;

    public FileAndOperation(FileInfo sourceFileInfo, FileInfo targetFileInfo)
    {
        SourceFileInfo = sourceFileInfo;
        TargetFileInfo = targetFileInfo;
        SyncOperation = GetSyncOperation(sourceFileInfo, targetFileInfo);
    }
    private static SyncOperation GetSyncOperation(FileInfo sourceFileInfo, FileInfo targetFileInfo)
    {
        if (targetFileInfo.Exists == false)
        {
            return SyncOperation.CreateTarget;
        }

        if (targetFileInfo.LastWriteTime < sourceFileInfo.LastWriteTime)
        {
            return SyncOperation.UpdateTarget;
        }

        if (targetFileInfo.LastWriteTime > sourceFileInfo.LastWriteTime)
        {
            return SyncOperation.UpdateSource;
        }
        return SyncOperation.None;
    }

    public void Synchronize()
    {
        if (SyncOperation != SyncOperation.None)
        {
            Console.WriteLine($"F {SyncOperation.GetChar()} {SourceFileInfo.FullName}");
        }
        switch (SyncOperation)
        {
            case SyncOperation.CreateTarget:
                {
                    SourceFileInfo.CopyTo(TargetFileInfo.FullName);
                    File.SetCreationTime(TargetFileInfo.FullName, SourceFileInfo.CreationTime);
                    File.SetLastWriteTime(TargetFileInfo.FullName, SourceFileInfo.LastWriteTime);
                    break;
                }
            case SyncOperation.UpdateTarget:
                {
                    SourceFileInfo.CopyTo(TargetFileInfo.FullName, true);
                    File.SetLastWriteTime(TargetFileInfo.FullName, SourceFileInfo.LastWriteTime);
                    break;
                }
        }

    }
}
