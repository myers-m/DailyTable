using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParentTable : BaseTable
{
    public SerializableDic<string, BaseTable> _tables = new SerializableDic<string, BaseTable>();
}
