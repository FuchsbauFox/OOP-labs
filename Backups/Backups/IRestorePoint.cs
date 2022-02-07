using System;
using System.Collections.Generic;
using Backups.Algorithm;

namespace Backups.Backups
{
    public interface IRestorePoint
    {
        DateTime Time { get; }
        string Name { get; }
        IAlgorithmStorage Algorithm { get; }
        IReadOnlyList<string> Jobs();
    }
}