using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetRollback
{
    class AssetRollbackMenu : MonoBehaviour
    {
        private static IEnumerable<AssetBackup> GetBackupPathsFor(string path)
        {
            return Directory.EnumerateFiles(AssetRollback.backupStorePath, AssetRollback.BackupFileGlob(path))
                .Select(s => new AssetBackup(s));
        }
        
        [MenuItem("Assets/Show Asset rollback files")]
        static void ShowMenu()
        {
            if (Selection.objects.Length > 0)
            {
                var menu = new GenericMenu();
                foreach (var path in GetBackupPathsFor(AssetDatabase.GetAssetPath(Selection.objects[0])))
                {
                    menu.AddItem(new GUIContent(path.lastWriteTime.ToString("s")), false, () => Debug.Log("Clicked"));
                }
                menu.DropDown(Rect.zero);
            }
        }
    }
}
