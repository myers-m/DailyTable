using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseObject : MonoBehaviour {
    public Transform _transform;
    public Action _updateAction;
    public Action _fixedUpdateAction;
    public Action _lateUpdateAction;
    [ReadOnly]
    public SerializableDictionary<string, IBaseObjectBehaviour> _behaviourList = new SerializableDictionary<string, IBaseObjectBehaviour>();

    protected virtual void Awake() {
        this._transform = this.transform;
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList)
        {
            element.Value.Awake(this);
        }
        MyFrame.EventManager._instance.RegisEvent("BeginChangeScene", this.DoDestroy);
    }

    public virtual void Do(string tag, params object[] objects)
    {
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList)
        {
            element.Value.DoSomeThing(tag, objects);
        }
    }

    protected virtual void OnEnable() {
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList) {
            element.Value.OnEnable();
        }
    }

    protected virtual void FixedUpdate() {
        this._fixedUpdateAction?.Invoke();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList)
        {
            if (!element.Value.isStart)
            {
                element.Value.Start();
                element.Value.isStart = true;
            }
        }
        this._updateAction?.Invoke();
    }

    protected virtual void LateUpdate() {
        this._lateUpdateAction?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList)
        {
            element.Value.OnDestroy();
        }
        MyFrame.EventManager._instance.RemoveEvent("BeginChangeScene", this.DoDestroy);
    }

    public virtual void JoinBehaviour(IBaseObjectBehaviour behaviour) {
        this._behaviourList.Add(behaviour.GetType().Name, behaviour);
        behaviour.Awake(this);
    }

    public virtual System.Object[] Get(string tag, params System.Object[] objects)
    {
        foreach (KeyValuePair<string, IBaseObjectBehaviour> element in this._behaviourList)
        {
            System.Object[] result = element.Value.GetSomeThing(tag, objects);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    protected virtual void DoDestroy(object[] param) {
        Destroy(this.gameObject);
    }
}
