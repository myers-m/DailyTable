using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiScale : UiEffect
{
    public Vector3 _endScale;
    public float _duration = 1;

    protected override void DoEffect() {
        base.DoEffect();
        DOTween.To(() => ((RectTransform)this.transform).localScale, goal => ((RectTransform)this.transform).localScale = goal, this._endScale, this._duration);
    }
}
