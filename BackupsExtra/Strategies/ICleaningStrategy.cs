using BackupsExtra.BackupsExtra.Impl;

namespace BackupsExtra.Strategies
{
    public interface ICleaningStrategy
    {
        void SetBackupJobExtra(BackupJobExtra backupJobExtra);
        void CheckingAndCleaningPoints();
    }
}