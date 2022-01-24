using System.Collections.Generic;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using BackupsExtra.Tools.BackupsExtra;

namespace BackupsExtra.Strategies
{
    public abstract class CleaningStrategy
    {
        public void CleaningPoints(BackupJobExtra backupJobExtra)
        {
            List<IRestorePoint> pointsToRemove = GetListPointsToRemove(backupJobExtra);

            if (pointsToRemove.Count >= backupJobExtra.Points().Count)
                throw new AllRestorePointsNotPassedLimitsException();

            foreach (IRestorePoint point in pointsToRemove)
                backupJobExtra.DeleteRestorePoint(point.Name);
        }

        public abstract List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra);
    }
}