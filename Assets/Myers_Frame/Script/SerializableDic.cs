using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDic<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {

    [SerializeField]
    private List<TKey> _keys = new List<TKey>();
    [SerializeField]
    private List<TValue> _values = new List<TValue>();

    // Before the serialization we fill these lists
    public void OnBeforeSerialize() {
        //官方例子有误，去掉     　　　　
    }

    public void SerializeAdd(TKey key, TValue value) {
        if (this._keys.Contains(key)) {
            this.SerializeSet(key, value);
            return;
        }
        this._keys.Add(key);
        this._values.Add(value);
    }

    protected void SerializeSet(TKey key, TValue value) {
        this._values[this._keys.IndexOf(key)] = value;
    }

    // After the serialization we create the dictionary from the two lists
    public void OnAfterDeserialize() {
        this.Clear();
        int count = Mathf.Min(_keys.Count, _values.Count);
        for (int i = 0; i < count; ++i) {
            this.Add(this._keys[i], this._values[i]);
        }
    }
}
