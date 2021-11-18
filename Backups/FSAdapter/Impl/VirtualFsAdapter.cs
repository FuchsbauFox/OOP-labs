using System;
using System.Linq;
using System.Text.RegularExpressions;
using Backups.FileSystem;
using Backups.FileSystem.Impl;
using Backups.Tools.FSAdapterException;

namespace Backups.FSAdapter.Impl
{
    public class VirtualFsAdapter : IFsAdapter
    {
        private VirtualFileSystem _fileSystem;

        public VirtualFsAdapter()
        {
            _fileSystem = new VirtualFileSystem();
        }

        public void AddDirectory(string path, string name)
        {
            CheckPath(path);

            IDirectory parentDirectory = GetParentDirectory(path);
            parentDirectory.AddObject(new Directory(name));
        }

        public void DeleteDirectory(string path, string name)
        {
            CheckPath(path);

            IDirectory parentDirectory = GetParentDirectory(path);
            var directory = (IDirectory)parentDirectory.Objects().FirstOrDefault(obj =>
                obj.Name == name && obj.GetType() == typeof(Directory));

            if (directory == null)
            {
                throw new StorageObjectNotFoundException();
            }

            parentDirectory.DeleteObject(directory);
        }

        public void AddFile(string path, string name)
        {
            CheckPath(path);

            IDirectory parentDirectory = GetParentDirectory(path);
            parentDirectory.AddObject(new File(name));
        }

        public void DeleteFile(string path, string name)
        {
            CheckPath(path);

            IDirectory parentDirectory = GetParentDirectory(path);
            var file = (IFile)parentDirectory.Objects().FirstOrDefault(obj =>
                obj.Name == path && obj.GetType() == typeof(File));

            if (file == null)
            {
                throw new StorageObjectNotFoundException();
            }

            parentDirectory.DeleteObject(file);
        }

        public void CopyStorageObject(string path, string name, string newPath)
        {
            IDirectory parentDirectory = GetParentDirectory(path);

            IStorageObject obj = parentDirectory.Objects().FirstOrDefault(obj =>
                obj.Name == path);

            if (obj == null)
            {
                throw new StorageObjectNotFoundException();
            }

            IDirectory newDirectory = GetParentDirectory(newPath);
            newDirectory.AddObject(obj);
        }

        public void MoveStorageObject(string prevPath, string name, string newPath)
        {
            CopyStorageObject(prevPath, name, newPath);
            IDirectory prevDirectory = GetParentDirectory(prevPath);

            IStorageObject obj = prevDirectory.Objects().FirstOrDefault(obj =>
                obj.Name == prevPath);

            prevDirectory.DeleteObject(obj);
        }

        public void CreateArchive(string name)
        {
            throw new System.NotImplementedException();
        }

        private void CheckPath(string path, bool thisMustByDirectory = true)
        {
            Regex rgx = thisMustByDirectory ? new Regex(@"^C:[\\]+\\$") : new Regex(@"^C:\\+");

            if (!rgx.IsMatch(path))
            {
                throw new InvalidPathException();
            }
        }

        private IFile GetFile(string path)
        {
            IDirectory directory = GetParentDirectory(path);
            path = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            var file = (IFile)directory.Objects().FirstOrDefault(obj =>
                obj.Name == path && obj.GetType() == typeof(File));

            if (file == null)
            {
                throw new StorageObjectNotFoundException();
            }

            return file;
        }

        private IDirectory GetParentDirectory(string path)
        {
            IDirectory currentDir = _fileSystem.GetRoot();
            path = path.Replace("C:\\", string.Empty);

            while (Regex.IsMatch(path, @"[\\]"))
            {
                string dirName = path[..path.IndexOf('\\')];
                path = path.Remove(0, path.IndexOf("\\", StringComparison.Ordinal) + 1);

                currentDir = (IDirectory)currentDir.Objects().FirstOrDefault(obj =>
                    obj.Name == dirName && obj.GetType() == typeof(Directory));

                if (currentDir == null)
                {
                    throw new StorageObjectNotFoundException();
                }
            }

            return currentDir;
        }
    }
}