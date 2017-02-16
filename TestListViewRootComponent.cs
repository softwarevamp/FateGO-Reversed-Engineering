using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/TestListViewRootComponent")]
public class TestListViewRootComponent : SceneRootComponent
{
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

