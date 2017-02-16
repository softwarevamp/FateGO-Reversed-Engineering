using System;

public class SceneJumpInfo
{
    protected int id;
    protected string name;
    protected string returnSceneName;

    public SceneJumpInfo()
    {
    }

    public SceneJumpInfo(int id)
    {
        this.id = id;
    }

    public SceneJumpInfo(string name)
    {
        this.name = name;
    }

    public SceneJumpInfo(string name, int id)
    {
        this.name = name;
        this.id = id;
    }

    public bool IsEnableReturnScene() => 
        (this.returnSceneName != null);

    public bool ReturnScene()
    {
        if (this.returnSceneName != null)
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.returnSceneName, SceneManager.FadeType.BLACK, null);
            return true;
        }
        return false;
    }

    public void SetReturnNowScene()
    {
        this.returnSceneName = SingletonMonoBehaviour<SceneManager>.Instance.GetNowSceneName();
    }

    public void SetReturnScene(SceneList.Type type)
    {
        this.returnSceneName = SceneList.getSceneName(type);
    }

    public int Id =>
        this.id;

    public string Name =>
        this.name;
}

