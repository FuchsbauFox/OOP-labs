using System.Collections.Generic;

namespace Backups.Backups
{
    public interface IBackupJob : IBackup
    {
        IReadOnlyList<string> Objects();
        IReadOnlyList<IRestorePoint> Points();
    }
}