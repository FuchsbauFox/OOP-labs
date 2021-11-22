using System.Collections.Generic;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;

namespace Backups.Algorithm.Impl
{
    public class SplitStorage : IAlgorithmStorage
    {
        private readonly IFsAdapter _adapter;
        public SplitStorage(IFsAdapter adapter)
        {
            _adapter = adapter;
        }

        public IRestorePoint CreateRestorePoint(string restorePointName, List<string> jobObjects)
        {
            int counter = 0;
            foreach (string jobObject in jobObjects)
            {
                counter++;
                var listObject = new List<string>() { jobObject };
                _adapter.CreateArchive(restorePointName, "split" + counter.ToString(), listObject, true);
            }

            return new RestorePoint(restorePointName, "split", jobObjects);
        }
    }
}