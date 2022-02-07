using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backups.FSAdapter.Impl
{
    public class WinFsAdapter : IFsAdapter
    {
        public void AddDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void DeleteDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        public void AddFile(string path)
        {
            using (File.Create(path))
            {
            }
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void CopyFile(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        public void AddContentOnFile(string path, string content)
        {
            var sw = new StreamWriter(path);
            sw.WriteLine(content);
            sw.Close();
        }

        public void CreateArchive(string dirName, string archiveName, List<string> filePaths, bool dirCanBeExist = false)
        {
            try
            {
                Directory.Delete("C:\\Temp", true);
            }
            catch (Exception)
            {
                // ignored
            }

            string dirPath = "C:\\Temp";
            Directory.CreateDirectory(dirPath);
            foreach (string path in filePaths)
            {
                CopyFile(path, dirPath + "\\" + path[(path.LastIndexOf('\\') + 1) ..]);
            }

            Directory.CreateDirectory("C:\\Backups\\" + dirName);
            ZipFile.CreateFromDirectory(dirPath, "C:\\Backups\\" + dirName + "\\" + archiveName);
            Directory.Delete(dirPath, true);
        }

        public void ExtractArchive(string archiveName, string dirPath)
        {
            Directory.CreateDirectory(dirPath);
            foreach (string file in Directory.GetFiles("C:\\Backups\\" + archiveName))
            {
                ZipFile.ExtractToDirectory(file, dirPath);
            }
        }

        public List<string> ExtractArchiveToTemp(string archiveName)
        {
            try
            {
                Directory.Delete("C:\\Temp", true);
            }
            catch (Exception)
            {
                // ignored
            }

            Directory.CreateDirectory("C:\\Temp");
            foreach (string file in Directory.GetFiles("C:\\Backups\\" + archiveName))
            {
                ZipFile.ExtractToDirectory(file, "C:\\Temp");
            }

            return Directory.GetFiles("C:\\Temp").Select(file =>
                "C:\\Temp\\" + file[(file.LastIndexOf("\\", StringComparison.Ordinal) + 1) ..]).ToList();
        }

        public void DeleteArchive(string archiveName)
        {
            DeleteDirectory("C:\\Backups\\" + archiveName);
        }

        public void MergeArchiveDir(string oldDirName, string newDirName, string oldArchive, string newArchive)
        {
            CopyFile(
                "C:\\Backups\\" + oldDirName + "\\" + oldArchive,
                "C:\\Backups\\" + newDirName + "\\" + newArchive);
        }
    }
}