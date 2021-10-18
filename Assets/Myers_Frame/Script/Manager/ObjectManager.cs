using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFrame
{

    public class ObjectManager : MonoBehaviour
    {
        public static ObjectManager _instance;
        Transform _transform;

        Dictionary<string, Queue<IPoolElement>> _pool = new Dictionary<string, Queue<IPoolElement>>();

        [SerializeField]
        int _computeCd = 5;
        float _nowCd = 0;

        private void Awake()
        {
            ObjectManager._instance = this;
            this._transform = this.transform;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            this.ComputeList();
        }

        void ComputeList()
        {
            if (this._nowCd >= this._computeCd)
            {
                List<string> removeList = new List<string>();
                foreach (KeyValuePair<string, Queue<IPoolElement>> poolElement in this._pool)
                {
                    if (poolElement.Value.Peek().time != -1)
                    {
                        int destroyNum = 0;
                        foreach (IPoolElement element in poolElement.Value)
                        {
                            element.nowTime += this._computeCd;
                            if (element.nowTime >= element.time)
                            {
                                destroyNum += 1;
                            }
                        }
                        for (int i = 0; i < destroyNum; i++)
                        {
                            this.ReleaseObject(poolElement.Value.Dequeue());
                        }
                        if (poolElement.Value.Count == 0)
                        {
                            removeList.Add(poolElement.Key);
                        }
                    }
                }
                foreach (string key in removeList)
                {
                    this._pool.Remove(key);
                }
                this._nowCd = 0;
                return;
            }
            this._nowCd += Time.deltaTime;
        }

        public GameObject GetObject(GameObject obj, params System.Object[] param)
        {
            if (obj.GetComponent<IPoolElement>() != null)
            {
                GameObject res;
                if (this._pool.ContainsKey(obj.name))
                {
                    IPoolElement element = this._pool[obj.name].Dequeue();
                    element.Init(param);
                    res = element.self;
                    if (this._pool[obj.name].Count == 0) {
                        this._pool.Remove(obj.name);
                    }
                }
                else
                {
                    res = Instantiate(obj);
                    res.name = obj.name;
                    res.GetComponent<IPoolElement>().Init(param);
                }
                return res;
            }
            else
            {
                return Instantiate(obj);
            }
        }

        public void RemoveObject(GameObject obj, bool force = false)
        {
            IPoolElement res;
            if (force)
            {
                if ((res = obj.GetComponent<IPoolElement>()) != null)
                {
                    if (this._pool.ContainsKey(obj.name))
                    {
                        this._pool.Remove(obj.name);
                    }
                    this.ReleaseObject(res);
                }
                else
                {
                    Destroy(obj);
                }
                return;
            }
            if ((res = obj.GetComponent<IPoolElement>()) != null)
            {
                if (this._pool.ContainsKey(obj.name))
                {
                    this._pool[obj.name].Enqueue(res);
                }
                else
                {
                    Queue<IPoolElement> list = new Queue<IPoolElement>();
                    list.Enqueue(res);
                    this._pool.Add(obj.name, list);
                }
                res.Recover();
                obj.transform.SetParent(this._transform);
            }
            else
            {
                Destroy(obj);
            }
        }

        public void ReleaseObject(IPoolElement element)
        {
            element.Release();
            Destroy(element.self);
        }
    }
}