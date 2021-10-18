using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[MyObject("UiElement", "The behaviour can change some text and image")]
public class UiGeneral : UiBaseBehaviour
{
    Text[] _texts;
    Image[] _images;

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this._texts = self.GetComponentsInChildren<Text>();
        this._images = self.GetComponentsInChildren<Image>();
    }

    public GameObject GetChild(string name)
    {
        return this._self._transform.Find(name).gameObject;
    }
}
