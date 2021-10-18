using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MyObject("UiElement", "The behaviour control object's text")]
public class UiText : BaseObjectBehaviour
{
    UiElement _self;
    UnityEngine.UI.Text _text;

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this._self = (UiElement)self;
        this._text = this._self.GetComponent<UnityEngine.UI.Text>();
    }

    public override void DoSomeThing(string tag, object[] param)
    {
        switch (tag) {
            case "SetText":
                this._text.text = (string)param[0];
                break;
        }
    }
}
