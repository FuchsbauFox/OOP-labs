using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using BackupsExtra.Tools.BackupsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridIfAll : ICleaningStrategy
    {
        [JsonProperty]
        private List<ICleaningStrategy> _strategies;

        public HybridIfAll(List<ICleaningStrategy> strategies)
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

        private void CheckStrategies(List<ICleaningStrategy> strategies)
        {
            if (strategies == null)
                throw new ArgumentNullException();
        }
    }
}