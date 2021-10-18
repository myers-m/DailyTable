using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyFrame {

    public class MyFrameManager : EditorWindow {
        Dictionary<BehaviourType, List<Type>> _baseBehaviourList = new Dictionary<BehaviourType, List<Type>>();
        Dictionary<string, List<Type>> _objectBehaviourList = new Dictionary<string, List<Type>>();

        Dictionary<BehaviourType, List<string>> _baseBehavious = new Dictionary<BehaviourType, List<string>>();
        Dictionary<string, Dictionary<string, List<string>>> _objectBehaviours = new Dictionary<string, Dictionary<string, List<string>>>();
        List<string> _foldName = new List<string>();
        List<bool> _foldValue = new List<bool>();

        bool _managerFold = false;
        bool _objectFold = false;

        Vector3 _scrollPos = Vector3.zero;

        int _length = 0;

        [MenuItem("MyFrame/Manager")]
        public static void OpenManager() {
            EditorWindow.GetWindow<MyFrameManager>(true);
        }

        private void Awake() {
            this.InitList();
            this.LoadALLAssembly();
            this.InitInfo();
            EditorSceneManager.activeSceneChanged += this.ChangeScene;
            this.autoRepaintOnSceneChange = true;
        }

        private void OnGUI() {
            GUIStyle style = new GUIStyle();
            style.fontSize = 100;
            style.normal.textColor = Color.yellow;
            EditorGUILayout.LabelField(EditorSceneManager.GetActiveScene().name, style);
            this._scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), this._scrollPos, new Rect(0, 0, 1000, 150 + this._length * 30));
            GUILayout.Space(100);
            if (this._managerFold = EditorGUILayout.Foldout(this._managerFold, "Managers")) {
                foreach (BehaviourType e in Enum.GetValues(typeof(BehaviourType))) {
                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.LabelField(e.ToString() + "Behaviour");
                    EditorGUI.indentLevel = 2;
                    for (int i = 0; i < this._baseBehavious[e].Count; i++) {
                        if (GUILayout.Button(this._baseBehavious[e][i])) {
                            this.OpenScript(this._baseBehavious[e][i]);
                        }
                    }
                }
            }
            EditorGUI.indentLevel = 0;
            if (this._objectFold = EditorGUILayout.Foldout(this._objectFold, "Objects")) {
                foreach (KeyValuePair<string, Dictionary<string, List<string>>> element in this._objectBehaviours) {
                    EditorGUI.indentLevel = 1;
                    if (this._foldValue[this._foldName.IndexOf(element.Key)] = EditorGUILayout.Foldout(this._foldValue[this._foldName.IndexOf(element.Key)], element.Key)) {
                        foreach (KeyValuePair<string, List<string>> element1 in element.Value) {
                            EditorGUI.indentLevel = 2;
                            if (this._foldValue[this._foldName.IndexOf(element.Key + "/" + element1.Key)] = EditorGUILayout.Foldout(this._foldValue[this._foldName.IndexOf(element.Key + "/" + element1.Key)], element1.Key)) {
                                for (int i = 0; i < element1.Value.Count; i++) {
                                    if (GUILayout.Button(element1.Value[i])) {
                                        this.OpenScript(element1.Value[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            GUI.EndScrollView();
        }

        private void OnDestroy() {
            EditorSceneManager.activeSceneChanged -= this.ChangeScene;
        }

        void LoadALLAssembly() {
            Type[] type = typeof(MyLoader).Assembly.GetTypes();
            Type managerAtt = typeof(MyAttribute);
            Type objectAtt = typeof(MyObjectAttribute);
            for (int i = 0; i < type.Length; i++) {
                MyAttribute result;
                if ((result = (MyAttribute)type[i].GetCustomAttribute(managerAtt)) != null) {
                    this._baseBehaviourList[result._type].Add(type[i]);
                    continue;
                }
                MyObjectAttribute result1;
                if ((result1 = (MyObjectAttribute)type[i].GetCustomAttribute(objectAtt)) != null) {
                    if (this._objectBehaviourList.ContainsKey(result1._type)) {
                        this._objectBehaviourList[result1._type].Add(type[i]);
                    } else {
                        List<Type> list = new List<Type>();
                        list.Add(type[i]);
                        this._objectBehaviourList.Add(result1._type, list);
                    }
                    continue;
                }
            }
        }

        void OpenScript(string name) {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets(name)[0]), 0);
        }

        void InitList() {
            foreach (BehaviourType e in Enum.GetValues(typeof(BehaviourType))) {
                this._baseBehaviourList.Add(e, new List<Type>());
                this._baseBehavious.Add(e, new List<string>());
            }
            this._objectBehaviourList.Clear();
            this._objectBehaviours.Clear();
            this._foldName.Clear();
            this._foldValue.Clear();
            this._length = 0;
        }

        void InitInfo() {
            Scene scene = EditorSceneManager.GetActiveScene();
            string sceneName = scene.name;
            foreach (KeyValuePair<BehaviourType, List<Type>> element in this._baseBehaviourList) {
                for (int i = 0; i < element.Value.Count; i++) {
                    List<string> scenes = ((MyAttribute)element.Value[i].GetCustomAttribute(typeof(MyAttribute)))._scene;
                    if (scenes.Contains(sceneName) || scenes.Contains("*")) {
                        this._baseBehavious[element.Key].Add(element.Value[i].Name);
                        this._length += 2;
                    }
                }
            }
            Dictionary<string, BaseObject[]> dic = new Dictionary<string, BaseObject[]>();
            {
                List<GameObject> all = new List<GameObject>();
                GameObject[] objects = scene.GetRootGameObjects();
                for (int i = 0; i < objects.Length; i++) {
                    this.JoinObject(objects[i], all);
                }
                for (int i = 0; i < all.Count; i++) {
                    if (all[i].GetComponent<BaseObject>()) {
                        dic.Add(all[i].name, all[i].GetComponents<BaseObject>());
                    }
                }
            }
            foreach (KeyValuePair<string, BaseObject[]> element in dic) {
                this._objectBehaviours.Add(element.Key, new Dictionary<string, List<string>>());
                this.JoinFold(element.Key);
                for (int i = 0; i < element.Value.Length; i++) {
                    this._objectBehaviours[element.Key].Add(element.Value[i].GetType().Name, new List<string>());
                    this.JoinFold(element.Key + "/" + element.Value[i].GetType().Name);
                    foreach (KeyValuePair<string, IBaseObjectBehaviour> childElement in element.Value[i]._behaviourList) {
                        this._objectBehaviours[element.Key][element.Value[i].GetType().Name].Add(childElement.Key);
                        this._length += 4;
                    }
                }
            }
        }

        void JoinFold(string name) {
            this._foldName.Add(name);
            this._foldValue.Add(false);
        }

        void ChangeScene(Scene current, Scene next) {
            this.InitList();
            this.LoadALLAssembly();
            this.InitInfo();
        }

        void JoinObject(GameObject obj, List<GameObject> list) {
            list.Add(obj);
            for (int i = 0; i < obj.transform.childCount; i++) {
                this.JoinObject(obj.transform.GetChild(i).gameObject, list);
            }
        }
    }
}