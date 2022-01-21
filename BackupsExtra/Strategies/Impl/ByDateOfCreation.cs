using System;
using System.Linq;
using Backups.MyDateTime;
using BackupsExtra.BackupsExtra.Impl;
using BackupsExtra.Tools.BackupsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class ByDateOfCreation : ICleaningStrategy
    {
        [JsonProperty]
        private int _days;
        [JsonProperty]
        private int _months;
        [JsonProperty]
        private int _years;
        [JsonProperty]
        private BackupJobExtra _backupJobExtra;

        public ByDateOfCreation(int days, int months, int years)
        {
            CheckData(days, months, years);
            _days = days;
            _months = months;
            _years = years;
        }

        public void SetBackupJobExtra(BackupJobExtra backupJobExtra)
        {
            _backupJobExtra = backupJobExtra;
        }

        public void CheckingAndCleaningPoints()
        {
            var restorePointsForDelete = _backupJobExtra.Points()
                .Select(restorePoint => new
                {
                    restorePoint.Name, date = restorePoint.Time.AddDays(_days).AddMonths(_months).AddYears(_years),
                })
                .Where(point => CurrentDate.GetInstance().Date > point.date)
                .Select(point => point.Name).ToList();

            if (restorePointsForDelete.Count >= _backupJobExtra.Points().Count)
                throw new AllRestorePointsNotPassedLimitsException();

            foreach (string name in restorePointsForDelete)
            {
                _backupJobExtra.DeleteRestorePoint(name);
            }
        }

        private void CheckData(int days, int months, int years)
        {
            if (days < 0 || months < 0 || years < 0 || (days == 0 && months == 0 && years == 0))
                throw new ArgumentException();
        }
    }
}