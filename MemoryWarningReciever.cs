using System;
using UnityEngine;

public class MemoryWarningReciever : MonoBehaviour
{
    public void DidReceiveMemoryWarning(string message)
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }
}

