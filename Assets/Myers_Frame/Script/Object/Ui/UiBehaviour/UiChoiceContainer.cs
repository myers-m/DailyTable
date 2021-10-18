using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MyObject("UiElement", "The behaviour make object become choicecontainer it will content some element to choice")]
public class UiChoiceContainer : BaseObjectBehaviour
{
    UiChoiceElement _current;

    public override void DoSomeThing(string tag, object[] param)
    {
        switch (tag) {
            case "SetCurrentChoice":
                this._current?.CloseChoice();
                this._current = (UiChoiceElement)param[0];
                this._current.InChoice();
                break;
        }
    }
}
