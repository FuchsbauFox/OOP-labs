using System.Linq;
using Backups.Backups;
using Backups.FSAdapter;
using BackupsExtra.Enrichers;
using BackupsExtra.Strategies;
using Newtonsoft.Json;
using Serilog;

namespace BackupsExtra.BackupsExtra.Impl
{
    public class Backup : IBackup, IBackupExtra
    {
        public Backup(IFsAdapter adapter, ICleaningStrategy strategy, string loggerType)
        {
            BackupJobExtra = new BackupJobExtra(adapter);
            Strategy = strategy;
            Strategy.SetBackupJobExtra(BackupJobExtra);

            Log.Logger = loggerType switch
            {
                "console" => new LoggerConfiguration().Enrich.With(new ThreadIdEnricher())
                    .WriteTo.Console(
                        outputTemplate: "{Timestamp:HH:mm} [{Level}] {Message}{NewLine}{NewLine}{Exception}")
                    .CreateLogger(),
                "file" => new LoggerConfiguration().Enrich.With(new ThreadIdEnricher())
                    .WriteTo.File(
                        "log.txt",
                        outputTemplate: "{Timestamp:HH:mm} [{Level}] {Message}{NewLine}{NewLine}{Exception}")
                    .CreateLogger(),
                _ => Log.Logger
            };
        }

        [JsonProperty]
        public BackupJobExtra BackupJobExtra { get; }
        [JsonProperty]
        protected ICleaningStrategy Strategy { get; }

        public void AddJobObject(string path)
        {
            BackupJobExtra.AddJobObject(path);

            Log.Information(
                "Add job object: {Path},\n\tList JobObjects: {JobObjects}",
                path,
                BackupJobExtra.Objects());
        }

        public void RemoveJobObject(string path)
        {
            BackupJobExtra.RemoveJobObject(path);

            Log.Information(
                "Remove job object: {Path},\n\tList JobObjects: {JobObjects}",
                path,
                BackupJobExtra.Objects());
        }

        public void SetAlgorithmStorage(string algorithm)
        {
            BackupJobExtra.SetAlgorithmStorage(algorithm);

            Log.Information("Set algorithm storage: {Algorithm}", algorithm);
        }

        public void CreateBackup(string name)
        {
            BackupJobExtra.CreateBackup(name);
            Log.Information(
                "Create RestorePoint: {@RestorePoint}\n\tList JobObjects: {JobObjects}",
                BackupJobExtra.Points().Last(),
                BackupJobExtra.Points().Last().Jobs());
            Strategy.CheckingAndCleaningPoints();
            Log.Information(
                "Non-removed points: {@RestorePoints}",
                BackupJobExtra.Points());
        }

        public void MergeRestorePoints(string oldPoint, string newPoint)
        {
            BackupJobExtra.MergeRestorePoints(oldPoint, newPoint);
            Log.Information(
                "Merge {OldPoint} with {NewPoint}\n\tRestore points: {@RestorePoints}",
                oldPoint,
                newPoint,
                BackupJobExtra.Points());
        }

        public void ToOriginalLocation(string restorePointName)
        {
            BackupJobExtra.ToOriginalLocation(restorePointName);
            Log.Information("Recovery {RestorePoint} to original location", restorePointName);
        }

        public void ToDifferentLocation(string restorePointName, string dirPath)
        {
            BackupJobExtra.ToDifferentLocation(restorePointName, dirPath);
            Log.Information("Recovery {RestorePoint} to {DirPath}", restorePointName, dirPath);
        }

        public void DeleteRestorePoint(string restorePointName)
        {
            BackupJobExtra.DeleteRestorePoint(restorePointName);
            Log.Information("Remove RestorePoint {RestorePoint}", restorePointName);
        }
    }
}