using System;
using System.Collections.Generic;

namespace Backups.Backups
{
    public interface IRestorePoint
    {
        DateTime Time { get; }
        string Name { get; }
        string Algorithm { get; }
        IReadOnlyList<string> BackupJobs();
    }
}