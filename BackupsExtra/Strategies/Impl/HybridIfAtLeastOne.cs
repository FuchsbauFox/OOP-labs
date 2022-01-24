using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using BackupsExtra.Tools.BackupsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridIfAtLeastOne : ICleaningStrategy
    {
        [JsonProperty]
        private List<ICleaningStrategy> _strategies;

        public HybridIfAtLeastOne(List<ICleaningStrategy> strategies)
        {
            CheckStrategies(strategies);
            _strategies = strategies;
        }

        public void CleaningPoints(BackupJobExtra backupJobExtra)
        {
            List<IRestorePoint> pointsToRemove = GetListPointsToRemove(backupJobExtra);

            if (pointsToRemove.Count >= backupJobExtra.Points().Count)
                throw new AllRestorePointsNotPassedLimitsException();

            foreach (IRestorePoint point in pointsToRemove)
                backupJobExtra.DeleteRestorePoint(point.Name);
        }

        public List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            return _strategies.Aggregate(
                new List<IRestorePoint>(),
                (current, strategy) => current
                    .Union(strategy.GetListPointsToRemove(backupJobExtra)).ToList());
        }

        private void CheckStrategies(List<ICleaningStrategy> strategies)
        {
            if (strategies == null)
                throw new ArgumentNullException();
        }
    }
}