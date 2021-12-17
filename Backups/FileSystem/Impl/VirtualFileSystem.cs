namespace Backups.FileSystem.Impl
{
    public class VirtualFileSystem : IFileSystem
    {
        private readonly Directory _root;

        public VirtualFileSystem()
        {
            _root = new Directory("C:", true);
            _root.AddObject(new Directory("Backups"));
        }

        public IDirectory GetRoot()
        {
            return _root;
        }
    }
}