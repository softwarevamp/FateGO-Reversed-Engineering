using System;

public class TestSceneRootComponent : SceneRootComponent
{
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
    }
}

