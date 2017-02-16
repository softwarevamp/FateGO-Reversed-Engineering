using System;
using UnityEngine;

public class RealTime : MonoBehaviour
{
    public static float deltaTime =>
        Time.unscaledDeltaTime;

    public static float time =>
        Time.unscaledTime;
}

