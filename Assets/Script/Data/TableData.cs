using MyFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[My(BehaviourType.Player,"StartScene","")]
public class TableData : BasePlayerBehaviour
{
    public ParentTable _mainTable = new ParentTable();

    public override void Awake(BaseManager self)
    {
        this._self = (PlayerManager)self;
        this._path = "Table";
        this._name = PlayerPrefs.GetString("LastTable","¡Ÿ ±");
        this._self.LoadData(this._path, this._name, this);
    }

    public void Save()
    {
        this._self.SaveData<TableData>(this._path, this._name, this);
    }

    public void CreateTable(string name, string content)
    {
        this._name = name;
        this._mainTable = new ParentTable();
        this._mainTable._content = content;
    }
}
