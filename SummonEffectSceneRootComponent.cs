using System;
using UnityEngine;

public class SummonEffectSceneRootComponent : SceneRootComponent
{
    public Transform summonInstance;

    public override void beginFinish()
    {
    }

    public override void beginInitialize()
    {
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
        this.summonInstance.GetComponent<PlayMakerFSM>().SendEvent("START_EFFECT");
    }
}

