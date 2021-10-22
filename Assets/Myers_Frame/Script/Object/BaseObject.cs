using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BaseObject : MonoBehaviour {
    public Transform _transform;
    public Action _updateAction;
    public Action _fixedUpdateAction;
    public Action _lateUpdateAction;
    [SerializeField]
    public SerializableDic<string, ObjectParam> _serialDic = new SerializableDic<string, ObjectParam>();
    [NonSerialized]
    public Dictionary<string, object> _dic = new Dictionary<string, object>();
    [ReadOnly]
    public SerializableDictionary<string, IBaseObjectBehaviour> _behaviourList = new SerializableDictionary<string, IBaseObjectBehaviour>();

    protected virtual void Awake() {
        this._transform = this.transform;
        #region 序列化字段
        foreach (var dic in this._serialDic)
        {
            Transform element = this._transform.Find(dic.Key);
            object behaviour = null;
            switch (dic.Value)
            {
                case ObjectParam.Text:
                    behaviour = element.GetComponent<Text>();
                    break;

                case ObjectParam.Image:
                    behaviour = element.GetComponent<Image>();
                    break;

                case ObjectParam.Gameobject:
                    behaviour = element.gameObject;
                    break;

                case ObjectParam.InputFiled:
                    behaviour = element.GetComponent<InputField>();
                    break;
            }
            this._dic.Add(dic.Key, behaviour);
        }
        this._serialDic = null;
        #endregion
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

public enum ObjectParam
{
    Text,
    Image,
    InputFiled,
    Gameobject
}