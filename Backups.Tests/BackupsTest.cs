using System.Collections.Generic;
using System.Linq;
using Backups.Backups;
using Backups.FSAdapter;
using Backups.FSAdapter.Impl;
using Backups.Backups.Impl;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private IFsAdapter _adapter;
        private IBackupJob _backupJob;
        
        [SetUp]
        public void Setup()
        {
            _adapter = new VirtualFsAdapter();
            _adapter.AddDirectory(@"C:\programming");
            _adapter.AddDirectory(@"C:\programming\OOP");
            _adapter.AddDirectory(@"C:\programming\Algorithm");
            
            _adapter.AddFile(@"C:\programming\OOP\lab-0.txt");
            _adapter.AddContentOnFile(@"C:\programming\OOP\lab-0.txt", "It's lab-0");
            _adapter.AddFile(@"C:\programming\OOP\lab-1.txt");
            _adapter.AddContentOnFile(@"C:\programming\OOP\lab-1.txt", "It's lab-1");
            _adapter.AddFile(@"C:\programming\OOP\lab-2.txt");
            _adapter.AddContentOnFile(@"C:\programming\OOP\lab-2.txt", "It's lab-2");
            _adapter.AddFile(@"C:\programming\OOP\lab-3.txt");
            _adapter.AddContentOnFile(@"C:\programming\OOP\lab-3.txt", "It's lab-3");
            
            _adapter.AddFile(@"C:\programming\Algorithm\DFS.txt");
            _adapter.AddContentOnFile(@"C:\programming\Algorithm\DFS.txt", "Algorithm DFS:");
            _adapter.AddFile(@"C:\programming\Algorithm\BFS.txt");
            _adapter.AddContentOnFile(@"C:\programming\Algorithm\BFS.txt", "Algorithm BFS:");
            
            _backupJob = new BackupJob(_adapter);
        }

        [Test]
        public void CreateTwoRestorePoints_DifferentFilesInOneDirectory_DifferentAlgorithm()
        {
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-0.txt");
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-1.txt");
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-2.txt");
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-3.txt");

            var objects = new List<string>()
            {
                @"C:\programming\OOP\lab-0.txt",
                @"C:\programming\OOP\lab-1.txt",
                @"C:\programming\OOP\lab-2.txt",
                @"C:\programming\OOP\lab-3.txt"
            };
            Assert.AreEqual(objects, _backupJob.Objects().ToList());

            _backupJob.SetAlgorithmStorage("single");
            _backupJob.CreateBackup("RestorePoint1");
            
            Assert.AreEqual(1, _backupJob.Points().Count);
            Assert.AreEqual("single", _backupJob.Points().ToList()[0].Algorithm);
            Assert.AreEqual(objects, _backupJob.Points().ToList()[0].Jobs().ToList());
            
            
            
            _backupJob.RemoveJobObject(@"C:\programming\OOP\lab-0.txt");
            _backupJob.RemoveJobObject(@"C:\programming\OOP\lab-1.txt");
            objects.Remove(@"C:\programming\OOP\lab-0.txt");
            objects.Remove(@"C:\programming\OOP\lab-1.txt");
            Assert.AreEqual(objects, _backupJob.Objects().ToList());

            _backupJob.SetAlgorithmStorage("split");
            _backupJob.CreateBackup("RestorePoint2");
            
            Assert.AreEqual(2, _backupJob.Points().Count);
            Assert.AreEqual("split", _backupJob.Points().ToList()[1].Algorithm);
            Assert.AreEqual(objects, _backupJob.Points().ToList()[1].Jobs().ToList());
        }
        
        [Test]
        public void CreateTwoRestorePoints_DifferentFilesInDifferentDirectory_DifferentAlgorithm()
        {
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-0.txt");
            _backupJob.AddJobObject(@"C:\programming\Algorithm\DFS.txt");
            _backupJob.AddJobObject(@"C:\programming\OOP\lab-2.txt");
            _backupJob.AddJobObject(@"C:\programming\Algorithm\BFS.txt");

            var objects = new List<string>()
            {
                @"C:\programming\OOP\lab-0.txt",
                @"C:\programming\Algorithm\DFS.txt",
                @"C:\programming\OOP\lab-2.txt",
                @"C:\programming\Algorithm\BFS.txt"
            };
            Assert.AreEqual(objects, _backupJob.Objects().ToList());

            _backupJob.SetAlgorithmStorage("single");
            _backupJob.CreateBackup("RestorePoint1");
            
            Assert.AreEqual(1, _backupJob.Points().Count);
            Assert.AreEqual("single", _backupJob.Points().ToList()[0].Algorithm);
            Assert.AreEqual(objects, _backupJob.Points().ToList()[0].Jobs().ToList());
            
            
            
            _backupJob.RemoveJobObject(@"C:\programming\OOP\lab-0.txt");
            _backupJob.RemoveJobObject(@"C:\programming\Algorithm\DFS.txt");
            objects.Remove(@"C:\programming\OOP\lab-0.txt");
            objects.Remove(@"C:\programming\Algorithm\DFS.txt");
            Assert.AreEqual(objects, _backupJob.Objects().ToList());

            _backupJob.SetAlgorithmStorage("split");
            _backupJob.CreateBackup("RestorePoint2");
            
            Assert.AreEqual(2, _backupJob.Points().Count);
            Assert.AreEqual("split", _backupJob.Points().ToList()[1].Algorithm);
            Assert.AreEqual(objects, _backupJob.Points().ToList()[1].Jobs().ToList());
        }
    }
}