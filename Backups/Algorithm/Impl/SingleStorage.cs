using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;

namespace Backups.Algorithm.Impl
{
    public class SingleStorage : IAlgorithmStorage
    {
        public IRestorePoint CreateRestorePoint(IFsAdapter adapter, string restorePointName, List<string> jobObjects)
        {
            adapter.CreateArchive(restorePointName, "single", jobObjects);

            return new RestorePoint(restorePointName, this, jobObjects);
        }

        public List<string> MergeRestorePoints(IFsAdapter adapter, IRestorePoint oldRestorePoint, IRestorePoint newRestorePoint)
        {
            adapter.DeleteDirectory("C:\\Backups\\" + oldRestorePoint.Name);
            return new List<string>(newRestorePoint.Jobs().ToList());
        }
    }
}