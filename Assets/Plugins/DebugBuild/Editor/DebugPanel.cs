using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyTool {
    public class DebugPanel : EditorWindow {
        DebugAssets _assets;
        List<DebugTag> _tags = new List<DebugTag>();
        List<bool> _values = new List<bool>();

        Vector2 _local = Vector2.zero;

        private void Awake() {
            this._assets = (DebugAssets)Resources.Load("MyPrint/DebugAssets");
            this.InitTags();
        }

        private void OnProjectChange() {
            this.InitTags();
            this._local = Vector2.zero;
        }

        private void OnGUI() {
            this._local = EditorGUILayout.BeginScrollView(this._local);
            int index = 0;
            foreach (DebugTag element in Enum.GetValues(typeof(DebugTag))) {
                this._values[index] = EditorGUILayout.Toggle(element.ToString(), this._values[index]);
                index++;
            }
            EditorGUILayout.EndScrollView();
            this.ChangeList();
        }

        void InitTags() {
            this._tags.Clear();
            this._values.Clear();
            foreach (DebugTag element in Enum.GetValues(typeof(DebugTag))) {
                this._tags.Add(element);
                this._values.Add(false);
            }
            for (int i = 0; i < this._assets._tags.Count; i++) {
                int index = this._tags.IndexOf(this._assets._tags[i]);
                this._values[index] = true;
            }
        }

        void ChangeList() {
            this._assets._tags.Clear();
            for (int i = 0; i < this._values.Count; i++) {
                if (this._values[i]) {
                    this._assets._tags.Add(this._tags[i]);
                }
            }
        }

        [MenuItem("MyTool/DebugPanel")]
        public static void OpenManager() {
            EditorWindow.GetWindow<DebugPanel>(true);
        }
    }
}