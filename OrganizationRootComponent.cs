using System;
using UnityEngine;

public class OrganizationRootComponent : SceneRootComponent
{
    protected int selectItemNum;
    protected long[] selectServantIdList;
    protected State state;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected OrganizationTopListViewManager topListViewManager;

    public void BackBuyQpItem()
    {
        if (this.state == State.INPUT_BUY_QP_ITEM)
        {
            this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.state = State.QUIT_BUY_QP_ITEM;
        }
    }

    public override void beginFinish()
    {
        this.Quit();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.FORMATION, 0x1d);
        base.hideUserStatus();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.hideUserStatus();
        base.beginResume();
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.PARTY_ORGANIZATION);
        this.RefreshInfo();
        MainMenuBar.setMenuActive(true, null);
        base.beginStartUp();
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.topListViewManager.gameObject.SetActive(true);
            this.topListViewManager.CreateList(OrganizationTopListViewManager.Kind.NORMAL);
            this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(false, TitleInfoControl.TitleKind.SHOP);
            this.state = State.INIT_TOP;
        }
    }

    public void OnClickBack()
    {
        State state = this.state;
        switch (state)
        {
            case State.INPUT_TOP:
                this.titleInfo.sendEvent("CLICK_BACK");
                return;

            case State.INPUT_BUY_QP_ITEM:
                break;

            default:
                if ((state != State.INPUT_BUY_MANA_ITEM) && (state != State.INPUT_BUY_STONE_ITEM))
                {
                    return;
                }
                break;
        }
        this.titleInfo.sendEvent("CLICK_BACK");
    }

    private void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_TOP:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.INPUT, new OrganizationTopListViewManager.CallbackFunc(this.OnSelectTop));
                if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_SHOP))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_SHOP1"), TutorialFlag.Id.TUTORIAL_LABEL_SHOP, null);
                }
                break;

            case State.INIT_BUY_QP_ITEM:
                this.state = State.INIT_BUY_QP_ITEM2;
                break;

            case State.INIT_BUY_QP_ITEM2:
                this.state = State.INPUT_BUY_QP_ITEM;
                break;

            case State.QUIT_BUY_QP_ITEM:
                this.state = State.QUIT_BUY_QP_ITEM2;
                break;

            case State.QUIT_BUY_QP_ITEM2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.INPUT, new OrganizationTopListViewManager.CallbackFunc(this.OnSelectTop));
                break;
        }
    }

    protected void OnSelectTop(string result)
    {
        if (result != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.myFSM.SendEvent(result);
        }
    }

    public void Quit()
    {
        this.topListViewManager.DestroyList();
        this.topListViewManager.gameObject.SetActive(false);
        this.state = State.INIT;
    }

    public void RefreshInfo()
    {
    }

    public void SelectBuyManaItem()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.state = State.INIT_BUY_MANA_ITEM;
        }
    }

    public void SelectBuyQpItem()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.topListViewManager.SetMode(OrganizationTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.state = State.INIT_BUY_QP_ITEM;
        }
    }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INPUT_TOP,
        INIT_BUY_QP_ITEM,
        INIT_BUY_QP_ITEM2,
        INPUT_BUY_QP_ITEM,
        QUIT_BUY_QP_ITEM,
        QUIT_BUY_QP_ITEM2,
        INIT_BUY_MANA_ITEM,
        INIT_BUY_MANA_ITEM2,
        INPUT_BUY_MANA_ITEM,
        QUIT_BUY_MANA_ITEM,
        QUIT_BUY_MANA_ITEM2,
        INIT_BUY_STONE_ITEM,
        INIT_BUY_STONE_ITEM2,
        INPUT_BUY_STONE_ITEM,
        QUIT_BUY_STONE_ITEM,
        QUIT_BUY_STONE_ITEM2
    }
}

