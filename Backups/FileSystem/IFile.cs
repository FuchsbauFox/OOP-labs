namespace Backups.FileSystem
{
    public interface IFile : IStorageObject
    {
        byte[] Read();
        void Write(byte[] content);
    }
}