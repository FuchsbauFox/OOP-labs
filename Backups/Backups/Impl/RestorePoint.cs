using System;
using System.Collections.Generic;

namespace Backups.Backups.Impl
{
    public class RestorePoint : IRestorePoint
    {
        private readonly List<string> _backupJobs;

        public RestorePoint(string name, string algorithm, List<string> listBackupJobs)
        {
            CheckName(name);

            Time = DateTime.Now;
            Name = name;
            Algorithm = algorithm;
            _backupJobs = new List<string>(listBackupJobs);
        }

        public DateTime Time { get; }
        public string Name { get; }
        public string Algorithm { get; }
        public IReadOnlyList<string> BackupJobs() => _backupJobs;

        private void CheckName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}