namespace Backups.FSAdapter
{
    public interface IFsAdapter
    {
        void AddDirectory(string path, string name);
        void DeleteDirectory(string path, string name);
        void AddFile(string path, string name);
        void DeleteFile(string path, string name);
        void CopyStorageObject(string path, string name, string newPath);
        void MoveStorageObject(string prevPath, string name, string newPath);
        void CreateArchive(string name);
    }
}