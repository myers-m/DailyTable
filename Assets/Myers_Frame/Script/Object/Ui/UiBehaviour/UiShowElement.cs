using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MyObject("UiTriggerElement", "The behaviour show something while it has choose")]
public class UiShowElement : UiBaseBehaviour
{
    [SerializeField]
    public string _goalUi = "";
    /// <summary>
    /// type_param
    /// </summary>
    [SerializeField]
    public string _doIt = "";

    void OnPointerEnter()
    {
        if (this._doIt != "")
        {
            MyFrame.UiManager._instance.OpenWindow(this._goalUi,null,(uiElement)=> {
                MyFrame.UiManager._instance.Do("OnPointerEnter", this._doIt, uiElement._behaviourList["UiShowPanel"]);
            });
        }
    }

    void OnPointerExit()
    {
        MyFrame.UiManager._instance.CloseWindow(this._goalUi);
    }

    public override void DoSomeThing(string tag, object[] param)
    {
        switch (tag) {
            case "OnPointerEnter":
                this.OnPointerEnter();
                return;

            case "OnPointerExit":
                this.OnPointerExit();
                return;

            case "_other":
                this._doIt = (string)param[0];
                return;
        }
        base.DoSomeThing(tag, param);
    }
}
