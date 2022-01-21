using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;
using Backups.FSAdapter.Impl;
using BackupsExtra.Tools.BackupsExtra;

namespace BackupsExtra.BackupsExtra.Impl
{
    [Serializable]
    public class BackupJobExtra : BackupJob, IBackupExtra
    {
        public BackupJobExtra(IFsAdapter adapter)
            : base(adapter)
        {
        }

        public void MergeRestorePoints(string oldPoint, string newPoint)
        {
            CheckRestorePoint(oldPoint);
            CheckRestorePoint(newPoint);
            IRestorePoint oldRestorePoint = GetRestorePoint(oldPoint);
            IRestorePoint newRestorePoint = GetRestorePoint(newPoint);
            if (oldRestorePoint.Algorithm == "single" || newRestorePoint.Algorithm == "single")
                Adapter.DeleteDirectory("C:\\Backups\\" + oldPoint);

            var newJobObjectList = new List<string>(newRestorePoint.Jobs().ToList());
            int counter = newRestorePoint.Jobs().Count;
            for (int i = 0; i < oldRestorePoint.Jobs().Count; i++)
            {
                string jobObjectPath = oldRestorePoint.Jobs()[i];
                if (newRestorePoint.Jobs().Any(jobObject => jobObject == jobObjectPath)) continue;
                counter++;
                switch (Adapter)
                {
                    case WinFsAdapter _:
                        Adapter.CopyFile(
                            "C:\\Backups\\" + oldPoint + "\\split" + (i + 1),
                            "C:\\Backups\\" + newPoint + "\\split" + counter.ToString());
                        break;
                    case VirtualFsAdapter adapter:
                        adapter.AddDirectory("C:\\TEMP");
                        adapter.ExtractArchive("C:\\Backups\\" + oldPoint + "\\split" + (i + 1), "C:\\TEMP");

                        var jobObject = new List<string>
                        {
                            "C:\\TEMP\\" + jobObjectPath.Remove(0, jobObjectPath.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                        };
                        adapter.CreateArchive(newPoint, "split" + counter.ToString(), jobObject, true);
                        adapter.DeleteDirectory("C:\\TEMP");
                        break;
                }

                newJobObjectList.Add(jobObjectPath);
            }

            Adapter.DeleteDirectory("C:\\Backups\\" + oldPoint);
            RestorePoints.Remove(oldRestorePoint);
            RestorePoints.Add(new RestorePoint(newRestorePoint.Name, newRestorePoint.Algorithm, newJobObjectList));
            RestorePoints.Remove(newRestorePoint);
        }

        public void ToOriginalLocation(string restorePointName)
        {
            CheckRestorePoint(restorePointName);
            IRestorePoint restorePoint = GetRestorePoint(restorePointName);
            Adapter.AddDirectory("C:\\TEMP");
            ExtractArchiveToTempDir(restorePoint);

            foreach (string path in restorePoint.Jobs())
            {
                try
                {
                    Adapter.DeleteFile(path);
                }
                catch (Exception)
                {
                    // ignored
                }

                Adapter.CopyFile(
                    "C:\\TEMP\\" + path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                    path);
            }

            Adapter.DeleteDirectory("C:\\TEMP");
        }

        public void ToDifferentLocation(string restorePointName, string dirPath)
        {
            CheckRestorePoint(restorePointName);
            IRestorePoint restorePoint = GetRestorePoint(restorePointName);
            Adapter.AddDirectory("C:\\TEMP");
            ExtractArchiveToTempDir(restorePoint);

            foreach (string path in restorePoint.Jobs())
            {
                string fileName = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                try
                {
                    Adapter.DeleteFile(dirPath + "\\" + fileName);
                }
                catch (Exception)
                {
                    // ignored
                }

                Adapter.CopyFile(
                    "C:\\TEMP\\" + path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                    dirPath + "\\" + fileName);
            }

            Adapter.DeleteDirectory("C:\\TEMP");
        }

        public void DeleteRestorePoint(string restorePointName)
        {
            CheckRestorePoint(restorePointName);
            IRestorePoint restorePoint = GetRestorePoint(restorePointName);

            Adapter.DeleteDirectory("C:\\Backups\\" + restorePointName);
            RestorePoints.Remove(restorePoint);
        }

        private IRestorePoint GetRestorePoint(string name)
        {
            IRestorePoint findRestorePoint = RestorePoints.FirstOrDefault(restorePoint => restorePoint.Name == name);
            return findRestorePoint ?? throw new RestorePointNotFoundException();
        }

        private void CheckRestorePoint(string name)
        {
            if (RestorePoints.All(restorePoint => restorePoint.Name != name))
            {
                throw new RestorePointNotFoundException();
            }
        }

        private void ExtractArchiveToTempDir(IRestorePoint restorePoint)
        {
            switch (restorePoint.Algorithm)
            {
                case "single":
                    Adapter.ExtractArchive("C:\\Backups\\" + restorePoint.Name + "\\single", "C:\\TEMP");
                    break;
                case "split":
                    for (int i = 0; i < restorePoint.Jobs().Count; i++)
                    {
                        Adapter.ExtractArchive("C:\\Backups\\" + restorePoint.Name + "\\split" + (i + 1), "C:\\TEMP");
                    }

                    break;
            }
        }
    }
}