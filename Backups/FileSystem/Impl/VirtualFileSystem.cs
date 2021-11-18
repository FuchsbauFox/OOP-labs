namespace Backups.FileSystem.Impl
{
    public class VirtualFileSystem : IFileSystem
    {
        private Directory _root;

        public VirtualFileSystem()
        {
            _root = new Directory("C:", true);
        }

        public IDirectory GetRoot()
        {
            return _root;
        }
    }
}