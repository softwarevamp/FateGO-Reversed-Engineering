using System;
using UnityEngine;

[AddComponentMenu("Sample/Test2ListView/Test2ListViewRootComponent")]
public class TestListView2RootComponent : SceneRootComponent
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

