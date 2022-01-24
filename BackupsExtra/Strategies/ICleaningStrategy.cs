using System.Collections.Generic;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;

namespace BackupsExtra.Strategies
{
    public interface ICleaningStrategy
    {
        void CleaningPoints(BackupJobExtra backupJobExtra);
        List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra);
    }
}