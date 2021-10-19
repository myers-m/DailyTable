using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolElement
{
    GameObject self { get; }
    float nowTime { get; set; }
    int time { get; }

    void Init(System.Object[] param);
    void Recover();
    void Release();
}
