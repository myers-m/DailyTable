using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBaseBehaviour : BaseObjectBehaviour 
{
    protected UiElement _self;

    public override void Awake(BaseObject self) {
        this._self = (UiElement)self;
        base.Awake(self);
    }

    public void SetTexts(params string[] texts) {
        Text[] needs = this._self.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++) {
            needs[i].text = texts[i];
        }
    }

    public void SetImages(params string[] sprites) {
        Image[] images = this._self.GetComponentsInChildren<Image>();
        for (int i = 0; i < sprites.Length; i++) {
            images[i].sprite = MyFrame.DataManager._instance._spriteList[sprites[i]];
        }
    }

    public void SetSprites(params Sprite[] sprites) {
        Image[] images = this._self.GetComponentsInChildren<Image>();
        for (int i = 0; i < sprites.Length; i++) {
            images[i].sprite = sprites[i];
        }
    }

    public void SetAction(params UnityAction[] actions) {
        Button[] buttons = this._self.GetComponentsInChildren<Button>();
        for (int i = 0; i < actions.Length; i++) {
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(actions[i]);
        }
    }

    public void SetActives(params string[] actives) {
        for (int i = 0; i < actives.Length; i++) {
            this._self._transform.Find(actives[i]).gameObject.SetActive(true);
        }
    }

    public void SetUnActives(params string[] unActives) {
        for (int i = 0; i < unActives.Length; i++) {
            this._self._transform.Find(unActives[i]).gameObject.SetActive(false);
        }
    }

    public override void DoSomeThing(string tag, object[] param) {
        switch (tag) {
            case "_text":
                string[] updateTexts = ((string)param[0]).Split('|');
                this.SetTexts(updateTexts);
                break;

            case "_sprite":
                string[] updateSprites = ((string)param[0]).Split('|');
                this.SetImages(updateSprites);
                break;

            case "_active":
                string[] actives = ((string)param[0]).Split('|');
                this.SetActives(actives);
                break;

            case "_unActive":
                string[] unActives = ((string)param[0]).Split('|');
                this.SetUnActives(unActives);
                break;
        }
    }
}
