using System;
using UnityEngine;

public class TestFaceRootComponent : SceneRootComponent
{
    public GameObject figureParent;

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        StandFigureManager.CreateRenderPrefab(this.figureParent, 0x18704, 0, Face.Type.ANGRY, 10, null);
        base.beginStartUp();
    }
}

