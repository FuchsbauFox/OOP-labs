using System.Collections.Generic;

namespace Backups.Backups
{
    public interface IBackupJob
    {
        IReadOnlyList<string> JobObjects();
        IReadOnlyList<IRestorePoint> RestorePoints();
        void AddJobObject(string path);
        void RemoveJobObject(string path);
        void SetAlgorithmStorage(string algorithm);
        void CreateBackup(string name);
    }
}