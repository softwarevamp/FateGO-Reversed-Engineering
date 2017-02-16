using System;
using UnityEngine;

public class showmemory : MonoBehaviour
{
    private void OnGUI()
    {
        uint totalAllocatedMemory = Profiler.GetTotalAllocatedMemory();
        GUI.Label(new Rect(10f, 10f, 200f, 50f), "Memory :" + (totalAllocatedMemory / 0xf4240) + "M");
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

