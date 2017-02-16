using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MainMenuBar : SingletonMonoBehaviour<MainMenuBar>
{
    protected SceneRootComponent activeScene;
    protected MainMenuBarBase barBase;
    protected Kind kind;
    public GameObject mainMenuPrefab;
    private GameObject obj;
    protected int panelDepth = -1;

    public static void FrameIn(bool is_force = false)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.FrameInOut(true, is_force);
    }

    public static void FrameOut(bool is_force = false)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.FrameInOut(false, is_force);
    }

    public static void requestSelectedSceneChange()
    {
        if (SingletonMonoBehaviour<MainMenuBar>.Instance != null)
        {
            SingletonMonoBehaviour<MainMenuBar>.Instance.RequestSelectedSceneChange();
        }
    }

    public void RequestSelectedSceneChange()
    {
        if (this.barBase != null)
        {
            this.barBase.RequestSelectedSceneChange();
        }
    }

    public static void requestSelectedSignal()
    {
        if (SingletonMonoBehaviour<MainMenuBar>.Instance != null)
        {
            SingletonMonoBehaviour<MainMenuBar>.Instance.RequestSelectedSignal();
        }
    }

    public void RequestSelectedSignal()
    {
        if (this.barBase != null)
        {
            this.barBase.RequestSelectedSignal();
        }
    }

    public static void requestTerminalSceneChange()
    {
        if (SingletonMonoBehaviour<MainMenuBar>.Instance != null)
        {
            SingletonMonoBehaviour<MainMenuBar>.Instance.RequestTerminalSceneChange();
        }
    }

    public void RequestTerminalSceneChange()
    {
        if (this.barBase != null)
        {
            this.barBase.RequestTerminalSceneChange();
        }
    }

    public static void resumeMenuBar(SceneRootComponent scene, Kind kind, int depth)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetResumeMenu(scene, kind, depth);
        }
    }

    public static void setActiveScene(SceneRootComponent scene, Camera cam = null)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetActiveScene(scene, Kind.NONE, -1, cam);
        }
    }

    public static void setActiveScene(SceneRootComponent scene, int depth, Camera cam = null)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetActiveScene(scene, Kind.NONE, depth, cam);
        }
    }

    public static void setActiveScene(SceneRootComponent scene, Kind kind, int depth, Camera cam = null)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetActiveScene(scene, kind, depth, cam);
        }
    }

    protected void SetActiveScene(SceneRootComponent scene, Kind kind, int panelDepth, Camera cam = null)
    {
        if ((this.activeScene != scene) && (this.barBase != null))
        {
            UnityEngine.Object.Destroy(this.obj);
            this.obj = null;
        }
        this.kind = kind;
        this.panelDepth = panelDepth;
        this.activeScene = scene;
        if (this.barBase != null)
        {
            this.barBase.SetActiveScene(scene, kind, panelDepth, cam);
        }
    }

    public static void setButtonActive(bool isActive)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetButtonActive(instance.kind, isActive);
        }
    }

    protected void SetButtonActive(Kind kind, bool isActive)
    {
        if (this.barBase != null)
        {
            this.barBase.SetButtonActive(kind, isActive);
        }
    }

    protected void SetButtonKind(Kind kind)
    {
        this.kind = kind;
        if (this.barBase != null)
        {
            this.barBase.SetButtonKind(kind);
        }
    }

    public static void setCloseHideMode(bool is_enable)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.IsCloseHideMode = is_enable;
    }

    public static void SetDispBtnAct(MainMenuBarButton.Kind kind, System.Action act)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.SetDispBtnAct(kind, act);
    }

    public static void SetDispBtnColliderEnable(bool is_enable, MainMenuBarButton.Kind kind = 6)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.SetDispBtnColliderEnable(is_enable, kind);
    }

    public static void setKind(Kind kind)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetButtonKind(kind);
        }
    }

    public static void setMenuActive(bool isActive, Camera cam = null)
    {
        MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
        if (instance != null)
        {
            instance.SetMenuActive(isActive, cam);
        }
    }

    protected void SetMenuActive(bool isActive, Camera cam = null)
    {
        if ((isActive && (this.obj == null)) && (this.activeScene != null))
        {
            UICamera componentInChildren = null;
            if (cam == null)
            {
                componentInChildren = this.activeScene.GetComponentInChildren<UICamera>();
            }
            else
            {
                componentInChildren = cam.gameObject.GetComponent<UICamera>();
            }
            if (componentInChildren != null)
            {
                this.obj = UnityEngine.Object.Instantiate<GameObject>(this.mainMenuPrefab);
                this.obj.transform.parent = componentInChildren.transform;
                this.obj.transform.localPosition = Vector3.zero;
                this.obj.transform.localRotation = Quaternion.identity;
                this.obj.transform.localScale = Vector3.one;
                this.obj.layer = componentInChildren.gameObject.layer;
                this.barBase = this.obj.GetComponent<MainMenuBarBase>();
                this.barBase.SetActiveScene(this.activeScene, this.kind, this.panelDepth, cam);
            }
        }
        if (this.barBase != null)
        {
            this.barBase.SetMenuActive(isActive);
        }
    }

    public static void SetMenuBtnAct(System.Action act)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.SetMenuBtnAct(act);
    }

    public static void SetMenuBtnColliderEnable(bool is_enable)
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.SetMenuBtnColliderEnable(is_enable);
    }

    protected void SetResumeMenu(SceneRootComponent scene, Kind kind, int panelDepth)
    {
        if (this.activeScene != scene)
        {
            this.kind = kind;
            this.panelDepth = panelDepth;
            this.activeScene = scene;
            if (this.obj != null)
            {
                UnityEngine.Object.Destroy(this.obj);
                this.obj = null;
                this.SetMenuActive(true, null);
            }
        }
    }

    public static void UpdateNoticeNumber()
    {
        SingletonMonoBehaviour<MainMenuBar>.Instance.barBase.UpdateNoticeNumber();
    }

    public static bool IsEnableOutSideCollider
    {
        get
        {
            MainMenuBar instance = SingletonMonoBehaviour<MainMenuBar>.Instance;
            if (instance == null)
            {
                return false;
            }
            return instance.barBase?.IsEnableOutSideCollider;
        }
    }

    public enum Kind
    {
        NONE,
        TERMINAL,
        AREA,
        FORMATION,
        SUMMON,
        COMBINE,
        SHOP,
        FRIEND,
        MYROOM
    }
}

