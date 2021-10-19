using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[MyObject("UiElement", "Make button get the event or control someone window")]
public class UiButtonBehaviour : UiBaseBehaviour
{
    [SerializeField]
    public string _buttonEvent = "";
    [SerializeField]
    public string _goalUi = "";

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this.SetEvent();
    }

    void SetEvent()
    {
        if (this._buttonEvent != "")
        {
            switch (this._buttonEvent)
            {
                case "Open":
                    this.SetAction(() => {
                        MyFrame.UiManager._instance.OpenWindow(this._goalUi);
                    });
                    break;

                case "Close":
                    this.SetAction(()=> {
                        MyFrame.UiManager._instance.CloseWindow(this._goalUi);
                    });
                    break;

                default:
                    this.SetAction((UnityEngine.Events.UnityAction)MyFrame.UiManager._instance.Get("GetEvent", this._buttonEvent)[0]);
                    break;
            }
        }
    }

    public override void DoSomeThing(string tag, object[] param) {
        switch (tag) {
            case "_buttonEvent":
                this._buttonEvent = (string)param[0];
                this.SetEvent();
                return;
        }
        base.DoSomeThing(tag, param);
    }
}
