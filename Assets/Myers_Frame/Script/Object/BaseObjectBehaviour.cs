using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using MyFrame;

[System.Serializable]
public class BaseObjectBehaviour : IBaseObjectBehaviour
{
    protected Dictionary<string, Action<object[]>> _listener = new Dictionary<string, Action<object[]>>();

    public bool isStart { get ; set ; }

    public virtual void Awake(BaseObject self)
    {
        foreach (KeyValuePair<string, Action<object[]>> element in this._listener)
        {
            EventManager._instance.RegisEvent(element.Key, element.Value);
        }
    }

    public virtual void DoSomeThing(string tag, object[] param)
    {

    }

    public virtual object[] GetSomeThing(string tag, object[] param)
    {
        return null;
    }

    public virtual void OnDestroy()
    {
        foreach (KeyValuePair<string, Action<object[]>> element in this._listener)
        {
            EventManager._instance.RemoveEvent(element.Key, element.Value);
        }
        this._listener.Clear();
    }

    public virtual void OnEnable() {

    }

    public virtual void Start() {

    }
}