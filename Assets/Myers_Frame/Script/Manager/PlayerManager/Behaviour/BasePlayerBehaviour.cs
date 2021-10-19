using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace MyFrame {
    public class BasePlayerBehaviour : IBaseBehaviour {
        protected PlayerManager _self;

        public bool isStart { get; set; }
        [NonSerialized]
        public string _path = "PlayerPref";
        [NonSerialized]
        public string _name = "";

        public virtual void Awake(BaseManager self) {
            this._self = (PlayerManager)self;
            this._name = this.GetType().Name;
            this._self.LoadData(this._path, this._name, this);
        }

        public void Start() {

        }

        public void DoSomeThing(string tag, object[] param) {

        }

        public object[] GetSomeThing(string tag, object[] param) {
            return null;
        }

        public void OnDestroy() {
            this._self.SaveData(this._path, this._name, this);
        }
    }
}