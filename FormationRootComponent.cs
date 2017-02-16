using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FormationRootComponent : SceneRootComponent
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache15;
    private bool isTutorial1;
    private bool isTutorial2;
    [SerializeField]
    protected UIButton MasterEquipButton;
    [SerializeField]
    protected UISprite MasterEquipSprite;
    [SerializeField]
    protected StandFigureSlideComponent mStandServant;
    private UserServantEntity[] mStandSvtDatas;
    private int mStandSvtIdx;
    [SerializeField]
    protected UIButton mstFormationBtn;
    [SerializeField]
    protected MasterFormationComponent mstFormationComp;
    [SerializeField]
    protected UIButton supportSelectBtn;
    [SerializeField]
    protected UIButton svtFormationBtn;
    [SerializeField]
    protected UIButton svtListBtn;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected GameObject topDispRootObj;
    public readonly Vector2 TUTORIAL_BACK_ARROW_POS = new Vector2(-430f, 250f);
    public readonly Rect TUTORIAL_BACK_ARROW_RECT = new Rect(-510f, 225f, 160f, 60f);
    public const float TUTORIAL_BACK_ARROW_WAY = 180f;
    public readonly Vector2 TUTORIAL_FORMATION_ARROW_POS = new Vector2(42f, 150f);
    public readonly Rect TUTORIAL_FORMATION_ARROW_RECT = new Rect(23f, 90f, 460f, 120f);
    public const float TUTORIAL_FORMATION_ARROW_WAY = 90f;
    public readonly Vector2 TUTORIAL_FORMATION_MESSAGE_POS = new Vector2(0f, -40f);
    private MENUTYPE type;
    private UserGameEntity usrGameEnt;

    public override void beginFinish()
    {
        Debug.Log("Call beginFinish");
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.FORMATION, 30);
        base.hideUserStatus();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        Debug.Log("Call beginInitialize");
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.resumeMainMenuBar();
        base.beginResume();
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.FORMATION);
        this.titleInfo.setBackBtnDepth(0x1d);
        MainMenuBar.setMenuActive(true, null);
        base.sendMessageStartUp();
        Debug.Log("Call beginStartUp");
    }

    private void changeUserEquipCallback(string res)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    public void closeArrowMark()
    {
        Debug.Log(string.Concat(new object[] { "***** closeArrowMark: ", this.isTutorial1, " ", this.isTutorial2 }));
        if (this.isTutorial1)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
        }
        if (this.isTutorial2)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(null);
        }
    }

    public void closeMasterFormation()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.type = MENUTYPE.MAIN;
            this.mstFormationComp.closeMasterFormation();
            base.myFSM.SendEvent("GO_NEXT");
        });
    }

    public void Init()
    {
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.FORMATION);
        this.setMainActive(MENUTYPE.MAIN);
        this.usrGameEnt = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        if (this.usrGameEnt.genderType == 2)
        {
            this.MasterEquipButton.normalSprite = "img_form_menu05";
            this.MasterEquipSprite.spriteName = "img_form_menu05";
        }
        else
        {
            this.MasterEquipButton.normalSprite = "img_form_menu01";
            this.MasterEquipSprite.spriteName = "img_form_menu01";
        }
        this.isTutorial1 = false;
        this.isTutorial2 = false;
        this.titleInfo.setBackBtnColliderEnable(true);
        if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END))
        {
            if (TutorialFlag.IsProgressDone(TutorialFlag.Progress._2) && TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA))
            {
                if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
                {
                    this.isTutorial1 = true;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_FORMATION1"), this.TUTORIAL_FORMATION_ARROW_POS, this.TUTORIAL_FORMATION_ARROW_RECT, (float) 90f, this.TUTORIAL_FORMATION_MESSAGE_POS, -1, delegate {
                        this.titleInfo.setBackBtnColliderEnable(false);
                        this.titleInfo.SetHelpBtnColliderEnable(false);
                        MainMenuBar.SetMenuBtnColliderEnable(false);
                        this.mstFormationBtn.isEnabled = false;
                        this.svtListBtn.isEnabled = false;
                        this.supportSelectBtn.isEnabled = false;
                        this.svtFormationBtn.gameObject.GetComponent<UIDragScrollView>().enabled = false;
                    });
                }
                else if (TutorialFlag.IsProgressDone(TutorialFlag.Progress._3))
                {
                    this.isTutorial2 = true;
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(this.TUTORIAL_BACK_ARROW_POS, (float) 180f, this.TUTORIAL_BACK_ARROW_RECT, delegate {
                        this.titleInfo.SetHelpBtnColliderEnable(false);
                        MainMenuBar.SetMenuBtnColliderEnable(false);
                        this.svtFormationBtn.isEnabled = false;
                        this.mstFormationBtn.isEnabled = false;
                        this.svtListBtn.isEnabled = false;
                        this.supportSelectBtn.isEnabled = false;
                    });
                }
            }
        }
        else if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_DECK_SCENE))
        {
            if (<>f__am$cache15 == null)
            {
                <>f__am$cache15 = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.FORMATION_TOP, TutorialFlag.Id.TUTORIAL_LABEL_DECK_SCENE, <>f__am$cache15);
        }
        this.mStandSvtDatas = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).GetUserServantListFromDeck();
        this.mStandSvtIdx = -1;
        this.SetupStandServant(delegate {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            base.myFSM.SendEvent("GO_NEXT");
        });
    }

    public void OnClickBack()
    {
        string msg = "CLICK_TERMINAL";
        switch (this.type)
        {
            case MENUTYPE.MAIN:
                msg = "CLICK_TERMINAL";
                break;

            case MENUTYPE.MASTER_FORMATION:
                msg = "CLICK_BACK";
                break;
        }
        this.titleInfo.sendEvent(msg);
    }

    public void requestChangeUsrEquip()
    {
        long userEquipId = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).userEquipId;
        long num2 = this.mstFormationComp.getCurrentUsrEquipId();
        bool flag = this.mstFormationComp.isChangeEquip();
        if ((userEquipId != num2) && flag)
        {
            NetworkManager.getRequest<UserFormationRequest>(new NetworkManager.ResultCallbackFunc(this.changeUserEquipCallback)).beginRequest(num2);
        }
        else
        {
            base.myFSM.SendEvent("NO_CHANGE_EQUIP");
        }
    }

    private void setMainActive(MENUTYPE type)
    {
        switch (type)
        {
            case MENUTYPE.MAIN:
                MainMenuBar.setMenuActive(true, null);
                this.topDispRootObj.SetActive(true);
                this.topDispRootObj.transform.GetComponentInChildren<UIScrollView>().ResetPosition();
                break;

            case MENUTYPE.MASTER_FORMATION:
                MainMenuBar.setMenuActive(false, null);
                this.topDispRootObj.SetActive(false);
                break;
        }
    }

    private void SetupStandServant(System.Action end_act = null)
    {
        <SetupStandServant>c__AnonStorey87 storey = new <SetupStandServant>c__AnonStorey87 {
            end_act = end_act,
            <>f__this = this
        };
        UserServantEntity usd = null;
        for (int i = 0; i < this.mStandSvtDatas.Length; i++)
        {
            this.mStandSvtIdx++;
            if (this.mStandSvtIdx >= this.mStandSvtDatas.Length)
            {
                this.mStandSvtIdx = 0;
            }
            usd = this.mStandSvtDatas[this.mStandSvtIdx];
            if (usd != null)
            {
                break;
            }
        }
        this.mStandServant.Setup(usd, 0, new System.Action(storey.<>m__10C));
        this.mStandServant.SetBtnAct(new System.Action(storey.<>m__10D));
    }

    public void showMasterFormation()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.type = MENUTYPE.MASTER_FORMATION;
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.FORM_MASTER);
            this.setMainActive(this.type);
            this.mstFormationComp.setCmdSpellImg();
            this.mstFormationComp.setMasterFormation(this.usrGameEnt);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    [CompilerGenerated]
    private sealed class <SetupStandServant>c__AnonStorey87
    {
        internal FormationRootComponent <>f__this;
        internal System.Action end_act;

        internal void <>m__10C()
        {
            this.<>f__this.mStandServant.SlideIn(null);
            this.end_act.Call();
        }

        internal void <>m__10D()
        {
            if ((TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_END) && !this.<>f__this.mStandServant.IsLoding()) && !this.<>f__this.mStandServant.IsMoving())
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.<>f__this.mStandServant.SlideOut(() => this.<>f__this.SetupStandServant(null), false);
            }
        }

        internal void <>m__110()
        {
            this.<>f__this.SetupStandServant(null);
        }
    }

    private enum MENUTYPE
    {
        MAIN,
        MASTER_FORMATION,
        SERVANT_FORMATION,
        SERVANT_LIST
    }
}

