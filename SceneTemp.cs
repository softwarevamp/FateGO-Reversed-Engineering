using System;

public class SceneTemp
{
    private string sceneName;

    public SceneTemp(SceneRootComponent comp)
    {
        this.sceneName = comp.getSceneName();
    }

    public SceneTemp(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public string getSceneName() => 
        this.sceneName;
}

