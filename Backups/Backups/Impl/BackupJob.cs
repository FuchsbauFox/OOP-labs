using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Algorithm;
using Backups.Algorithm.Impl;
using Backups.FSAdapter;
using Backups.Tools.BackupJobException;

namespace Backups.Backups.Impl
{
    public class BackupJob : IBackupJob
    {
        private readonly IFsAdapter _adapter;
        private readonly List<string> _jobObjects;
        private readonly List<IRestorePoint> _restorePoints;
        private IAlgorithmStorage _algorithmStorage;

        public BackupJob(IFsAdapter adapter)
        {
            _adapter = adapter;
            _jobObjects = new List<string>();
            _restorePoints = new List<IRestorePoint>();
            _algorithmStorage = null;
        }

        public IReadOnlyList<string> JobObjects() => _jobObjects;
        public IReadOnlyList<IRestorePoint> RestorePoints() => _restorePoints;

        public void AddJobObject(string path)
        {
            CheckJobObject(path);

            _jobObjects.Add(path);
        }

        public void RemoveJobObject(string path)
        {
            CheckJobObject(path, true);

            _jobObjects.Remove(path);
        }

        public void SetAlgorithmStorage(string algorithm)
        {
            _algorithmStorage = algorithm switch
            {
                "split" => new SplitStorage(_adapter),
                "single" => new SingleStorage(_adapter),
                _ => throw new AlgorithmNotFoundException()
            };
        }

        public void CreateBackup(string name)
        {
            CheckAlgorithm();
            CheckRestorePoint(name);

            _restorePoints.Add(_algorithmStorage.CreateRestorePoint(name, _jobObjects));
        }

        private void CheckJobObject(string path, bool shouldExist = false)
        {
            bool jobObjectExist = _jobObjects.Any(jobObject => jobObject == path);
            switch (shouldExist)
            {
                case false when jobObjectExist:
                    throw new JobObjectAlreadyExistException();
                case true when !jobObjectExist:
                    throw new JobObjectNotExistException();
            }
        }

        private void CheckAlgorithm()
        {
            if (_algorithmStorage == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void CheckRestorePoint(string name)
        {
            if (_restorePoints.Any(restorePoint => restorePoint.Name == name))
            {
                throw new RestorePointWithThisNameAlreadyExistException();
            }
        }
    }
}