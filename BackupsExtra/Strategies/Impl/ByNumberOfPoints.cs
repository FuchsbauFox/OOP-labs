using System;
using System.Linq;
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
            BackupJobExtra = null;
        }

        [JsonProperty]
        protected int Number { get; }
        [JsonProperty]
        private BackupJobExtra BackupJobExtra { get; set; }

        public void SetBackupJobExtra(BackupJobExtra backupJobExtra)
        {
            BackupJobExtra = backupJobExtra;
        }

        public void CheckingAndCleaningPoints()
        {
            while (BackupJobExtra.Points().Count > Number)
            {
                BackupJobExtra.DeleteRestorePoint(BackupJobExtra.Points().First().Name);
            }
        }

        private void CheckNumber(int argument)
        {
            if (argument <= 0)
                throw new ArgumentException();
        }
    }
}