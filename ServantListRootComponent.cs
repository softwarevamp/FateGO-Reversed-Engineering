using System;
using UnityEngine;

public class ServantListRootComponent : SceneRootComponent
{
    [SerializeField]
    protected CharaGraphListMenu charaGraphListMenu;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;

    public override void beginFinish()
    {
        this.state = State.INIT;
        this.charaGraphListMenu.Init();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.FORM_SVT_LIST);
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.FORM_SVT_LIST);
        base.setMainMenuBar(MainMenuBar.Kind.NONE, 40);
        MainMenuBar.setMenuActive(false, null);
        base.beginStartUp();
    }

    protected void EndCloseServantList()
    {
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.state = State.INIT_TOP;
            this.charaGraphListMenu.Open(CharaGraphListMenu.Kind.EQUIP, new CharaGraphListMenu.CallbackFunc(this.SelectServantList));
        }
    }

    public void OnClickBack()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
        {
            SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
        }
        else
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Formation, SceneManager.FadeType.BLACK, null);
        }
    }

    public void Quit()
    {
        this.state = State.INIT;
        this.charaGraphListMenu.Init();
    }

    public void SelectServantList(CharaGraphListMenu.ResultKind result)
    {
        this.charaGraphListMenu.Close(new System.Action(this.EndCloseServantList));
    }

    protected enum State
    {
        INIT,
        INIT_TOP
    }
}

