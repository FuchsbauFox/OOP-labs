using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Algorithm;
using Backups.Algorithm.Impl;
using Backups.FSAdapter;
using Backups.Tools.BackupJobException;
using Newtonsoft.Json;

namespace Backups.Backups.Impl
{
    public class BackupJob : IBackupJob
    {
        public BackupJob(IFsAdapter adapter)
        {
            Adapter = adapter;
            JobObjects = new List<string>();
            RestorePoints = new List<IRestorePoint>();
            AlgorithmStorage = null;
        }

        [JsonProperty]
        protected IFsAdapter Adapter { get; }
        [JsonProperty]
        protected List<string> JobObjects { get; }
        [JsonProperty]
        protected List<IRestorePoint> RestorePoints { get; }
        [JsonProperty]
        protected IAlgorithmStorage AlgorithmStorage { get; set; }
        public IReadOnlyList<string> Objects() => JobObjects;
        public IReadOnlyList<IRestorePoint> Points() => RestorePoints;

        public void AddJobObject(string path)
        {
            CheckJobObject(path);

            JobObjects.Add(path);
        }

        public void RemoveJobObject(string path)
        {
            CheckJobObject(path, true);

            JobObjects.Remove(path);
        }

        public void SetAlgorithmStorage(string algorithm)
        {
            AlgorithmStorage = algorithm switch
            {
                "split" => new SplitStorage(Adapter),
                "single" => new SingleStorage(Adapter),
                _ => throw new AlgorithmNotFoundException()
            };
        }

        public void CreateBackup(string name)
        {
            CheckAlgorithm();
            CheckRestorePoint(name);

            RestorePoints.Add(AlgorithmStorage.CreateRestorePoint(name, JobObjects));
        }

        private void CheckJobObject(string path, bool shouldExist = false)
        {
            bool jobObjectExist = JobObjects.Any(jobObject => jobObject == path);
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
            if (AlgorithmStorage == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void CheckRestorePoint(string name)
        {
            if (RestorePoints.Any(restorePoint => restorePoint.Name == name))
            {
                throw new RestorePointWithThisNameAlreadyExistException();
            }
        }
    }
}