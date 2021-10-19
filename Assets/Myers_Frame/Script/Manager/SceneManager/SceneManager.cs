using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {

    public class SceneManager : BaseManager {
        public static SceneManager _instance;

        protected override void Awake() {
            //this.JoinBehaviour(new ShowSceneBehaviour());
            SceneManager._instance = SceneManager._instance == null ? this : SceneManager._instance;
            base.Awake();
        }
    }
}