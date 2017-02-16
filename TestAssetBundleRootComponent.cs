using System;
using UnityEngine;

public class TestAssetBundleRootComponent : SceneRootComponent
{
    public GameObject stagelistprefab;
    public GameObject stagelistroot;

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
    }
}

