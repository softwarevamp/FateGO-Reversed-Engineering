using System;
using UnityEngine;

public class SceneRootComponent : BaseMonoBehaviour
{
    protected int depth;
    protected MainMenuBar.Kind kind;
    protected int manualHeight = ManagerConfig.HEIGHT;
    protected int manualWidth = ManagerConfig.WIDTH;
    public PlayMakerFSM myFSM;
    protected UIRoot uiRoot;

    public virtual void beginFinish()
    {
    }

    public virtual void beginInitialize()
    {
        Debug.Log("beginInitialize:" + this.getSceneName());
        this.initRenderSetting();
        this.setMainMenuBar(MainMenuBar.Kind.NONE, 1);
        this.hideUserStatus();
    }

    public virtual void beginPause()
    {
    }

    public virtual void beginResume()
    {
        this.beginFinish();
        this.beginStartUp();
        this.resumeMainMenuBar();
    }

    public virtual void beginResume(object data)
    {
        this.beginResume();
    }

    public virtual void beginStartUp()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        Debug.Log("beginStartUp:" + this.getSceneName());
        this.sendMessageStartUp();
    }

    public virtual void beginStartUp(object data)
    {
        this.beginStartUp();
    }

    public bool checkSceneName(string name) => 
        base.gameObject.name.Equals(name);

    public string getSceneName() => 
        base.gameObject.name;

    public virtual void hideUserStatus()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.HideUserStatus();
    }

    public void initRenderSetting()
    {
        RenderSettings.ambientLight = Color.white;
    }

    protected void ReScaleUpdate()
    {
        if (((this.manualWidth > 0) && (this.manualHeight > 0)) && (this.uiRoot != null))
        {
            int manualHeight = this.manualHeight;
            float num2 = ((float) (Screen.height * this.manualWidth)) / ((float) (Screen.width * this.manualHeight));
            if (num2 > 1f)
            {
                manualHeight = (int) (manualHeight * num2);
            }
            if (this.uiRoot.manualHeight != manualHeight)
            {
                this.uiRoot.manualHeight = manualHeight;
            }
        }
    }

    public void resumeMainMenuBar()
    {
        MainMenuBar.resumeMenuBar(this, this.kind, this.depth);
    }

    public void sendMessage(string message)
    {
        if (this.myFSM != null)
        {
            Debug.Log("CUSTOM[" + message + "]:" + this.getSceneName());
            this.myFSM.SendEvent(message);
        }
    }

    public void sendMessageResume()
    {
        if (this.myFSM != null)
        {
            Debug.Log("RESUME:" + this.getSceneName());
            if (SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
            {
                this.myFSM.SendEvent("RESUME");
            }
        }
    }

    public void sendMessageStartUp()
    {
        if (this.myFSM != null)
        {
            Debug.Log("STARTUP:" + this.getSceneName());
            if (SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
            {
                this.myFSM.SendEvent("STARTUP");
            }
        }
    }

    public void setMainMenuBar(MainMenuBar.Kind kind, int depth)
    {
        this.kind = kind;
        this.depth = depth;
        MainMenuBar.setActiveScene(this, kind, depth, null);
    }

    public bool SetSceneActive(bool flag)
    {
        if (this.uiRoot != null)
        {
            this.uiRoot.gameObject.SetActive(flag);
            return true;
        }
        return false;
    }

    public virtual void showUserStatus()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.ShowUserStatus();
    }

    public void Start()
    {
        this.myFSM = base.GetComponent<PlayMakerFSM>();
        this.uiRoot = base.GetComponentInChildren<UIRoot>();
        this.ReScaleUpdate();
    }
}

