using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAttribute : Attribute
{
    public BehaviourType _type;
    public List<string> _scene = new List<string>();
    public string _resource;

    public MyAttribute(BehaviourType type,string scene,string resource) {
        this._type = type;
        this._resource = resource;
        string[] list = scene.Split(',');
        for (int i = 0; i < list.Length; i++) {
            this._scene.Add(list[i]);
        }
    }

}

public enum BehaviourType {
    Game,
    Contol,
    Data,
    Scene,
    Ui,
    Player,
}
