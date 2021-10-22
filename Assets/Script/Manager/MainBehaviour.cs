using MyFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviour : BaseBehaviour
{
    TableData _data;

    public override void Awake(BaseManager self)
    {
        this._listener.Add("Exit", this.Exit);
        this._listener.Add("CreatePanel", this.CreatePanel);
        base.Awake(self);
        this._data = (TableData)PlayerManager._instance.Get("TableData")[0];
    }

    void CreatePanel(params object[] param)
    {
        string name = (string)param[0];
        string content = (string)param[1];
        this._data.CreateTable(name, content);
    }

    void Exit(params object[] param)
    {
        Application.Quit();
    }
}
