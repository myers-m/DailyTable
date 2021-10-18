using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {
    public class BaseBehaviour : IBaseBehaviour {
        protected Dictionary<string, Action<object[]>> _listener = new Dictionary<string, Action<object[]>>();

        public bool isStart { get ; set ; }

        public virtual void Awake(BaseManager self) {
            foreach (KeyValuePair<string, Action<object[]>> element in this._listener) {
                EventManager._instance.RegisEvent(element.Key, element.Value);
            }
        }

        public virtual void DoSomeThing(string tag, object[] param) {

        }

        public virtual object[] GetSomeThing(string tag, object[] param) {
            return null;
        }

        public virtual void OnDestroy()
        {
            foreach (KeyValuePair<string, Action<object[]>> element in this._listener)
            {
                EventManager._instance.RemoveEvent(element.Key, element.Value);
            }
            this._listener.Clear();
        }

        public virtual void Start() {

        }
    }
}