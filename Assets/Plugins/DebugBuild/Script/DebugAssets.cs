using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool {
    [CreateAssetMenu(menuName = "MyPrint/DebugAssets")]
    public class DebugAssets : ScriptableObject {
        public List<DebugTag> _tags = new List<DebugTag>();
    }
}