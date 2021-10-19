using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {
    public class EventManager : MonoBehaviour {
        public static EventManager _instance;

        Dictionary<string, List<Action<object[]>>> _eventList = new Dictionary<string, List<Action<object[]>>>();

        private void Awake() {
            EventManager._instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void RegisEvent(string tag, Action<object[]> call) {
            if (this._eventList.ContainsKey(tag)) {
                this._eventList[tag].Add(call);
                return;
            }
            List<Action<object[]>> list = new List<Action<object[]>>();
            list.Add(call);
            this._eventList.Add(tag, list);
        }

        public void DoEvent(string tag, params object[] param) {
            List<Action<object[]>> call;
            if (this._eventList.TryGetValue(tag, out call)) {
                for (int i = 0; i < call.Count; i++) {
                    call[i].Invoke(param);
                }
                return;
            }
            Debug.LogError(tag + " 事件集不存在");
        }

        public void RemoveEvent(string tag, Action<object[]> call) {
            if (this._eventList.ContainsKey(tag)) {
                this._eventList[tag].Remove(call);
                if (this._eventList[tag].Count == 0) {
                    this._eventList.Remove(tag);
                }
                return;
            }
            Debug.LogError(tag + " 事件集不存在");
        }
    }
}
