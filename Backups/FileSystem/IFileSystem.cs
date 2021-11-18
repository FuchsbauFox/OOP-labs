namespace Backups.FileSystem
{
    public interface IFileSystem
    {
        IDirectory GetRoot();
    }
}