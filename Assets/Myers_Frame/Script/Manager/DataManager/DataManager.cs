using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyFrame {
    public class DataManager : BaseManager {
        public static DataManager _instance;
        public Dictionary<string, UiElement> _uiList = new Dictionary<string, UiElement>();
        public Dictionary<string, GameObject> _objList = new Dictionary<string, GameObject>();
        public Dictionary<string, Sprite> _spriteList = new Dictionary<string, Sprite>();
        public Dictionary<string, CommonConfig> _configList = new Dictionary<string, CommonConfig>();
        public Dictionary<string, Material> _materialList = new Dictionary<string, Material>();
        public Dictionary<string, string> _assetList = new Dictionary<string, string>();

        public int _maxLoad = 0;
        int _nowLoad = 0;
        public int nowLoad { get { return this._nowLoad; } set { this._nowLoad = value; if (this._nowLoad == this._maxLoad) { EventManager._instance.DoEvent("FinishLoading", null); } } }

        protected override void Awake() {
            DataManager._instance = DataManager._instance == null ? this : DataManager._instance;
            base.Awake();
            this.LoadResource("MainResource", true);
        }

        internal void LoadResource(string config, bool analy = true, string loadType = "General") {
            if (loadType == "General") {
                this._maxLoad += 1;
                CommonConfig resource = new CommonConfig();
                resource.Load(config, (_) => {
                    if (analy) {
                        this.DoJoin(resource);
                    }
                    this.nowLoad += 1;
                });
                this._configList.Add(config, resource);
                return;
            }
            this.Do("Loading", loadType, config, analy);
        }

        void DoJoin(CommonConfig resource) {
            for (int i = 0; i < resource._data["TYPE"].Count; i++) {
                string name = resource._data["ID"][i];
                switch (resource._data["TYPE"][i]) {
                    case "UI":
                        this._maxLoad += 1;
                        Addressables.LoadAssetAsync<GameObject>(resource._data["ID"][i]).Completed += (_) => {
                            this._uiList.Add(name, _.Result.GetComponent<UiElement>());
                            this._objList.Add(name, _.Result);
                            this.nowLoad += 1;
                        };
                        break;

                    case "OBJECT":
                        this._maxLoad += 1;
                        Addressables.LoadAssetAsync<GameObject>(resource._data["ID"][i]).Completed += (_) => {
                            this._objList.Add(name, _.Result);
                            this.nowLoad += 1;
                        };
                        break;

                    case "SPRITE":
                        this._maxLoad += 1;
                        Addressables.LoadAssetAsync<Sprite>(resource._data["ID"][i]).Completed += (_) => {
                            this._spriteList.Add(name, _.Result);
                            this.nowLoad += 1;
                        };
                        break;

                    case "MATERIAL":
                        this._maxLoad += 1;
                        Addressables.LoadAssetAsync<Material>(resource._data["ID"][i]).Completed += (_) => {
                            this._materialList.Add(name, _.Result);
                            this.nowLoad += 1;
                        };
                        break;

                    case "JSON":
                        this.LoadResource(name, false);
                        break;

                    case "CONFIG":
                        this.LoadResource(name, true);
                        break;

                    case "ASSET":
                        this._maxLoad += 1;
                        Addressables.LoadAssetAsync<TextAsset>(resource._data["ID"][i]).Completed += (_) => {
                            this._assetList.Add(name, _.Result.text);
                            this.nowLoad += 1;
                        };
                        break;

                    default:
                        this._assetList.Add(name, resource._data["TYPE"][i]);
                        break;
                }
            }
        }

        public void RemoveResource(string config, bool analy = true, string loadType = "General") {
            if (loadType == "General") {
                if (analy) {
                    this.DoRemove(this._configList[config]);
                }
                this._configList.Remove(config);
                return;
            }
            this.Do("Remove", loadType, config, analy);
        }

        void DoRemove(CommonConfig resource) {
            for (int i = 0; i < resource._data["TYPE"].Count; i++) {
                string name = resource._data["ID"][i];
                switch (resource._data["TYPE"][i]) {
                    case "UI":
                        this._uiList.Remove(name);
                        break;

                    case "OBJECT":
                        this._objList.Remove(name);
                        break;

                    case "SPRITE":
                        this._spriteList.Remove(name);
                        break;

                    case "JSON":
                        this.RemoveResource(name, false);
                        break;

                    case "ASSET":
                        this._assetList.Remove(name);
                        break;

                    case "MATERIAL":
                        this._materialList.Remove(name);
                        break;

                    default:
                        this._assetList.Remove(name);
                        break;
                }
            }
        }
    }
}