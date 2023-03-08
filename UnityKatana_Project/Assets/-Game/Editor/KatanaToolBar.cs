using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Codice.CM.Common.Tree;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityToolbarExtender
{
    [InitializeOnLoad]
    public class KatanaToolBar : MonoBehaviour
    {
        static KatanaToolBar()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUILeft);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
        }

        private static void OnToolbarGUIRight()
        {
            // if (GUILayout.Button(new GUIContent("LevelPrefabs"), SirenixGUIStyles.ToolbarButtonSelected))
            // {
            //     ShowFolderContents("Resources");
            // }
            // if (GUILayout.Button(new GUIContent("-Art"), SirenixGUIStyles.ToolbarButtonSelected))
            // {
            //     ShowFolderContents("-Art");
            // }
            // if (GUILayout.Button(new GUIContent("-Game"), SirenixGUIStyles.ToolbarButtonSelected))
            // {
            //     ShowFolderContents("-Game");
            // }
        }

        public static void ShowFolderContents(string path)
        {
            var getInstanceIDMethod = typeof(AssetDatabase).GetMethod("GetMainAssetInstanceID",
                BindingFlags.Static | BindingFlags.NonPublic);
            var instanceID = (int) getInstanceIDMethod.Invoke(null, new object[] {path});
            ShowFolderContents(instanceID);
        }

        private static void ShowFolderContents(int folderInstanceID)
        {
            // Find the internal ProjectBrowser class in the editor assembly.
            var editorAssembly = typeof(Editor).Assembly;
            var projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");

            // This is the internal method, which performs the desired action.
            // Should only be called if the project window is in two column mode.
            var showFolderContents = projectBrowserType.GetMethod(
                "ShowFolderContents", BindingFlags.Instance | BindingFlags.NonPublic);

            // Find any open project browser windows.
            var projectBrowserInstances = Resources.FindObjectsOfTypeAll(projectBrowserType);

            if (projectBrowserInstances.Length > 0)
            {
                for (var i = 0; i < projectBrowserInstances.Length; i++)
                    ShowFolderContentsInternal(projectBrowserInstances[i], showFolderContents, folderInstanceID);
            }
            else
            {
                var projectBrowser = OpenNewProjectBrowser(projectBrowserType);
                ShowFolderContentsInternal(projectBrowser, showFolderContents, folderInstanceID);
            }
        }

        private static void ShowFolderContentsInternal(Object projectBrowser, MethodInfo showFolderContents,
            int folderInstanceID)
        {
            // Sadly, there is no method to check for the view mode.
            // We can use the serialized object to find the private property.
            var serializedObject = new SerializedObject(projectBrowser);
            var inTwoColumnMode = serializedObject.FindProperty("m_ViewMode").enumValueIndex == 1;

            if (!inTwoColumnMode)
            {
                // If the browser is not in two column mode, we must set it to show the folder contents.
                var setTwoColumns = projectBrowser.GetType().GetMethod(
                    "SetTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
                setTwoColumns.Invoke(projectBrowser, null);
            }

            var revealAndFrameInFolderTree = true;
            showFolderContents.Invoke(projectBrowser, new object[] {folderInstanceID, revealAndFrameInFolderTree});
        }

        private static EditorWindow OpenNewProjectBrowser(Type projectBrowserType)
        {
            var projectBrowser = EditorWindow.GetWindow(projectBrowserType);
            projectBrowser.Show();

            // Unity does some special initialization logic, which we must call,
            // before we can use the ShowFolderContents method (else we get a NullReferenceException).
            var init = projectBrowserType.GetMethod("Init", BindingFlags.Instance | BindingFlags.Public);
            init.Invoke(projectBrowser, null);

            return projectBrowser;
        }

        private static void OnToolbarGUILeft()
        {
            var options = GUILayout.Width(150);
            if (GUILayout.Button(new GUIContent("Clear PlayerPrefs"), options))
            {
                Debug.Log("PlayerPrefs Cleared");
                PlayerPrefs.DeleteAll();
            }
        }

        private static void NamesOnSelectionConfirmed(IEnumerable<SceneAsset> obj)
        {
            string path2;

            path2 = AssetDatabase.GetAssetOrScenePath(obj.FirstOrDefault());
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) EditorSceneManager.OpenScene(path2);
        }

        public static T[] GetAtPath<T>(string path)
        {
            var al = new ArrayList();
            var fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (var fileName in fileEntries)
            {
                var temp = fileName.Replace("\\", "/");
                var index = temp.LastIndexOf("/");
                var localPath = "Assets/" + path;

                if (index > 0)
                    localPath += temp.Substring(index);

                object t = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));

                if (t != null)
                    al.Add(t);
            }

            var result = new T[al.Count];

            for (var i = 0; i < al.Count; i++)
                result[i] = (T) al[i];

            return result;
        }

        private static string GETMenuItemName(SceneAsset arg)
        {
            return arg.name;
        }
    }
}