using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBaseObjectBehaviour {
    bool isStart { get; set; }
    void Awake(BaseObject self);
    void Start();
    void OnEnable();
    void OnDestroy();
    void DoSomeThing(string tag, object[] param);
    object[] GetSomeThing(string tag, object[] param);
}
