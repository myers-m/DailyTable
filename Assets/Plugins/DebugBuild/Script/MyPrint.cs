using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool {
    public class MyPrint {
        static MyPrint _instance;
        public DebugAssets _assets;

        public static void Print(object content, DebugTag tag = DebugTag.General,MyPrintType type = MyPrintType.Log) {
            Init();
            if (MyPrint._instance._assets._tags.Contains(tag)) {
                switch (type) {
                    case MyPrintType.Log:
                        Debug.Log(content);
                        break;

                    case MyPrintType.Warning:
                        Debug.LogWarning(content);
                        break;

                    case MyPrintType.Error:
                        Debug.LogError(content);
                        break;
                }
            }
        }

        public static void Init() {
            if (MyPrint._instance == null) {
                MyPrint._instance = new MyPrint();
                MyPrint._instance._assets = (DebugAssets)Resources.Load("MyPrint/DebugAssets");
            }
        }
    }

    public enum MyPrintType {
        Log,
        Error,
        Warning
    }
}
