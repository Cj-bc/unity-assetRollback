using System;
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
            if (Selection.objects.Length > 0 && AssetDatabase.Contains(Selection.objects[0]))
            {
                var selectedObject = Selection.objects[0];
                var correspondingAssetPath = AssetDatabase.GetAssetPath(selectedObject);
                var menu = new GenericMenu();
                foreach (var path in GetBackupPathsFor(correspondingAssetPath))
                {
                    menu.AddItem(new GUIContent(path.lastWriteTime.ToString("s")), false, (obj) =>
                    {
                        Undo.RecordObject(obj as UnityEngine.Object, "Rollbacked asset.");
                        File.Copy(Path.Combine(AssetRollback.backupStorePath, path.backupFileName), correspondingAssetPath, true);
                        File.SetLastWriteTime(correspondingAssetPath, DateTime.Now);
                        AssetDatabase.Refresh();
                    }, selectedObject);
                }
                menu.DropDown(Rect.zero);
            } else
            {
                Debug.Log("Show Asset rollback files: Could not find available backups for given object");
            }
        }
    }
}
