using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiElement : PoolObject
{
    public List<string> _scene;
    public string _level;
    public bool _need = false;

    [Tooltip("while the param is true ,the prefab can't destroy and create multily")]
    public bool _joinPool = true;

    protected override void Awake() {
        base.Awake();
        if (this._joinPool) {
            MyFrame.UiManager._instance.JoinElement(this);
        }
    }

    private void Start() {
        this.gameObject.SetActive(this._need);
    }

    public void SetScene(List<string> scene) {
        this._scene = scene;
    }

    protected override void DoDestroy(object[] param) {
        if (!this._scene.Contains("*") && !this._scene.Contains((string)param[0])) {
            MyFrame.UiManager._instance.CloseWindow(this, true);
        }
    }

    public override void Init(object[] param) {
        this.gameObject.SetActive(true);
    }

    public override void Recover() {
        this.gameObject.SetActive(false);
    }

    public override void Release() {

    }

    protected override void OnDestroy() {
        base.OnDestroy();
        if (this._joinPool) {
            MyFrame.UiManager._instance.RemoveElement(this.name);
        }
    }
}
