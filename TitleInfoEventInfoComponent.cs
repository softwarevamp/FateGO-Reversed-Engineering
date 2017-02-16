using System;
using UnityEngine;

public abstract class TitleInfoEventInfoComponent : MonoBehaviour
{
    protected TitleInfoEventInfoComponent()
    {
    }

    public abstract bool IsDispPossible();
    public abstract void UpdateDisp();
}

