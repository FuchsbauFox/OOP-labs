namespace BackupsExtra.BackupsExtra
{
    public interface IBackupExtra
    {
        void MergeRestorePoints(string oldPoint, string newPoint);
        void ToOriginalLocation(string restorePointName);
        void ToDifferentLocation(string restorePointName, string dirPath);
        void DeleteRestorePoint(string restorePointName);
    }
}