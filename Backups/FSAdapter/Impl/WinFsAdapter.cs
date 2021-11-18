namespace Backups.FSAdapter.Impl
{
    public class WinFsAdapter : IFsAdapter
    {
        public void AddDirectory(string path, string name)
        {
        }

        public void DeleteDirectory(string path, string name)
        {
            throw new System.NotImplementedException();
        }

        public void AddFile(string path, string name)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string path, string name)
        {
            throw new System.NotImplementedException();
        }

        public void CopyStorageObject(string path, string name, string newPath)
        {
            throw new System.NotImplementedException();
        }

        public void MoveStorageObject(string prevPath, string name, string newPath)
        {
            throw new System.NotImplementedException();
        }

        public void CreateArchive(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}