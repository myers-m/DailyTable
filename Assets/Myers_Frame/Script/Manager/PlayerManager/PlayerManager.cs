using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyFrame{
    public class PlayerManager : BaseManager {
        public static PlayerManager _instance;

        protected override void Awake() {
            PlayerManager._instance = PlayerManager._instance == null ? this : PlayerManager._instance;
            base.Awake();
        }

        internal List<T> LoadAllJsonData<T>(string path) {
            string filePath = Application.persistentDataPath + path;
            List<T> res = new List<T>();
            if (File.Exists(filePath)) {
                DirectoryInfo root = new DirectoryInfo(filePath);
                FileInfo[] dics = root.GetFiles();
                foreach (FileInfo info in dics)
                {
                    StreamReader reader = info.OpenText();
                    res.Add(JsonUtility.FromJson<T>(reader.ReadToEnd()));
                    reader.Close();
                }
            }
            return res;
        }

        internal void LoadData<T>(string path, string name, T content)where T : BasePlayerBehaviour{
            switch (path) {
                case "PlayerPref":
                    content = JsonUtility.FromJson<T>(PlayerPrefs.GetString(name, JsonUtility.ToJson(content)));
                    break;

                default:
                    content = JsonUtility.FromJson<T>(this.Load(path, name, JsonUtility.ToJson(content)));
                    break;
            }
        }

        internal void SaveData<T>(string path, string name, T content) {
            switch (path) {
                case "PlayerPref":
                    PlayerPrefs.SetString(name, JsonUtility.ToJson(content));
                    break;

                default:
                    this.Save(path, name, JsonUtility.ToJson(content));
                    break;
            }
        }

        public override void JoinBehaviour(IBaseBehaviour behaviour) {
            behaviour.Awake(this);
            this._behaviourList.Add(((BasePlayerBehaviour)behaviour).GetType().Name, behaviour);
        }

        public override void RemoveBehaviour(List<IBaseBehaviour> behaviourList) {
            for (int i = 0; i < behaviourList.Count; i++) {
                behaviourList[i].OnDestroy();
                this._behaviourList.Remove(((BasePlayerBehaviour)behaviourList[i]).GetType().Name);
            }
        }

        public override object[] Get(string tag, params object[] objects) {
            return new object[] { this._behaviourList[tag] };
        }

        string Load(string path, string name, string defaultRes) {
            string res = defaultRes;
            string fileName = Application.persistentDataPath + path + name;
            if (File.Exists(fileName)) {
                StreamReader reader = new StreamReader(fileName);
                res = reader.ReadToEnd();
                reader.Close();
            }
            return res;
        }

        void Save(string path, string name, string content) {
            string fileName = Application.persistentDataPath + path;
            if (!File.Exists(fileName)) {
                Directory.CreateDirectory(fileName);
            }
            fileName += name;
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(content);
            writer.Close();
        }
    }
}