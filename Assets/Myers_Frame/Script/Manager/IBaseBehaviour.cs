using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {
    public interface IBaseBehaviour {
        bool isStart { get; set; }
        void Awake(BaseManager self);
        void Start();
        void DoSomeThing(string tag, object[] param);
        object[] GetSomeThing(string tag, object[] param);
        void OnDestroy();
    }
}