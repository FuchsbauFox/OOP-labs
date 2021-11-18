using System.Collections.Generic;

namespace Backups.FileSystem
{
    public interface IDirectory : IStorageObject
    {
        IReadOnlyList<IStorageObject> Objects();
        void AddObject(IStorageObject obj);
        void DeleteObject(IStorageObject obj);
    }
}