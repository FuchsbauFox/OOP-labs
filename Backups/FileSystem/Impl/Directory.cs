using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Backups.Tools.FileSystemException;

namespace Backups.FileSystem.Impl
{
    public class Directory : IDirectory
    {
        private List<IStorageObject> _objects;

        public Directory(string name, bool isRoot = false)
        {
            CheckName(name, isRoot);

            Name = name;
            _objects = new List<IStorageObject>();
        }

        public string Name { get; }

        public IReadOnlyList<IStorageObject> Objects() => _objects;

        public void AddObject(IStorageObject obj)
        {
            CheckExistsObject(obj.Name);
            _objects.Add(obj);
        }

        public void DeleteObject(IStorageObject obj)
        {
            _objects.Remove(obj);
        }

        private void CheckName(string name, bool isRoot)
        {
            var rgx = new Regex(isRoot ? @"[\/\\\*\?\<\>\|]" : @"[\/\\\:\*\?\<\>\|]");
            if (rgx.IsMatch(name))
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