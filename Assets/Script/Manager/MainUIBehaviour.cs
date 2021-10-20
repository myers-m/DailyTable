using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFrame;
using UnityEngine.Events;

[My(BehaviourType.Ui,"StartScene","")]
public class MainUIBehaviour : BaseBehaviour
{
    UiManager _self;

    Animation _menuAnimation;
    bool _openMenu = true;

    public override void Awake(BaseManager self)
    {
        base.Awake(self);
        this._self = (UiManager)self;
    }

    public override void Start()
    {
        this._menuAnimation = this._self.GetElement("MenuPanel").GetComponent<Animation>();
    }

    public override object[] GetSomeThing(string tag, object[] param)
    {
        switch (tag)
        {
            case "GetEvent":
                UnityAction action = null;
                switch (param[0])
                {
                    case "TakeMenu":
                        action = () => { this.TakeMenu(); };
                        break;

                    case "OpenChildTable":
                        action = () => { this.OpenChildTable(); };
                        break;

                    case "Exit":
                        action = () => { EventManager._instance.DoEvent("Exit"); };
                        break;
                }
               return new object[] { action };
        }
        return base.GetSomeThing(tag, param);
    }

    void TakeMenu()
    {
        this._menuAnimation.Play(this._openMenu ? "CloseMenu" : "OpenMenu");
        this._openMenu = !this._openMenu;
    }

    void OpenChildTable()
    {

    }

    void OpenCratePanel()
    {

    }

    void CreatePanel()
    {

    }

    void RemovePanel()
    {

    }

    void SavePanel()
    {

    }

    void OpenExtendPanel()
    {

    }

    void CreateChildTable()
    {

    }

    void RemoveChildTable()
    {

    }

    void CreateItemTable()
    {

    }

    void RemoveItemTable()
    {

    }
}
