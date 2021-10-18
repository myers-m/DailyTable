using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MyObject("UiElement", "The behaviour make object become container")]
public class UiContent : BaseObjectBehaviour
{
    UiElement _self;

    Dictionary<string, UiElement> _childList = new Dictionary<string, UiElement>();

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this._self = (UiElement)self;
    }

    public void AddElement(Dictionary<string,object> list, Action<string, UiElement> call = null) {
        string prefab = (string)list["Prefab"];
        List<string> names = (List<string>)list["ContentName"];
        List<string> texts = list.ContainsKey("ContentText") ? (List<string>)list["ContentText"] : null;
        List<string> sprites = list.ContainsKey("ContentSprite") ? (List<string>)list["ContentSprite"] : null;
        List<string> actions = list.ContainsKey("ContentAction") ? (List<string>)list["ContentAction"] : null;
        List<string> actives = list.ContainsKey("ContentActive") ? (List<string>)list["ContentActive"] : null;
        List<string> unActives = list.ContainsKey("ContentUnActive") ? (List<string>)list["ContentUnActive"] : null;
        List<object[]> others = list.ContainsKey("ContentOther") ? (List<object[]>)list["ContentOther"] : null;
        for (int i = 0; i < names.Count; i++) {
            MyFrame.UiManager._instance.OpenWindow(prefab, null, (element) =>
            {
                this._childList.Add(names[i], element);
                if (texts != null && texts[i] != "") {
                    element.Do("_text", texts[i]);
                }
                if (sprites != null && sprites[i] != "") {
                    element.Do("_sprite", sprites[i]);
                }
                if (actions != null && actions[i] != "") {
                    element.Do("_buttonEvent", actions[i]);
                }
                if (actives != null) {
                    element.Do("_active", actives[i]);
                }
                if (unActives != null) {
                    element.Do("_unActive", unActives[i]);
                }
                if (others != null) {
                    element.Do("_other", others[i]);
                }
                element._transform.SetParent(this._self._transform);
                call?.Invoke(names[i], element);
            });
        }
    }

    public void SetElement(Dictionary<string, object> list, Action<string,UiElement> call = null) {
        List<string> names = (List<string>)list["ContentName"];
        List<string> texts = list.ContainsKey("ContentText") ? (List<string>)list["ContentText"] : null;
        List<string> sprites = list.ContainsKey("ContentSprite") ? (List<string>)list["ContentSprite"] : null;
        List<string> actions = list.ContainsKey("ContentAction") ? (List<string>)list["ContentAction"] : null;
        List<string> actives = list.ContainsKey("ContentActive") ? (List<string>)list["ContentActive"] : null;
        List<string> unActives = list.ContainsKey("ContentUnActive") ? (List<string>)list["ContentUnActive"] : null;
        List<object[]> others = list.ContainsKey("ContentOther") ? (List<object[]>)list["ContentOther"] : null;
        for (int i = 0; i < names.Count; i++) {
            UiElement element = this._childList[names[i]];
            if (texts != null && texts[i] != "") {
                element.Do("_text", texts[i]);
            }
            if (sprites != null && sprites[i] != "") {
                element.Do("_sprite", sprites[i]);
            }
            if (actions != null && actions[i] != "") {
                element.Do("_buttonEvent", actions[i]);
            }
            if (actives != null) {
                element.Do("_active", actives[i]);
            }
            if (unActives != null) {
                element.Do("_unActive", unActives[i]);
            }
            if (others != null) {
                element.Do("_other", others[i]);
            }
            call?.Invoke(names[i], element);
        }
    }

    public void RemoveElement(Dictionary<string, object> list) {
        List<string> names = (List<string>)list["ContentName"];
        for (int i = 0; i < names.Count; i++) {
            MyFrame.UiManager._instance.CloseWindow(this._childList[names[i]]);
            this._childList.Remove(names[i]);
        }
    }

    public void ClearElement() {
        foreach (KeyValuePair<string, UiElement> element in this._childList) {
            MyFrame.UiManager._instance.CloseWindow(element.Value);
        }
        this._childList.Clear();
    }

    public override void DoSomeThing(string tag, object[] param)
    {
        switch (tag)
        {
            case "AddElement":
                this.AddElement((Dictionary<string, object>)param[0]);
                break;

            case "SetElement":
                this.SetElement((Dictionary<string, object>)param[0]);
                break;

            case "RemoveElement":
                this.RemoveElement((Dictionary<string, object>)param[0]);
                break;

            case "ClearElement":
                this.ClearElement();
                break;
        }
    }
}
