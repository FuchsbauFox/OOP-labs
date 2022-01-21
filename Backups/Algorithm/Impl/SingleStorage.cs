using System.Collections.Generic;
using Backups.Backups;
using Backups.Backups.Impl;
using Backups.FSAdapter;
using Newtonsoft.Json;

namespace Backups.Algorithm.Impl
{
    public class SingleStorage : IAlgorithmStorage
    {
        [JsonProperty]
        private readonly IFsAdapter _adapter;
        public SingleStorage(IFsAdapter adapter)
        {
            _adapter = adapter;
        }

        public IRestorePoint CreateRestorePoint(string restorePointName, List<string> jobObjects)
        {
            _adapter.CreateArchive(restorePointName, "single", jobObjects);

            return new RestorePoint(restorePointName, "single", jobObjects);
        }
    }
}