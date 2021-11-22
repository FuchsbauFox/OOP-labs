using System.Collections.Generic;

namespace Backups.FileSystem
{
    public interface IArchive : IStorageObject
    {
        IReadOnlyList<IFile> Objects();
        void AddObject(IFile obj);
        void DeleteObject(IFile obj);
    }
}