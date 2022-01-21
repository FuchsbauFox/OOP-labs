using System;
using System.Linq;
using Backups.MyDateTime;
using BackupsExtra.BackupsExtra.Impl;
using BackupsExtra.Tools.BackupsExtra;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridDateOrNumber : ICleaningStrategy
    {
        [JsonProperty]
        private readonly int _number;
        [JsonProperty]
        private int _days;
        [JsonProperty]
        private int _months;
        [JsonProperty]
        private int _years;
        [JsonProperty]
        private BackupJobExtra _backupJobExtra;

        public HybridDateOrNumber(int number, int days, int months, int years)
        {
            CheckNumber(number);
            CheckData(days, months, years);
            _number = number;
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

            int i = restorePointsForDelete.Count;
            while (_backupJobExtra.Points().Count - _number > restorePointsForDelete.Count)
            {
                restorePointsForDelete.Add(_backupJobExtra.Points()[i].Name);
                i++;
            }

            foreach (string name in restorePointsForDelete)
            {
                _backupJobExtra.DeleteRestorePoint(name);
            }
        }

        private void CheckNumber(int argument)
        {
            if (argument <= 0)
                throw new ArgumentException();
        }

        private void CheckData(int days, int months, int years)
        {
            if (days < 0 || months < 0 || years < 0 || (days == 0 && months == 0 && years == 0))
                throw new ArgumentException();
        }
    }
}