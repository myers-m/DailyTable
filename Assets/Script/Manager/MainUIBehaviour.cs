using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFrame;
using UnityEngine.Events;
using UnityEngine.UI;

[My(BehaviourType.Ui,"StartScene","")]
public class MainUIBehaviour : BaseBehaviour
{
    Text _createPanelName;
    Text _createPanelContent;
    UiManager _self;
    TableData _data;

    Animation _menuAnimation;
    bool _openMenu = true;

    public override void Awake(BaseManager self)
    {
        base.Awake(self);
        this._self = (UiManager)self;
        this._data = (TableData)PlayerManager._instance.Get("TableData")[0];
    }

    public override void Start()
    {
        base.Start();
        this._menuAnimation = this._self.GetElement("MenuPanel").GetComponent<Animation>();
        this.InitPanel();
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
                        action = this.TakeMenu;
                        break;

                    case "CreatePanel":
                        action = this.CreatePanel;
                        break;

                    case "Exit":
                        action = () => { EventManager._instance.DoEvent("Exit"); };
                        break;
                }
               return new object[] { action };
        }
        return base.GetSomeThing(tag, param);
    }

    void InitPanel()
    {
        this._self.GetElement("PanelName").Do("Text", this._data._name);
        this._self.GetElement("PanelContent").Do("Text", this._data._mainTable._content);
        //this._self.GetElement("ChildTableContent")
    }

    void TakeMenu()
    {
        this._menuAnimation.Play(this._openMenu ? "CloseMenu" : "OpenMenu");
        this._openMenu = !this._openMenu;
    }

    void CreatePanel()
    {
        Text text = this._createPanelName == null ? this._createPanelName = (Text)this._self.GetElement("CreatePanelName").Get("Text")[0] : this._createPanelName;
        Text content = this._createPanelContent == null ? this._createPanelContent = (Text)this._self.GetElement("CreatePanelContent").Get("Text")[0] : this._createPanelContent;
        string panelName = text.name;
        string panelContent = content.name;
        this._self.CloseWindow("CreatePanel");
        EventManager._instance.DoEvent("CreatePanel", panelName, panelContent);
        this.InitPanel();
    }

    void RemovePanel()
    {

    }

    void SavePanel()
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
