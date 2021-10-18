using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MyFrame {

    public class MyLoader : MonoBehaviour {
        public static MyLoader _instance;

        Dictionary<BehaviourType, List<Type>> _baseBehaviourList = new Dictionary<BehaviourType, List<Type>>();
        Dictionary<string, List<Type>> _objectBehaviourList = new Dictionary<string, List<Type>>();

        bool first = true;

        private void Awake() {
            MyLoader._instance = this;
            DontDestroyOnLoad(this.gameObject);
            this.InitList();
            this.LoadALLAssembly();
            this.CreateManager(BehaviourType.Data);
            this.CreateManager(BehaviourType.Player);
            this.CreateManager(BehaviourType.Game);
            EventManager._instance.RegisEvent("FinishLoading", this.CreateOther);
            EventManager._instance.RegisEvent("BeginChangeScene",this.ChangeScene);
        }

        void ChangeScene(object[] param) {
            string name = (string)param[0];
            this.RemoveBehaviour(name);
            this.AddBehaviour(name);
        }

        void RemoveBehaviour(string name) {
            this.DoRemoveBehaviour(GameManager._instance, name);
            this.DoRemoveBehaviour(DataManager._instance, name);
            this.DoRemoveBehaviour(ControlManager._instance, name);
            this.DoRemoveBehaviour(SceneManager._instance, name);
            this.DoRemoveBehaviour(UiManager._instance, name);
            this.DoRemoveBehaviour(PlayerManager._instance, name);
        }

        void AddBehaviour(string name) {
            this.DoAddBehaviour(GameManager._instance, name, BehaviourType.Game);
            this.DoAddBehaviour(DataManager._instance, name, BehaviourType.Data);
            this.DoAddBehaviour(PlayerManager._instance, name, BehaviourType.Player);
            this.DoAddBehaviour(ControlManager._instance, name, BehaviourType.Contol);
            this.DoAddBehaviour(SceneManager._instance, name, BehaviourType.Scene);
            this.DoAddBehaviour(UiManager._instance, name, BehaviourType.Ui);
        }

        void DoRemoveBehaviour(BaseManager manager, string name) {
            Type need = typeof(MyAttribute);
            List<IBaseBehaviour> _needRemove = new List<IBaseBehaviour>();
            foreach (KeyValuePair<string,IBaseBehaviour> element in manager._behaviourList) {
                MyAttribute attribute = (MyAttribute)element.Value.GetType().GetCustomAttribute(need);
                if (!attribute._scene.Contains("*") && !attribute._scene.Contains(name)) {
                    _needRemove.Add(element.Value);
                }
            }
            manager.RemoveBehaviour(_needRemove);
        }

        void DoAddBehaviour(BaseManager manager,string name,BehaviourType type) {
            Type need = typeof(MyAttribute);
            for (int i = 0; i < this._baseBehaviourList[type].Count; i++) {
                MyAttribute attribute = ((MyAttribute)this._baseBehaviourList[type][i].GetCustomAttribute(need));
                if (attribute._scene.Contains(name) && !manager._behaviourList.ContainsKey(this._baseBehaviourList[type][i].Name)) {
                    IBaseBehaviour behaviour = (IBaseBehaviour)this._baseBehaviourList[type][i].Assembly.CreateInstance(this._baseBehaviourList[type][i].FullName);
                    manager.JoinBehaviour(behaviour);
                }
            }
        }

        void CreateOther(object[] param) {
            this.CreateManager(BehaviourType.Contol);
            this.CreateManager(BehaviourType.Scene);
            this.CreateManager(BehaviourType.Ui);
            AddBehaviour("*");
            EventManager._instance.RemoveEvent("FinishLoading", this.CreateOther);
            GameManager._instance.LoadScene(DataManager._instance._assetList["Start"]);
        }

        void CreateManager(BehaviourType type) {
            GameObject snap;
            switch (type) {
                case BehaviourType.Game:
                    snap = new GameObject();
                    snap.name = "GameManager";
                    snap.AddComponent<GameManager>();
                    break;

                case BehaviourType.Data:
                    snap = new GameObject();
                    snap.name = "DataManager";
                    snap.AddComponent<DataManager>();
                    break;

                case BehaviourType.Player:
                    snap = new GameObject();
                    snap.name = "PlayerManager";
                    snap.AddComponent<PlayerManager>();
                    break;

                case BehaviourType.Contol:
                    snap = new GameObject();
                    snap.name = "ControlManager";
                    snap.AddComponent<ControlManager>();
                    break;

                case BehaviourType.Scene:
                    snap = new GameObject();
                    snap.name = "SceneManager";
                    snap.AddComponent<SceneManager>();
                    break;

                case BehaviourType.Ui:
                    snap = new GameObject();
                    snap.name = "UiManager";
                    snap.AddComponent<UiManager>();
                    break;
            }
        }

        void InitList() {
            foreach (BehaviourType e in Enum.GetValues(typeof(BehaviourType))) {
                this._baseBehaviourList.Add(e, new List<Type>());
            }
        }

        void LoadALLAssembly() {
            Type[] type = Assembly.GetExecutingAssembly().GetTypes();
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
                    } 
                    else {
                        List<Type> list = new List<Type>();
                        list.Add(type[i]);
                        this._objectBehaviourList.Add(result1._type, list);
                    }
                    continue;
                }
            }
        }

        public IBaseObjectBehaviour GetObjectBehaviour(string type, string name) {
            List<Type> list = this._objectBehaviourList[type];
            for (int i = 0; i < list.Count; i++) {
                if (list[i].Name == name) {
                    return (IBaseObjectBehaviour)list[i].Assembly.CreateInstance(list[i].FullName);
                }
            }
            return null;
        }
    }
}