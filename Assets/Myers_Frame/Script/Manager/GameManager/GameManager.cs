using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFrame {

    public class GameManager : BaseManager {
        public static GameManager _instance;
        public string _currentScene = "";
        public List<string> _sceneList = new List<string>();
        
        AsyncOperation _operation;

        bool _isLoading = false;
        bool _isLoadingResource = false;

        int _value = 0;

        protected override void Awake() {
            GameManager._instance = GameManager._instance == null ? this : GameManager._instance;
            base.Awake();
        }

        protected override void Update() {
            base.Update();
            if (this._isLoading) {
                if (this._isLoadingResource) {
                    float value = (((float)DataManager._instance.nowLoad / DataManager._instance._maxLoad) * 0.5f);
                    if (this._value < value * 100) {
                        this._value++;
                    }
                    if (this._value == 50) {
                        this._isLoadingResource = false;
                        this._operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(this._currentScene);
                        this._operation.allowSceneActivation = false;
                    }
                } else {
                    float value = ((this._operation.progress / 0.9f) * 0.5f) + (((float)DataManager._instance.nowLoad / DataManager._instance._maxLoad) * 0.5f);
                    if (this._value < value * 100) {
                        this._value++;
                    }
                    if (this._value == 100) {
                        StartCoroutine(this.FinishLoad());
                    }
                }
            }
        }

        public void LoadScene(string name) {
            if (!this._isLoading) {
                this._isLoading = true;
                this._isLoadingResource = true;
                this._currentScene = name;
                EventManager._instance.DoEvent("BeginChangeScene", name);
            }
        }

        IEnumerator FinishLoad() {
            this._operation.allowSceneActivation = true;
            yield return new WaitForSeconds(1);
            this._value = 0;
            this._isLoading = false;
            EventManager._instance.DoEvent("FinishLoad", this._currentScene);
        }
    }
}