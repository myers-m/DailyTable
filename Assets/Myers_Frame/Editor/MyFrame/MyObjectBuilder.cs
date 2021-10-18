using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyFrame {
    [CustomEditor(typeof(BaseObject),true)]
    public class MyObjectBuilder : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set Behaviour")) {
                EditorWindow.GetWindow<MyBehaviourPanel>(true).SetCurrentType((BaseObject)target);
            }
            if (GUILayout.Button("Remove Behaviour")) {
                
            }
        }
    }
}