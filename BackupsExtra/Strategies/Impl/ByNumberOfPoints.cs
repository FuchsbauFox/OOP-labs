using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class ByNumberOfPoints : ICleaningStrategy
    {
        public ByNumberOfPoints(int number)
        {
            CheckNumber(number);
            Number = number;
        }

        [JsonProperty]
        protected int Number { get; }

        public void CleaningPoints(BackupJobExtra backupJobExtra)
        {
            foreach (IRestorePoint point in GetListPointsToRemove(backupJobExtra))
            {
                backupJobExtra.DeleteRestorePoint(point.Name);
            }
        }

        public List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            return backupJobExtra.Points().Take(backupJobExtra.Points().Count - Number).ToList();
        }

        private void CheckNumber(int number)
        {
            if (number <= 0)
                throw new ArgumentException();
        }
    }
}