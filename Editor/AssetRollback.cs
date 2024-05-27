/**
This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at https://mozilla.org/MPL/2.0/.
**/
using System.IO;
using UnityEngine;

namespace AssetRollback
{
    /// <summary>Singleton object that contains all configurations for AssetRollback package</summary>
    static class AssetRollback
    {
        /// <summary>Root directory where backup files are stored.</summary>
        public static string backupStorePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), ".assetBackup");

        /// <summary>Creates store directory if it does not exists.</summary>
        public static void EnsureStoreExists()
        {
            if (!Directory.Exists(backupStorePath))
            {
                Directory.CreateDirectory(backupStorePath);
            }
        }

        /// <summary>Constructs and returns glob pattern that expands to
        /// all backup files for given <param name="filepath">filepath</param></summary>
        static public string BackupFileGlob(string filepath) => $"{AssetBackup.ToIdentifier(filepath)}_*";
    }
}
