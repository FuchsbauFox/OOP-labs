using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridIfAll : CleaningStrategy
    {
        [JsonProperty]
        private List<CleaningStrategy> _strategies;

        public HybridIfAll(List<CleaningStrategy> strategies)
        {
            _strategies = strategies ?? throw new ArgumentNullException();
        }

        public override List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            List<IRestorePoint> pointsToRemove = _strategies.First().GetListPointsToRemove(backupJobExtra);
            foreach (List<IRestorePoint> temp in _strategies
                .Select(strategy => pointsToRemove
                    .Where(pointToRemove => strategy.GetListPointsToRemove(backupJobExtra)
                        .Any(point => point == pointToRemove)).ToList()))
            {
                pointsToRemove = temp;
            }

            return pointsToRemove;
        }
    }
}