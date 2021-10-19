using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame
{
    public class UiManager : BaseManager
    {
        public static UiManager _instance;

        Dictionary<string, UiElement> uiList = new Dictionary<string, UiElement>();

        protected override void Awake()
        {
            UiManager._instance = UiManager._instance == null ? this : UiManager._instance;
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            List<string> snap = new List<string>();
            snap.Add("*");
            this.OpenWindow("UI_Main", snap, _ =>
            {
                DontDestroyOnLoad(_.gameObject);
            });
        }

        public UiElement GetElement(string name) {
            return this.uiList[name];
        }

        public void OpenWindow(string name, List<string> scene = null, Action<UiElement> call = null)
        {
            if (!this.uiList.ContainsKey(name))
            {
                UiElement need = DataManager._instance._uiList[name];
                GameObject result = ObjectManager._instance.GetObject(need.gameObject);
                result.name = name;
                result.GetComponent<UiElement>().SetScene(scene != null ? scene : new List<string>());
                Vector3 position = ((RectTransform)result.transform).anchoredPosition;
                if (this.uiList.ContainsKey(need._level))
                {
                    result.transform.SetParent(this.uiList[need._level]._transform);
                }
                ((RectTransform)result.transform).anchoredPosition = position;
                call?.Invoke(result.GetComponent<UiElement>());
            }
            else
            {
                this.uiList[name].Init(null);
                call?.Invoke(this.uiList[name]);
            }
        }

        public void CloseWindow(string name, bool force = false)
        {
            this.uiList[name].Recover();
        }

        public void CloseWindow(UiElement element, bool force = false)
        {
            if (force || !this.uiList.ContainsKey(name))
            {
                ObjectManager._instance.RemoveObject(element.gameObject, force);
            }
            else
            {
                element.Recover();
            }
        }

        public void JoinElement(UiElement element)
        {
            this.uiList.Add(element.name, element);
        }

        public void RemoveElement(string name)
        {
            this.uiList.Remove(name);
        }
    }
}
