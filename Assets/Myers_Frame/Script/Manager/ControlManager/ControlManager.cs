using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {

    public class ControlManager : BaseManager {
        public static ControlManager _instance;

        protected override void Awake() {
            ControlManager._instance = ControlManager._instance == null ? this : ControlManager._instance;
            //this.JoinBehaviour(new ShowControlBehaviour());
            base.Awake();
        }
    }
}