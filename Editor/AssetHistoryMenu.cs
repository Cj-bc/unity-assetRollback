using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssetRollback
{
    class AssetRollbackMenu : MonoBehaviour
    {
        private static IEnumerable<string> GetBackupPathsFor(string path)
        {
            return Directory.EnumerateFiles(AssetRollback.backupStorePath, AssetRollback.BackupFileGlob(path));
        }
        
        [MenuItem("Assets/Show Asset rollback files")]
        static void ShowMenu()
        {
            if (Selection.objects.Length > 0)
            {
                var menu = new GenericMenu();
                foreach (var path in GetBackupPathsFor(AssetDatabase.GetAssetPath(Selection.objects[0])))
                {
                    menu.AddItem(new GUIContent(path), false, () => Debug.Log("Clicked"));
                }
                menu.DropDown(Rect.zero);
            }
        }
    }
}
