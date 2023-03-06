namespace DirectoryCompare;

public enum SyncOperation
{
    None = 0,
    CreateTarget = 1,
    UpdateTarget = 2,
    UpdateSource = 3,
}

public static class SyncOperationExtensions
{
    public static char GetChar(this SyncOperation operation)
    {
        switch (operation)
        {
            case SyncOperation.None: return '=';
            case SyncOperation.CreateTarget: return '+';
            case SyncOperation.UpdateTarget: return '>';
            case SyncOperation.UpdateSource: return '<';
            default: return '?';
        }
    }
}
