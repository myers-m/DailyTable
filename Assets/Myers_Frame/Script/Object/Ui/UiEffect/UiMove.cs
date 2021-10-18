using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiMove : UiEffect
{
    public Vector3 _endPosition;
    public float _duration = 1;


    protected override void DoEffect() {
        base.DoEffect();
        DOTween.To(() => ((RectTransform)this.transform).anchoredPosition, goal => ((RectTransform)this.transform).anchoredPosition = goal, (Vector2)this._endPosition, this._duration);
    }
}
