using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class ByNumberOfPoints : CleaningStrategy
    {
        public ByNumberOfPoints(int number)
        {
            if (number <= 0)
                throw new ArgumentException();
            Number = number;
        }

        [JsonProperty]
        protected int Number { get; }

        public override List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            return backupJobExtra.Points().Take(backupJobExtra.Points().Count - Number).ToList();
        }
    }
}