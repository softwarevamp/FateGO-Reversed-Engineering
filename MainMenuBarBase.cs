using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MainMenuBarBase : MonoBehaviour
{
    protected PlayMakerFSM activeSceneFSM;
    private Collider closeCol;
    public MainMenuBarButton combineBtn;
    public const float FRAME_IN_OUT_MV_SPD_TIME = 0.25f;
    public const float FRAME_OUT_POS_Y = -70f;
    public MainMenuBarButton friendBtn;
    protected bool isOpen;
    private bool isSwitchMenuMoving;
    protected MainMenuBar.Kind kind;
    private System.Action[] mDispBtnActList = new System.Action[6];
    private List<MainMenuBarButton> mDispBtnList = new List<MainMenuBarButton>();
    private const float MENU_CLOSE_HIDE_POS_Y = -420f;
    private const float MENU_CLOSE_POS_Y = -328f;
    private const float MENU_CLOSE_TIME = 0.034f;
    private const float MENU_OPEN_CLOSE_TIME = 0.034f;
    private const float MENU_OPEN_POS_Y = -205f;
    private const float MENU_OPEN_TIME = 0.034f;
    public GameObject menuComp;
    public UIButton menuFrameBnt;
    public UIButton menuStateBtn;
    public UISprite menuStateImg;
    private float mFrameInPosY;
    private NoticeNumberComponent mFriendNoticeNumber;
    private bool mIsActiveShopEvent;
    private bool mIsCloseHideMode;
    private System.Action mMenuBtnAct;
    private NoticeNumberComponent mMenuNoticeNumber;
    [SerializeField]
    private GameObject mNoticeNumberPrefab;
    [SerializeField]
    private BoxCollider mOutSide;
    [SerializeField]
    private UISprite mShopEventSp;
    private NoticeNumberComponent mSummonNoticeNumber;
    public MainMenuBarButton myroomBtn;
    public static readonly Vector3 NOTICE_NUMBER_POS_DISP_BTN = new Vector3(43f, 38f, 0f);
    public static readonly Vector3 NOTICE_NUMBER_POS_MENU_BTN = new Vector3(52f, 30f, 0f);
    public MainMenuBarButton partyBtn;
    public UIPanel rootPanel;
    protected string selectedKind;
    protected SceneList.Type selectedType = SceneList.Type.None;
    public MainMenuBarButton shopBtn;
    public MainMenuBarButton summonBtn;
    private Hashtable table = new Hashtable();

    private void Awake()
    {
        this.mFrameInPosY = base.gameObject.GetLocalPositionY();
        this.mDispBtnList.Clear();
        this.mDispBtnList.Add(this.partyBtn);
        this.mDispBtnList.Add(this.summonBtn);
        this.mDispBtnList.Add(this.combineBtn);
        this.mDispBtnList.Add(this.shopBtn);
        this.mDispBtnList.Add(this.friendBtn);
        this.mDispBtnList.Add(this.myroomBtn);
        if (this.mMenuNoticeNumber == null)
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
            self.SafeSetParent(this.menuStateBtn.gameObject);
            self.SetLocalPosition(NOTICE_NUMBER_POS_MENU_BTN);
            this.mMenuNoticeNumber = self.GetComponent<NoticeNumberComponent>();
            this.mMenuNoticeNumber.SetNumber(0);
        }
        if (this.mSummonNoticeNumber == null)
        {
            GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
            obj3.SafeSetParent(this.summonBtn.gameObject);
            obj3.SetLocalPosition(NOTICE_NUMBER_POS_DISP_BTN);
            this.mSummonNoticeNumber = obj3.GetComponent<NoticeNumberComponent>();
            this.mSummonNoticeNumber.SetNumber(0);
        }
        if (this.mFriendNoticeNumber == null)
        {
            GameObject obj4 = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
            obj4.SafeSetParent(this.friendBtn.gameObject);
            obj4.SetLocalPosition(NOTICE_NUMBER_POS_DISP_BTN);
            this.mFriendNoticeNumber = obj4.GetComponent<NoticeNumberComponent>();
            this.mFriendNoticeNumber.SetNumber(0);
        }
        int enableMainEventId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).GetEnableMainEventId();
        this.mIsActiveShopEvent = enableMainEventId > 0;
        this.mShopEventSp.gameObject.SetActive(this.mIsActiveShopEvent);
        this.UpdateNoticeNumber();
    }

    public void CloseMenu(bool is_play_se = true)
    {
        this.SwitchMenu(false, is_play_se);
    }

    private void closeMenuStateImg()
    {
        this.menuStateBtn.normalSprite = "img_menu_menu";
        this.menuStateImg.MakePixelPerfect();
    }

    public void FrameIn(bool is_force = false)
    {
        this.FrameInOut(true, is_force);
    }

    public void FrameInOut(bool is_framein, bool is_force = false)
    {
        <FrameInOut>c__AnonStorey67 storey = new <FrameInOut>c__AnonStorey67 {
            tgt_obj = base.gameObject
        };
        float mFrameInPosY = this.mFrameInPosY;
        float num2 = -70f;
        storey.tgt_y = !is_framein ? num2 : mFrameInPosY;
        if (is_force || (storey.tgt_y == storey.tgt_obj.GetLocalPositionY()))
        {
            storey.tgt_obj.SetLocalPositionY(storey.tgt_y);
        }
        else
        {
            float sec = TerminalPramsManager.GetIntpTime_AutoResume(0.25f);
            Vector3 localPosition = storey.tgt_obj.GetLocalPosition();
            localPosition.y = mFrameInPosY;
            Vector3 vector2 = storey.tgt_obj.GetLocalPosition();
            vector2.y = num2;
            storey.mo = storey.tgt_obj.SafeGetComponent<MoveObject>();
            Vector3 from = !is_framein ? localPosition : vector2;
            Vector3 to = !is_framein ? vector2 : localPosition;
            storey.mo.Play(from, to, sec, new System.Action(storey.<>m__43), new System.Action(storey.<>m__44), 0f, Easing.TYPE.EXPONENTIAL_OUT);
        }
    }

    public void FrameOut(bool is_force = false)
    {
        this.FrameInOut(false, is_force);
    }

    public void OnClickCombine()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.COMBINE);
        this.SendSelectSignal(SceneList.Type.Combine, "MAIN_MENU_BAR_SELECT_COMBINE");
    }

    public void OnClickCommon(MainMenuBarButton.Kind kind)
    {
        int index = (int) kind;
        this.mDispBtnActList[index].Call();
        this.mDispBtnActList[index] = null;
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
    }

    public void OnClickFormation()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.FORMATION);
        this.SendSelectSignal(SceneList.Type.Formation, "MAIN_MENU_BAR_SELECT_PARTY");
    }

    public void OnClickFriend()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.FRIEND);
        this.SendSelectSignal(SceneList.Type.Friend, "MAIN_MENU_BAR_SELECT_FRIEND");
    }

    public void OnClickMyroom()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.MYROOM);
        this.SendSelectSignal(SceneList.Type.MyRoom, "MAIN_MENU_BAR_SELECT_MYROOM");
    }

    public void OnClickOutSide()
    {
        this.CloseMenu(true);
    }

    public void OnClickShop()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.SHOP);
        this.SendSelectSignal(SceneList.Type.Shop, "MAIN_MENU_BAR_SELECT_SHOP");
    }

    public void OnClickSummon()
    {
        this.OnClickCommon(MainMenuBarButton.Kind.SUMMON);
        this.SendSelectSignal(SceneList.Type.Summon, "MAIN_MENU_BAR_SELECT_SUMMON");
    }

    public void OnClickUnivMenu()
    {
        this.SwitchMenu(!this.isOpen, true);
        this.mMenuBtnAct.Call();
        this.mMenuBtnAct = null;
    }

    private void onMoveComplete()
    {
        this.isSwitchMenuMoving = false;
        this.SetOutSideColliderEnable(this.isOpen);
    }

    private void openMenuStateImg()
    {
        this.menuStateBtn.normalSprite = "img_menu_close";
        this.menuStateImg.MakePixelPerfect();
    }

    public void RequestSelectedSceneChange()
    {
        if (this.selectedType != SceneList.Type.None)
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.selectedType, SceneManager.FadeType.BLACK, null);
        }
    }

    public void RequestSelectedSignal()
    {
        if (this.activeSceneFSM != null)
        {
            if (this.selectedKind != null)
            {
                this.activeSceneFSM.SendEvent(this.selectedKind);
            }
            else
            {
                this.activeSceneFSM.SendEvent("MAIN_MENU_BAR_SELECT_NONE");
            }
        }
    }

    public void RequestTerminalSceneChange()
    {
        TerminalPramsManager.IsAutoResume = true;
        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
    }

    protected void SendSelectSignal(SceneList.Type type, string message)
    {
        if ((this.activeSceneFSM != null) && !string.IsNullOrEmpty(message))
        {
            this.selectedType = type;
            this.selectedKind = message;
            this.activeSceneFSM.SendEvent("MAIN_MENU_BAR_SELECT");
        }
    }

    public void SetActiveScene(SceneRootComponent scene, MainMenuBar.Kind kind, int panelDepth, Camera cam)
    {
        Debug.Log("**!! SetActiveScene scene : " + this.menuComp.transform.localPosition);
        UICamera componentInChildren = null;
        if (cam == null)
        {
            componentInChildren = scene.GetComponentInChildren<UICamera>();
        }
        else
        {
            componentInChildren = cam.gameObject.GetComponent<UICamera>();
        }
        if (componentInChildren != null)
        {
            base.transform.parent = componentInChildren.transform;
            base.gameObject.layer = componentInChildren.gameObject.layer;
        }
        this.activeSceneFSM = (scene == null) ? null : scene.myFSM;
        this.rootPanel.depth = panelDepth;
        this.SetButtonKind(kind);
        this.SetButtonActive(kind, false);
        this.CloseMenu(false);
        this.mOutSide.enabled = false;
    }

    public void SetButtonActive(MainMenuBar.Kind kind, bool isActive)
    {
        MainMenuBarButton.Mode mode = !isActive ? MainMenuBarButton.Mode.DISNABLE : MainMenuBarButton.Mode.ENABLE;
        this.partyBtn.SetMode((kind != MainMenuBar.Kind.FORMATION) ? mode : MainMenuBarButton.Mode.SELECT);
        this.summonBtn.SetMode((kind != MainMenuBar.Kind.SUMMON) ? mode : MainMenuBarButton.Mode.SELECT);
        this.combineBtn.SetMode((kind != MainMenuBar.Kind.COMBINE) ? mode : MainMenuBarButton.Mode.SELECT);
        this.shopBtn.SetMode((kind != MainMenuBar.Kind.SHOP) ? mode : MainMenuBarButton.Mode.SELECT);
        this.friendBtn.SetMode((kind != MainMenuBar.Kind.FRIEND) ? mode : MainMenuBarButton.Mode.SELECT);
        this.myroomBtn.SetMode((kind != MainMenuBar.Kind.MYROOM) ? mode : MainMenuBarButton.Mode.SELECT);
        this.selectedType = SceneList.Type.None;
        this.selectedKind = null;
    }

    public void SetButtonKind(MainMenuBar.Kind kind)
    {
        this.kind = kind;
    }

    public void SetDispBtnAct(MainMenuBarButton.Kind kind, System.Action act)
    {
        this.mDispBtnActList[(int) kind] = act;
    }

    public void SetDispBtnColliderEnable(bool is_enable, MainMenuBarButton.Kind kind = 6)
    {
        for (int i = 0; i < this.mDispBtnList.Count; i++)
        {
            MainMenuBarButton button = this.mDispBtnList[i];
            button.gameObject.GetComponent<BoxCollider>().enabled = is_enable && (i == kind);
        }
    }

    public void SetMenuActive(bool isActive)
    {
        base.gameObject.SetActive(isActive);
    }

    public void SetMenuBtnAct(System.Action act)
    {
        this.mMenuBtnAct = act;
    }

    public void SetMenuBtnColliderEnable(bool is_enable)
    {
        this.menuStateBtn.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
        if (!is_enable)
        {
            this.menuStateBtn.gameObject.GetParent().GetComponent<BoxCollider>().enabled = false;
            this.mOutSide.enabled = false;
        }
        this.menuStateBtn.state = UIButtonColor.State.Normal;
        this.menuFrameBnt.state = UIButtonColor.State.Normal;
    }

    private void SetOutSideColliderEnable(bool is_enable)
    {
        if (this.menuStateBtn.gameObject.GetComponent<BoxCollider>().enabled)
        {
            this.mOutSide.enabled = is_enable;
        }
    }

    private void SwitchMenu(bool is_open, bool is_play_se = true)
    {
        if (!this.isSwitchMenuMoving && (is_open != this.isOpen))
        {
            this.isOpen = is_open;
            this.isSwitchMenuMoving = true;
            Vector3 localPosition = this.menuComp.transform.localPosition;
            GameObject gameObject = this.menuComp.gameObject;
            this.SetOutSideColliderEnable(true);
            this.mMenuNoticeNumber.SetDisp(!is_open);
            if (is_open)
            {
                Vector3 vector2 = new Vector3(localPosition.x, -205f, localPosition.z);
                this.table.Clear();
                this.table.Add("isLocal", true);
                this.table.Add("position", vector2);
                this.table.Add("onstart", "openMenuStateImg");
                this.table.Add("onstarttarget", base.gameObject);
                this.table.Add("oncomplete", "onMoveComplete");
                this.table.Add("oncompletetarget", base.gameObject);
                this.table.Add("easetype", "easeInQuad");
                this.table.Add("time", 0.034f);
                iTween.MoveTo(gameObject, this.table);
            }
            else
            {
                float y = !this.IsCloseHideMode ? -328f : -420f;
                Vector3 vector3 = new Vector3(localPosition.x, y, localPosition.z);
                this.table.Clear();
                this.table.Add("isLocal", true);
                this.table.Add("position", vector3);
                this.table.Add("onstart", "closeMenuStateImg");
                this.table.Add("onstarttarget", base.gameObject);
                this.table.Add("oncomplete", "onMoveComplete");
                this.table.Add("oncompletetarget", base.gameObject);
                this.table.Add("easetype", "easeInQuad");
                this.table.Add("time", 0.034f);
                iTween.MoveTo(gameObject, this.table);
            }
            if (is_play_se)
            {
                SoundManager.playSystemSe(!is_open ? SeManager.SystemSeKind.UNICLOSE : SeManager.SystemSeKind.UNIOPEN);
            }
        }
    }

    public void UpdateNoticeNumber()
    {
        int number = 0;
        if (this.mSummonNoticeNumber != null)
        {
            int friendPoint = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(NetworkManager.UserId).friendPoint;
            int num3 = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getFriendPointGachaEntity().getPrice();
            number = friendPoint / num3;
            this.mSummonNoticeNumber.SetNumber(number);
        }
        int sum = 0;
        if (this.mFriendNoticeNumber != null)
        {
            sum = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetSum(FriendStatus.Kind.OFFERED);
            this.mFriendNoticeNumber.SetNumber(sum);
        }
        if (this.mMenuNoticeNumber != null)
        {
            int num5 = number + sum;
            if (this.mIsActiveShopEvent)
            {
                num5++;
            }
            num5 = 0;
            this.mMenuNoticeNumber.SetNumber(num5);
        }
    }

    public bool IsCloseHideMode
    {
        get => 
            this.mIsCloseHideMode;
        set
        {
            this.mIsCloseHideMode = value;
            if (this.mIsCloseHideMode)
            {
                this.menuComp.SetLocalPositionY(-420f);
            }
        }
    }

    public bool IsEnableOutSideCollider =>
        this.mOutSide.enabled;

    [CompilerGenerated]
    private sealed class <FrameInOut>c__AnonStorey67
    {
        internal MoveObject mo;
        internal GameObject tgt_obj;
        internal float tgt_y;

        internal void <>m__43()
        {
            this.tgt_obj.SetLocalPositionY(this.mo.Now().y);
        }

        internal void <>m__44()
        {
            this.tgt_obj.SetLocalPositionY(this.tgt_y);
        }
    }
}

