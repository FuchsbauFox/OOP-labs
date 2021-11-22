using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Backups.Tools.FileSystemException;

namespace Backups.FileSystem.Impl
{
    public class Archive : IArchive
    {
        private readonly List<IFile> _objects;

        public Archive(string name)
        {
            CheckName(name);

            Name = name;
            _objects = new List<IFile>();
        }

        public string Name { get; }

        public IReadOnlyList<IFile> Objects() => _objects;

        public void AddObject(IFile obj)
        {
            CheckExistsObject(obj.Name);
            _objects.Add(obj);
        }

        public void DeleteObject(IFile obj)
        {
            _objects.Remove(obj);
        }

        private void CheckName(string name)
        {
            if (Regex.IsMatch(name, @"[\/\\\:\*\?\<\>\|]"))
            {
                throw new InvalidFileOrDirNameException();
            }
        }

        private void CheckExistsObject(string name)
        {
            if (_objects.Any(obj => obj.Name == name))
            {
                throw new ObjectWithThisNameAlreadyExistsException();
            }
        }
    }
}