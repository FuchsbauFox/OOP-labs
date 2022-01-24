using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;
using BackupsExtra.Tools.BackupsExtra;

namespace BackupsExtra.BackupsExtra.Impl
{
    public class BackupJobExtra : BackupJob, IBackupExtra
    {
        public BackupJobExtra(IFsAdapter adapter)
            : base(adapter)
        {
        }

        public void MergeRestorePoints(string oldPoint, string newPoint)
        {
            IRestorePoint oldRestorePoint = GetRestorePoint(oldPoint);
            IRestorePoint newRestorePoint = GetRestorePoint(newPoint);

            RestorePoints.Remove(oldRestorePoint);
            RestorePoints.Add(new RestorePoint(
                newRestorePoint.Name,
                newRestorePoint.Algorithm,
                newRestorePoint.Algorithm.MergeRestorePoints(Adapter, oldRestorePoint, newRestorePoint)));
            RestorePoints.Remove(newRestorePoint);
        }

        public void ToOriginalLocation(string restorePointName)
        {
            IRestorePoint restorePoint = GetRestorePoint(restorePointName);
            List<string> tempPaths = Adapter.ExtractArchiveToTemp(restorePointName);

            foreach (string path in restorePoint.Jobs())
            {
                string fileName = path[(path.LastIndexOf("\\", StringComparison.Ordinal) + 1) ..];
                try
                {
                    Adapter.DeleteFile(path);
                }
                catch (Exception)
                {
                    // ignored
                }

                Adapter.CopyFile(
                    tempPaths.FirstOrDefault(temp =>
                        temp[(temp.LastIndexOf("\\", StringComparison.Ordinal) + 1) ..] == fileName), path);
            }
        }

        public void ToDifferentLocation(string restorePointName, string dirPath)
        {
            CheckRestorePoint(restorePointName);
            List<string> tempPaths = Adapter.ExtractArchiveToTemp(restorePointName);

            foreach (string path in tempPaths)
            {
                string fileName = path[(path.LastIndexOf("\\", StringComparison.Ordinal) + 1) ..];
                try
                {
                    Adapter.DeleteFile(dirPath + "\\" + fileName);
                }
                catch (Exception)
                {
                    // ignored
                }

                Adapter.CopyFile(path, dirPath + "\\" + fileName);
            }
        }

        public void DeleteRestorePoint(string restorePointName)
        {
            IRestorePoint restorePoint = GetRestorePoint(restorePointName);

            Adapter.DeleteArchive(restorePointName);
            RestorePoints.Remove(restorePoint);
        }

        private IRestorePoint GetRestorePoint(string name)
        {
            CheckRestorePoint(name);
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
    }
}