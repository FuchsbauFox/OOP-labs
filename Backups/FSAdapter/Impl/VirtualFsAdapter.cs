using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Backups.FileSystem;
using Backups.FileSystem.Impl;
using Backups.Tools.FSAdapterException;
using Newtonsoft.Json;

namespace Backups.FSAdapter.Impl
{
    public class VirtualFsAdapter : IFsAdapter
    {
        [JsonProperty]
        private readonly VirtualFileSystem _fileSystem;

        public VirtualFsAdapter()
        {
            _fileSystem = new VirtualFileSystem();
        }

        public void AddDirectory(string path)
        {
            CheckPath(path);

            IDirectory parentDir = GetParentDir(path);
            parentDir.AddObject(new Directory(path[(path.LastIndexOf('\\') + 1) ..]));
        }

        public void DeleteDirectory(string path)
        {
            CheckPath(path);

            IDirectory parentDir = GetParentDir(path);
            parentDir.DeleteObject(GetDirectory(path));
        }

        public void AddFile(string path)
        {
            CheckPath(path);

            IDirectory parentDir = GetParentDir(path);
            parentDir.AddObject(new File(path[(path.LastIndexOf('\\') + 1) ..]));
        }

        public void DeleteFile(string path)
        {
            CheckPath(path);

            IDirectory parentDir = GetParentDir(path);
            parentDir.DeleteObject(GetFile(path));
        }

        public void CopyFile(string sourceFileName, string destFileName)
        {
            CheckPath(sourceFileName);
            CheckPath(destFileName);

            IFile sourceFile = GetFile(sourceFileName);
            IDirectory parentDir = GetParentDir(destFileName);
            IFile destFile = new File(destFileName[(destFileName.LastIndexOf('\\') + 1) ..]);

            parentDir.AddObject(destFile);
            destFile.Write(sourceFile.Read());
        }

        public void AddContentOnFile(string path, string content)
        {
            CheckPath(path);

            IFile file = GetFile(path);
            file.Write(Encoding.UTF8.GetBytes(content));
        }

        public void CreateArchive(string dirName, string archiveName, List<string> filePaths, bool dirCanBeExist = false)
        {
            IDirectory backupsDir = GetDirectory(_fileSystem.GetRoot().Name + "\\Backups");
            IDirectory backup = GetOrCreateDirectory(backupsDir, dirName, dirCanBeExist);
            IArchive archive = new Archive(archiveName);
            backup.AddObject(archive);

            foreach (string path in filePaths)
            {
                CheckPath(path);

                IFile file = GetFile(path);
                IFile archiveFile = new File(path[(path.LastIndexOf('\\') + 1) ..]);
                archiveFile.Write(file.Read());
                archive.AddObject(archiveFile);
            }
        }

        public void ExtractArchive(string archivePath, string dirPath)
        {
            CheckPath(archivePath);
            CheckPath(dirPath);

            IArchive archive = GetArchive(archivePath);
            IDirectory directory = GetDirectory(dirPath);

            foreach (IFile file in archive.Objects())
            {
                directory.AddObject(file);
            }
        }

        private void CheckPath(string path)
        {
            if (!Regex.IsMatch(path, @"^C:\\+"))
            {
                throw new InvalidPathException();
            }
        }

        private IFile GetFile(string path)
        {
            IDirectory parentDir = GetParentDir(path);
            path = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            if (parentDir.Objects().FirstOrDefault(obj =>
                obj.Name == path && obj is IFile) is not IFile file)
            {
                throw new StorageObjectNotFoundException();
            }

            return file;
        }

        private IDirectory GetDirectory(string path)
        {
            IDirectory parentDir = GetParentDir(path);
            path = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            if (parentDir.Objects().FirstOrDefault(obj =>
                obj.Name == path && obj is IDirectory) is not IDirectory directory)
            {
                throw new StorageObjectNotFoundException();
            }

            return directory;
        }

        private IArchive GetArchive(string path)
        {
            IDirectory parentDir = GetParentDir(path);
            path = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            if (parentDir.Objects().FirstOrDefault(obj =>
                obj.Name == path && obj is IArchive) is not IArchive archive)
            {
                throw new StorageObjectNotFoundException();
            }

            return archive;
        }

        private IDirectory GetParentDir(string path)
        {
            IDirectory parentDir = _fileSystem.GetRoot();
            path = path.Remove(0, path.IndexOf("\\", StringComparison.Ordinal) + 1);

            while (Regex.IsMatch(path, @"[\\]"))
            {
                string dirName = path[..path.IndexOf('\\')];
                path = path.Remove(0, path.IndexOf("\\", StringComparison.Ordinal) + 1);

                parentDir = parentDir.Objects().FirstOrDefault(obj =>
                    obj.Name == dirName && obj is IDirectory) as IDirectory;

                if (parentDir == null)
                {
                    throw new StorageObjectNotFoundException();
                }
            }

            return parentDir;
        }

        private IDirectory GetOrCreateDirectory(IDirectory parentDir, string name, bool dirCanBeExist)
        {
            IDirectory directory = null;
            if (dirCanBeExist)
            {
                directory = parentDir.Objects().FirstOrDefault(obj =>
                    obj.Name == name && obj is IDirectory) as IDirectory;
            }

            if (directory != null) return directory;

            directory = new Directory(name);
            parentDir.AddObject(directory);

            return directory;
        }
    }
}