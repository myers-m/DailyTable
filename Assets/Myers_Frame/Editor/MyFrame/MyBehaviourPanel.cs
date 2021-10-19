using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MyBehaviourPanel : EditorWindow
{
    BaseObject _current;
    List<string> _typeNameList = new List<string>();
    List<Type> _typeList = new List<Type>();
    List<MyObjectAttribute> _contentList = new List<MyObjectAttribute>();
    Dictionary<string, object> _fieldList = new Dictionary<string, object>();
    Dictionary<string, string> _fieldType = new Dictionary<string, string>();

    int _index = 0;

    public void SetCurrentType(BaseObject current) {
        this._current = current;
        this.titleContent.text = this._current.GetType().Name;
        this.LoadAllBehaviour();
    }

    void LoadAllBehaviour() {
        Type[] types = this._current.GetType().Assembly.GetTypes();
        Type need = typeof(MyObjectAttribute);
        MyObjectAttribute res;
        for (int i = 0; i < types.Length; i++) {
            if ((res = (MyObjectAttribute)types[i].GetCustomAttribute(need)) != null)
            {
                if (this._current.GetType().IsSubclassOf(this._current.GetType().Assembly.GetType(res._type)) || res._type == this.titleContent.text) {
                    this._typeNameList.Add(types[i].Name);
                    this._typeList.Add(types[i]);
                    this._contentList.Add(res);
                }
            }
        }
        this.CreateField();
    }

    void CreateField() {
        FieldInfo[] fields = this._typeList[this._index].GetFields();
        SerializeField att;
        this._fieldList.Clear();
        this._fieldType.Clear();
        for (int i = 0; i < fields.Length; i++)
        {
            if ((att = (SerializeField)fields[i].GetCustomAttribute(typeof(SerializeField))) != null)
            {
                switch (fields[i].FieldType.Name)
                {
                    case "String":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : "");
                        break;

                    case "Int32":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : 0);
                        break;

                    case "Single":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : 0f);
                        break;

                    case "Vector2":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : Vector2.zero);
                        break;

                    case "Vector3":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : Vector3.zero);
                        break;

                    case "Boolean":
                        this._fieldList.Add(fields[i].Name, this._current._behaviourList.ContainsKey(this._typeList[this._index].Name) ? fields[i].GetValue(this._current._behaviourList[this._typeList[this._index].Name]) : false);
                        break;
                }
                this._fieldType.Add(fields[i].Name, fields[i].FieldType.Name);
            }
        }
    }

    private void OnGUI() {
        int index = EditorGUILayout.Popup("Behaviour", this._index, this._typeNameList.ToArray());
        if (index != this._index) {
            this._index = index;
            this.CreateField();
        }
        GUILayout.Label(this._contentList[this._index]._content);
        Dictionary<string, object> param = new Dictionary<string, object>();
        foreach (KeyValuePair<string,object> element in this._fieldList)
        {
            switch (this._fieldType[element.Key]) {
                case "String":
                    param.Add(element.Key, EditorGUILayout.TextField(element.Key, (string)element.Value));
                    break;

                case "Int32":
                    param.Add(element.Key, EditorGUILayout.IntField(element.Key, (int)element.Value));
                    break;

                case "Single":
                    param.Add(element.Key, EditorGUILayout.FloatField(element.Key, (float)element.Value));
                    break;

                case "Vector2":
                    param.Add(element.Key, EditorGUILayout.Vector2Field(element.Key, (Vector2)element.Value));
                    break;

                case "Vector3":
                    param.Add(element.Key, EditorGUILayout.Vector3Field(element.Key, (Vector3)element.Value));
                    break;

                case "Boolean":
                    param.Add(element.Key, EditorGUILayout.Toggle(element.Key, (bool)element.Value));
                    break;
            }
        }
        foreach (KeyValuePair<string, object> element in param) {
            this._fieldList[element.Key] = element.Value;
        }
        if (GUILayout.Button("Do Join Behaviour")) {
            object res = this._current._behaviourList.ContainsKey(this._typeNameList[this._index]) ? this._current._behaviourList[this._typeNameList[this._index]] : this._current.GetType().Assembly.CreateInstance(this._typeNameList[this._index]);
            foreach (KeyValuePair<string, object> element in this._fieldList)
            {
                this._typeList[this._index].GetField(element.Key).SetValue(res, element.Value);
            }
            this._current._behaviourList.SerializeAdd(this._typeNameList[this._index], (BaseObjectBehaviour)res);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            this.Close();
        }
    }
}
