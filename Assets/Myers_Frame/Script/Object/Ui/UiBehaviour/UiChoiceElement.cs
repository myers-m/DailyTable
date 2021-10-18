using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[MyObject("UiElement", "The behaviour make object become containerelement it will effect other element white choice")]
public class UiChoiceElement : BaseObjectBehaviour
{
    UiElement _self;
    Button _button;

    [SerializeField]
    public string _parent = "";
    [SerializeField]
    public string _inChoiceEvent = "";
    [SerializeField]
    public string _goalUi = "";
    [SerializeField]
    public bool _main = false;

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this._self = (UiElement)self;
    }

    public override void Start()
    {
        base.Start();
        UiElement parent = MyFrame.UiManager._instance.GetElement(this._parent);
        this._button = this._self.GetComponent<Button>();
        this._button.onClick.AddListener(() => { parent.Do("SetCurrentChoice", this); });
        if (this._main) parent.Do("SetCurrentChoice", this);
    }

    public void InChoice()
    {
        switch (this._inChoiceEvent)
        {
            case "Open":
                MyFrame.UiManager._instance.OpenWindow(this._goalUi);
                break;

            case "Close":
                MyFrame.UiManager._instance.CloseWindow(this._goalUi);
                break;

            default:
                MyFrame.UiManager._instance.Do("InChoice", this._inChoiceEvent);
                break;
        }
        this._button.interactable = false;
    }

    public void CloseChoice()
    {
        switch (this._inChoiceEvent)
        {
            case "Close":
                MyFrame.UiManager._instance.OpenWindow(this._goalUi);
                break;

            case "Open":
                MyFrame.UiManager._instance.CloseWindow(this._goalUi);
                break;

            default:
                MyFrame.UiManager._instance.Do("CloseChoice", this._inChoiceEvent);
                break;
        }
        this._button.interactable = true;
    }
}
