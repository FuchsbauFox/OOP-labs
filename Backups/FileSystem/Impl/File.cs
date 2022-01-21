using System.Text.RegularExpressions;
using Backups.Tools.FileSystemException;
using Newtonsoft.Json;

namespace Backups.FileSystem.Impl
{
    public class File : IFile
    {
        [JsonProperty]
        private byte[] _content;

        public File(string name)
        {
            CheckName(name);

            Name = name;
            _content = new byte[] { };
        }

        public string Name { get; }

        public byte[] Read()
        {
            return _content;
        }

        public void Write(byte[] content)
        {
            _content = content;
        }

        private void CheckName(string name)
        {
            if (Regex.IsMatch(name, @"[\/\\\:\*\?\<\>\|]"))
            {
                throw new InvalidFileOrDirNameException();
            }
        }
    }
}