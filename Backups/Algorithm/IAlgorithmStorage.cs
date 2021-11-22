using System.Collections.Generic;
using Backups.Backups;

namespace Backups.Algorithm
{
    public interface IAlgorithmStorage
    {
        IRestorePoint CreateRestorePoint(string restorePointName, List<string> jobObjects);
    }
}