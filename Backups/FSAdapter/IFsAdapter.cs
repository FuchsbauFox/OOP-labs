using System.Collections.Generic;

namespace Backups.FSAdapter
{
    public interface IFsAdapter
    {
        void AddDirectory(string path);
        void DeleteDirectory(string path);
        void AddFile(string path);
        void DeleteFile(string path);
        void CopyFile(string sourceFileName, string destFileName);
        void AddContentOnFile(string path, string content);
        void CreateArchive(string dirName, string archiveName, List<string> filePaths, bool dirCanBeExist = false);
        void ExtractArchive(string archivePath, string dirPath);
    }
}