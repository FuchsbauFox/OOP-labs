using System;
using System.Linq;
using Backups.MyDateTime;
using BackupsExtra.BackupsExtra.Impl;
using Newtonsoft.Json;

namespace BackupsExtra.Strategies.Impl
{
    public class HybridDateAndNumber : ICleaningStrategy
    {
        [JsonProperty]
        private BackupJobExtra _backupJobExtra;

        public HybridDateAndNumber(int number, int days, int months, int years)
        {
            CheckNumber(number);
            CheckData(days, months, years);
            Number = number;
            Days = days;
            Months = months;
            Years = years;
        }

        [JsonProperty]
        protected int Number { get; }

        [JsonProperty]
        protected int Days { get; }
        [JsonProperty]
        protected int Months { get; }
        [JsonProperty]
        protected int Years { get; }

        public void SetBackupJobExtra(BackupJobExtra backupJobExtra)
        {
            _backupJobExtra = backupJobExtra;
        }

        public void CheckingAndCleaningPoints()
        {
            var restorePointsForDelete = _backupJobExtra.Points()
                .Select(restorePoint => new
                {
                    restorePoint.Name, date = restorePoint.Time.AddDays(Days).AddMonths(Months).AddYears(Years),
                })
                .Where(point => CurrentDate.GetInstance().Date > point.date)
                .Select(point => point.Name).ToList();

            int i = restorePointsForDelete.Count;
            while (_backupJobExtra.Points().Count - Number < restorePointsForDelete.Count && restorePointsForDelete.Count != 0)
            {
                restorePointsForDelete.Remove(_backupJobExtra.Points()[i].Name);
                i--;
            }

            foreach (string name in restorePointsForDelete)
            {
                _backupJobExtra.DeleteRestorePoint(name);
            }
        }

        private void CheckNumber(int number)
        {
            if (number <= 0)
                throw new ArgumentException();
        }

        private void CheckData(int days, int months, int years)
        {
            if (days < 0 || months < 0 || years < 0 || (days == 0 && months == 0 && years == 0))
                throw new ArgumentException();
        }
    }
}