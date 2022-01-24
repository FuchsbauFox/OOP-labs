using System.Collections.Generic;
using Backups.Backups;
using Backups.FSAdapter;

namespace Backups.Algorithm
{
    public interface IAlgorithmStorage
    {
        IRestorePoint CreateRestorePoint(IFsAdapter adapter, string restorePointName, List<string> jobObjects);
        List<string> MergeRestorePoints(IFsAdapter adapter, IRestorePoint oldRestorePoint, IRestorePoint newRestorePoint);
    }
}