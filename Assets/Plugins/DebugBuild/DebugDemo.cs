using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDemo : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        MyTool.MyPrint.Print("general");
        MyTool.MyPrint.Print("Test",MyTool.DebugTag.Test);
    }
}
