using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEffect : MonoBehaviour
{
    public float _deltaTime = 1;

    private void OnEnable() {
        StartCoroutine(this.StartEffect());
    }

    protected virtual void DoEffect() {

    }

    protected virtual IEnumerator StartEffect() {
        yield return new WaitForSeconds(this._deltaTime);
        this.DoEffect();
    }
}
