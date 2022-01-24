using Backups.Algorithm;

namespace Backups.Backups
{
    public interface IBackup
    {
        void AddJobObject(string path);
        void RemoveJobObject(string path);
        void SetAlgorithmStorage(IAlgorithmStorage algorithm);
        void CreateBackup(string name);
    }
}