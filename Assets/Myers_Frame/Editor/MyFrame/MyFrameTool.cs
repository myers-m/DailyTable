using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MyFrame {
    public class MyFrameTool : Editor {

        [MenuItem("/MyFrame/Start")]
        public static void CrateLoader() {
            if (!GameObject.FindObjectOfType<MyLoader>() && !Directory.Exists(Application.dataPath + "/Excel")) {
                MyFrameTool.CreateDirectory();
                MyFrameTool.CreateObject();
                MyFrameTool.CreateStartJson();
                MyFrameTool.ChangeSceneName();
            } 
            else {
                Debug.LogError("Start Error,Please clean the project to Start");
            }
        }

        public static void CreateDirectory() {
            Directory.CreateDirectory(Application.dataPath + "/Excel");
            Directory.CreateDirectory(Application.dataPath + "/Json");
            Directory.CreateDirectory(Application.dataPath + "/Prefab");
            if (!File.Exists("Assets/AddressableAssetsData/AddressableAssetSettings.asset"))
            {
                AddressableAssetSettings.Create("Assets/AddressableAssetsData", "AddressableAssetSettings", true, true);
            }
            var setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
            setting.CreateGroup("MyFrame", false, false, false, null, new Type[0]);
        }

        public static void CreateObject() {
            GameObject myLoader = new GameObject();
            myLoader.name = "MyLoader";
            myLoader.AddComponent<MyLoader>();
            GameObject eventManager = new GameObject();
            eventManager.name = "EventManager";
            eventManager.AddComponent<EventManager>();
            GameObject objectManager = new GameObject();
            objectManager.name = "ObjectManager";
            objectManager.AddComponent<ObjectManager>();
            GameObject uiMain = new GameObject();
            uiMain.name = "UI_Main";
            uiMain.AddComponent<Canvas>();
            uiMain.AddComponent<CanvasScaler>();
            uiMain.AddComponent<GraphicRaycaster>();
            uiMain.AddComponent<UiElement>();
            GameObject eventSystem = new GameObject();
            eventSystem.name = "eventSystem";
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(uiMain.transform);
            PrefabUtility.SaveAsPrefabAsset(uiMain, "Assets/Prefab/UI_Main.prefab");
            DestroyImmediate(uiMain);
            var setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
            setting.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID("Assets/Prefab/UI_Main.prefab"), setting.FindGroup("MyFrame")).SetAddress("UI_Main");
        }

        public static void CreateStartJson() {
            StreamWriter sw = new StreamWriter(Application.dataPath + "/Json/MainResource.json");
            sw.WriteLine("{\"config\":[{\"ID\":\"UI_Main\",\"TYPE\":\"UI\"},{\"ID\":\"Start\",\"TYPE\":\"StartScene\"}],\"Type\":\"JsonArray\",\"Version\":\"1.0.0_2021 / 4 / 30 18:18:55\"}");
            sw.Close();
            var setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
            setting.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID("Assets/Json/MainResource.json"), setting.FindGroup("MyFrame")).SetAddress("MainResource");
        }
        
        public static void ChangeSceneName() {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/MyFrameLoading.unity");
            EditorSceneManager.SaveScene(EditorSceneManager.NewScene(NewSceneSetup.EmptyScene), "Assets/Scenes/StartScene.unity");
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[2] { new EditorBuildSettingsScene("Assets/Scenes/MyFrameLoading.unity", true), new EditorBuildSettingsScene("Assets/Scenes/StartScene.unity", true) };
            EditorBuildSettings.scenes = scenes;
            AssetDatabase.Refresh();
        }
    }
}