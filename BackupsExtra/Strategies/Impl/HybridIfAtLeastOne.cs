using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridIfAtLeastOne : CleaningStrategy
    {
        [JsonProperty]
        private List<CleaningStrategy> _strategies;

        public HybridIfAtLeastOne(List<CleaningStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException();
        }

        public override List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            return _strategies.Aggregate(
                new List<IRestorePoint>(),
                (current, strategy) => current
                    .Union(strategy.GetListPointsToRemove(backupJobExtra)).ToList());
        }
    }
}