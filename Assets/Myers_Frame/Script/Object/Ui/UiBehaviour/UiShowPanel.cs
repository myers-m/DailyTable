using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[MyObject("UiElement", "The behaviour show something while other has choose")]
public class UiShowPanel : BaseObjectBehaviour
{
    UiElement _self;
    Text[] _texts;
    Image[] _images;

    public override void Awake(BaseObject self)
    {
        base.Awake(self);
        this._self = (UiElement)self;
        ((RectTransform)this._self._transform).anchorMin = new Vector2(0, 1);
        ((RectTransform)this._self._transform).anchorMax = new Vector2(0, 1);
        this._texts = this._self.GetComponentsInChildren<Text>();
        this._images = this._self.GetComponentsInChildren<Image>();
        this._self._updateAction += this.Update;
    }

    void Update()
    {
        ((RectTransform)this._self._transform).position = Input.mousePosition;
    }

    public void SetText(params string[] texts) {
        for (int i = 0; i < texts.Length; i++) {
            this._texts[i].text = texts[i];
        }
    }

    public void SetImage(params Sprite[] sprites)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            this._images[i].sprite = sprites[i];
        }
    }
}
