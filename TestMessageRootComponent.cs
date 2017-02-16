using System;

public class TestMessageRootComponent : SceneRootComponent
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

