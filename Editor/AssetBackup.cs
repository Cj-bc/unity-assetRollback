using System;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;

namespace AssetRollback
{
    /// <summary>One backup file for one asset.</summary>
    public struct AssetBackup
    {
        public DateTime lastWriteTime { get; private set; }
        /// <summary>path to target Asset. Relative to <c>Application.dataPath</c></summary>
        public string assetPath { get; private set; }
        /// Identifier of this asset.
        public string identifier { get; private set; }
        /// <summary>Actual filename of this backup</summary>
        public string backupFileName
        { get => $"{identifier}_{lastWriteTime.Year:D4}-{lastWriteTime.Month:D2}-{lastWriteTime.Day:D2}-{lastWriteTime.Hour:D2}"
                + $"-{lastWriteTime.Minute:D2}-{lastWriteTime.Second:D2}";
        }
        
        private const string identifierRegex = @"(?<identifier>[^_]+)_(?<lastWriteTime>\d{4}-\d{2}-\d{2}-\d{2}-\d{2}-\d{2})$";
        
        /// <summary>Construct AssetBackup that represents given backupFile</summary>
        public AssetBackup(string backupPath)
        {
            assetPath = ""; // TODO: how can I retrive this?
            var matches = Regex.Match(Path.GetFileName(backupPath), identifierRegex);
            if (!matches.Success)
            {
                throw new ArgumentException("Filename should match identifierRegex, but failed to parse it");
            }
            
            identifier = matches.Groups["identifier"].Value;
            string _timestamp = matches.Groups["lastWriteTime"].Value;
            System.DateTime.TryParse(_timestamp, out DateTime _time);
            lastWriteTime = _time;
        }
        
        internal AssetBackup(string assetPath, string identifier, DateTime lastWriteTime)
        {
            this.assetPath = assetPath;
            this.identifier = identifier;
            this.lastWriteTime = lastWriteTime;
        }
        
        /// <summary>Creates <c>AssetBackup</c> for given asset path</summary>
        public static AssetBackup Create(string assetPath)
        {
            string identifier = ToIdentifier(assetPath);
            DateTime lastWriteTime = File.GetLastWriteTime(assetPath);
            var backupInfo = new AssetBackup(assetPath, identifier, lastWriteTime);
            File.Copy(assetPath, Path.Combine(AssetRollback.backupStorePath, backupInfo.backupFileName));
            return backupInfo;
        }
        
        /// <summary>Calculate identifier for given path.</summary>
        public static string ToIdentifier(string path)
            => Path.GetRelativePath(Path.GetDirectoryName(Application.dataPath),
                                    path.Replace('/', '-').Replace(' ', '-').Replace('\\', '-'));
    }
}
