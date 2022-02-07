using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.MyDateTime;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class ByDateOfCreation : CleaningStrategy
    {
        [JsonProperty]
        private TimeSpan _timeSpan;

        public ByDateOfCreation(TimeSpan timeSpan)
        {
            if (timeSpan == null)
                throw new ArgumentNullException();
            if (timeSpan.Ticks <= 0)
                throw new ArgumentException();
            _timeSpan = timeSpan;
        }

        public override List<IRestorePoint> GetListPointsToRemove(BackupJobExtra backupJobExtra)
        {
            return backupJobExtra.Points()
                .Select(point => point)
                .Where(point => CurrentDate.GetInstance().Date > point.Time + _timeSpan)
                .Select(point => point).ToList();
        }
    }
}