using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Backups.Tools.FileSystemException;
using Newtonsoft.Json;

namespace Backups.FileSystem.Impl
{
    public class Directory : IDirectory
    {
        [JsonProperty]
        private readonly List<IStorageObject> _objects;

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
            CheckExistsObject(obj);
            _objects.Add(obj);
        }

        public void DeleteObject(IStorageObject obj)
        {
            CheckThisDirectoryForDelete(obj.Name);

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

        private void CheckExistsObject(IStorageObject newObject)
        {
            if (_objects.Any(obj => obj.Name == newObject.Name && obj.GetType() == newObject.GetType()))
            {
                throw new ObjectWithThisNameAlreadyExistsException();
            }
        }

        private void CheckThisDirectoryForDelete(string name)
        {
            if (name is "C:" or "Backups")
            {
                throw new ObjectCannotBeDeleteException();
            }
        }
    }
}