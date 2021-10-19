using UnityEngine;
using System.Collections.Generic;
 
/// Usage:
/// 
/// [System.Serializable]
/// class MyDictionary : SerializableDictionary<int, GameObject> {}
/// public MyDictionary dic;
///
[System.Serializable]
public class SerializableDictionary<TKey,TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // We save the keys and values in two lists because Unity does understand those.
     [SerializeField]
     private List<TKey> _keys = new List<TKey>();
     [SerializeField]
     private List<string> _values = new List<string>();
    [SerializeField]
    private List<string> _type = new List<string>();

    // Before the serialization we fill these lists
    public void OnBeforeSerialize()
     {
        //官方例子有误，去掉
    }

    public void SerializeAdd(TKey key, TValue value) {
        if (this._keys.Contains(key)) {
            this.SerializeSet(key, value);
            return;
        }
        this._keys.Add(key);
        this._values.Add(JsonUtility.ToJson(value));
        this._type.Add(value.GetType().Name);
    }

    protected void SerializeSet(TKey key, TValue value) {
        this._values[this._keys.IndexOf(key)] = JsonUtility.ToJson(value);
    }

     // After the serialization we create the dictionary from the two lists
     public void OnAfterDeserialize()
     {
         this.Clear();
         int count = Mathf.Min(_keys.Count, _values.Count);
         for (int i = 0; i<count; ++i)
         {
            this.Add(_keys[i], (TValue)JsonUtility.FromJson(_values[i], this.GetType().Assembly.GetType(this._type[i])));
         }
     }
}
