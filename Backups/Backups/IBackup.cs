namespace Backups.Backups
{
    public interface IBackup
    {
        void AddJobObject(string path);
        void RemoveJobObject(string path);
        void SetAlgorithmStorage(string algorithm);
        void CreateBackup(string name);
    }
}