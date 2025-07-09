using UnityEditor;
using UnityEngine;
using System.IO;

namespace Suasor.DstoolsRewrite
{
    public class DstoolsRewrite : EditorWindow
    {
        public enum GameType
        {
            Undefined,
            DarkSoulsPTDE,
            DarkSoulsRemastered,
            DarkSoulsIISOTFS,
            DarkSoulsIII,
            Bloodborne,
            Sekiro,
            EldenRing
        }

        private static readonly string[] EditorPrefsKeys = new string[]
        {
            "",
            "DS_PTDE_ExePath",
            "DS_Remastered_ExePath",
            "DS2_SOTFS_ExePath",
            "DS3_ExePath",
            "Bloodborne_ExePath",
            "Sekiro_ExePath",
            "EldenRing_ExePath"
        };

        private static readonly string[] DefaultPaths = new string[]
        {
            "",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Dark Souls Prepare to Die Edition",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\DARK SOULS REMASTERED",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Dark Souls II Scholar of the First Sin",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\DARK SOULS III\\Game",
            @"",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Sekiro",
            @"C:\\Program Files (x86)\\Steam\\steamapps\\common\\ELDEN RING\\Game"
        };

        private static readonly string[] ExeNames = new string[]
        {
            "",
            "DATA.exe",
            "DarkSoulsRemastered.exe",
            "DarkSoulsII.exe",
            "DarkSoulsIII.exe",
            "eboot.bin",
            "sekiro.exe",
            "eldenring.exe"
        };

        private static readonly string LastGameKey = "Dstools_LastSelectedGame";

        private GameType selectedGame = GameType.Undefined;
        private string currentExePath;

        [MenuItem("Tools/Suasor/DstoolsRewrite")]
        public static void ShowWindow()
        {
            GetWindow<DstoolsRewrite>("DstoolsRewrite");
        }

        private void OnEnable()
        {
            selectedGame = (GameType)EditorPrefs.GetInt(LastGameKey, (int)GameType.Undefined);
            LoadPath();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            GameType newGame = (GameType)EditorGUILayout.EnumPopup("Game", selectedGame);
            if (EditorGUI.EndChangeCheck())
            {
                selectedGame = newGame;
                EditorPrefs.SetInt(LastGameKey, (int)selectedGame);
                LoadPath();
            }

            LoadExecutablePicker();
        }

        private void LoadPath()
        {
            string key = EditorPrefsKeys[(int)selectedGame];
            string defaultPath = Path.Combine(DefaultPaths[(int)selectedGame], ExeNames[(int)selectedGame]);
            currentExePath = EditorPrefs.GetString(key, defaultPath);
        }

        private void LoadExecutablePicker()
        {
            GUILayout.Label($"Selected {ExeNames[(int)selectedGame]}:", EditorStyles.label);

            EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight));

            currentExePath = EditorGUILayout.TextField(currentExePath);

            GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");
            if (GUILayout.Button(folderIcon, GUILayout.Width(26), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                string initialPath = Path.GetDirectoryName(currentExePath);
                string selected = EditorUtility.OpenFilePanel($"Select {ExeNames[(int)selectedGame]}", initialPath, "exe");

                if (!string.IsNullOrEmpty(selected))
                {
                    if (Path.GetFileName(selected).ToLower() == ExeNames[(int)selectedGame].ToLower())
                    {
                        currentExePath = selected;
                        EditorPrefs.SetString(EditorPrefsKeys[(int)selectedGame], currentExePath);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Invalid File", $"Please select {ExeNames[(int)selectedGame]}", "OK");
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            bool exeExists = File.Exists(currentExePath);
            GUIContent statusContent = new GUIContent(
                exeExists ? $"✔ {ExeNames[(int)selectedGame]} is found" : $"✘ {ExeNames[(int)selectedGame]} is NOT found"
            );

            GUIStyle statusStyle = new GUIStyle(EditorStyles.label);
            statusStyle.normal.textColor = exeExists ? Color.green : Color.red;
            GUILayout.Label(statusContent, statusStyle);
        }
    }
}
