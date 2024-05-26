using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetRollback
{
    /// <summary>Do packing of backup</summary>
    public class AssetBackupPacker : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string[] paths)
        {
            AssetRollback.EnsureStoreExists();
            foreach (var path in paths)
            {
                Debug.Log($"Saving: {path}");
                var backup = AssetBackup.Create(path);
            }
            return paths;
        }

        static private string BackupPathBase(string filepath)
        {
            return Path.GetRelativePath(Path.GetDirectoryName(Application.dataPath), filepath).Replace('/', '-').Replace(' ', '-').Replace('\\', '-');
        }
    }
}
