using System;
using System.Collections.Generic;

public class MyObjectAttribute : Attribute
{
    public string _type;
    public string _content;

    public MyObjectAttribute(string type, string content) {
        this._type = type;
        this._content = content;
    }
}
