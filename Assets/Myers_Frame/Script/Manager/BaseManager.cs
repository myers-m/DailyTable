using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace MyFrame {
    public class BaseManager : MonoBehaviour {
        public Dictionary<string, IBaseBehaviour> _behaviourList = new Dictionary<string, IBaseBehaviour>();
        public Action _updateAction;

        protected virtual void Awake() {
            DontDestroyOnLoad(this.gameObject);
            EventManager._instance.RegisEvent("FinishLoad",this.StartBehaviour);
        }

        protected virtual void Start() {

        }

        protected virtual void Update() {
            this._updateAction?.Invoke();
        }

        protected virtual void StartBehaviour(object[] param) {
            foreach (KeyValuePair<string,IBaseBehaviour> element in this._behaviourList) {
                if (!element.Value.isStart) {
                    element.Value.Start();
                    element.Value.isStart = true;
                }
            }
        }

        public virtual void JoinBehaviour(IBaseBehaviour behaviour) {
            this._behaviourList.Add(behaviour.GetType().Name, behaviour);
            behaviour.Awake(this);
            MyAttribute attribute = (MyAttribute)behaviour.GetType().GetCustomAttribute(typeof(MyAttribute));
            if (attribute._resource != "") {
                DataManager._instance.LoadResource(attribute._resource);
            }
        }

        public virtual void RemoveBehaviour(List<IBaseBehaviour> behaviourList) {
            for (int i = 0; i < behaviourList.Count; i++) {
                behaviourList[i].OnDestroy();
                this._behaviourList.Remove(behaviourList[i].GetType().Name);
                MyAttribute attribute = (MyAttribute)behaviourList[i].GetType().GetCustomAttribute(typeof(MyAttribute));
                if (attribute._resource != "") {
                    DataManager._instance.RemoveResource(attribute._resource);
                }
            }
        }

        private void OnDestroy() {
            foreach (KeyValuePair<string, IBaseBehaviour> element in this._behaviourList) {
                element.Value.OnDestroy();
            }
        }

        public virtual void Do(string tag, params object[] objects) {
            foreach (KeyValuePair<string, IBaseBehaviour> element in this._behaviourList) {
                element.Value.DoSomeThing(tag, objects);
            }
        }

        public virtual object[] Get(string tag, params object[] objects) {
            foreach (KeyValuePair<string, IBaseBehaviour> element in this._behaviourList) {
                object[] result = element.Value.GetSomeThing(tag, objects);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }
    }
}