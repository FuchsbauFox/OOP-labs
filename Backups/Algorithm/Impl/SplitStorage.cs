using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;

namespace Backups.Algorithm.Impl
{
    public class SplitStorage : IAlgorithmStorage
    {
        public IRestorePoint CreateRestorePoint(IFsAdapter adapter, string restorePointName, List<string> jobObjects)
        {
            int counter = 0;
            foreach (string jobObject in jobObjects)
            {
                counter++;
                var listObject = new List<string>() { jobObject };
                adapter.CreateArchive(restorePointName, "split" + counter.ToString(), listObject, true);
            }

            return new RestorePoint(restorePointName, this, jobObjects);
        }

        public List<string> MergeRestorePoints(IFsAdapter adapter, IRestorePoint oldRestorePoint, IRestorePoint newRestorePoint)
        {
            var newJobObjectList = new List<string>(newRestorePoint.Jobs().ToList());
            if (oldRestorePoint.Algorithm is not SingleStorage)
            {
                int counter = newRestorePoint.Jobs().Count;
                for (int i = 0; i < oldRestorePoint.Jobs().Count; i++)
                {
                    string jobObjectPath = oldRestorePoint.Jobs()[i];
                    if (newRestorePoint.Jobs().Any(jobObject => jobObject == jobObjectPath)) continue;
                    counter++;
                    adapter.MergeArchiveDir(
                        oldRestorePoint.Name,
                        newRestorePoint.Name,
                        "split" + (i + 1),
                        "split" + counter.ToString());

                    newJobObjectList.Add(jobObjectPath);
                }
            }

            adapter.DeleteDirectory("C:\\Backups\\" + oldRestorePoint.Name);
            return newJobObjectList;
        }
    }
}