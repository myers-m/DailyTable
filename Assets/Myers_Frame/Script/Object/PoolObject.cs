using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolObject : BaseObject, IPoolElement {
    float IPoolElement.nowTime { get { return this._nowTime; } set { this._nowTime = value; } }
    int IPoolElement.time { get { return this._time; } }
    GameObject IPoolElement.self { get { return this.gameObject; } }

    private float _nowTime = 0;
    [SerializeField]
    private int _time = 10;
    [SerializeField]
    private bool _destroyInScene = true;

    public abstract void Init(object[] param);

    public abstract void Recover();

    public abstract void Release();

    protected override void DoDestroy(object[] param) {
        MyFrame.ObjectManager._instance.RemoveObject(this.gameObject, true);
    }
}
