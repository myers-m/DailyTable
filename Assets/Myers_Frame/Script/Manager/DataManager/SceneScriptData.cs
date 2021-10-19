using System.Collections.Generic;
using UnityEngine;


public class SceneScriptData : ISerializationCallbackReceiver {
    [SerializeField]
    public List<string> _name = new List<string>();
    [SerializeField]
    public List<Vector3> _position = new List<Vector3>();
    [SerializeField]
    public List<Quaternion> _rotation = new List<Quaternion>();


    public void OnAfterDeserialize() {

    }

    public void OnBeforeSerialize() {

    }
}