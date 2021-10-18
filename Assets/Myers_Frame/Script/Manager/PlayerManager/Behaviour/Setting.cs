using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame {
    [My(BehaviourType.Player,"*","")]
    public class Setting : BasePlayerBehaviour {
        public int _lastFile = 0;

        public KeyCode _forward = KeyCode.W;
        public KeyCode _back = KeyCode.S;
        public KeyCode _left = KeyCode.A;
        public KeyCode _right = KeyCode.D;
    }
}