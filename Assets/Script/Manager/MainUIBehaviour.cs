using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFrame;
using UnityEngine.Events;

[My(BehaviourType.Ui,"StartScene","")]
public class MainUIBehaviour : BaseBehaviour
{
    bool _openMenu = true;

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
                }
               return new object[] { action };
        }
        return base.GetSomeThing(tag, param);
    }

    void TakeMenu()
    {
        if (this._openMenu)
        {
            //obj.GetComponent<Animation>().Play("OpenMenu");
        }
        else
        {
            //obj.GetComponent<Animation>().Play("CloseMenu");
        }
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
