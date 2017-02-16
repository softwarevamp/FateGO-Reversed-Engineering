using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UserPresentBoxWindow : BaseMonoBehaviour
{
    [SerializeField]
    protected UIButton allReceiveBtn;
    private List<int> befSvtList = new List<int>();
    [SerializeField]
    protected GameObject bgObject;
    protected ClickDelegate callbackFunc;
    [SerializeField]
    protected UIButton checkedItemBtn;
    [SerializeField]
    protected UIPanel effectPanel;
    private int getSvtIdx;
    protected GetSvts[] getSvtList;
    private bool isCheckedFlg;
    private bool isItemReceiveFlg;
    protected bool isOverFlowFlg;
    private bool isReceiveFlg;
    [SerializeField]
    protected UIButton itemReceiveBtn;
    private bool mIsScrlResetPosition;
    [SerializeField]
    protected UILabel mpValLabel;
    private System.Action mReDispAct;
    [SerializeField]
    protected PlayMakerFSM myFsm;
    [SerializeField]
    protected UILabel nonPresentNoticeLabel;
    [SerializeField]
    protected UILabel presentInfoLabel;
    [SerializeField]
    protected UILabel presentNoticeLabel;
    [SerializeField]
    protected UILabel qpValLabel;
    [SerializeField]
    protected UICommonButton sortBtn;
    [SerializeField]
    protected UILabel stoneValLabel;
    private SummonEffectComponent summonComp;
    [SerializeField]
    protected UILabel svtEquipNumValLabel;
    private ServantRewardAction svtGetAction;
    private GameObject svtGetEffectGo;
    [SerializeField]
    protected GameObject svtGetEffectPrefab;
    [SerializeField]
    protected UILabel svtNumValLabel;
    private int targetLimitCnt;
    private int targetSvtId;
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected GameObject titlePrefab;
    [SerializeField]
    protected UserPresentListViewManager userPresentListViewManager;
    protected UserServantMaster userServantMaster;

    private void callbackPresentList(string result)
    {
        this.ReDisp();
    }

    private void CallbackReceiveRequest(string result)
    {
        resData[] dataArray = JsonManager.DeserializeArray<resData>("[" + result + "]");
        this.getSvtList = dataArray[0].getSvts;
        this.isOverFlowFlg = dataArray[0].isOverflow;
        if (this.getSvtList.Length > 0)
        {
            this.myFsm.SendEvent("SHOW_EFFECT");
        }
        else if (!this.isOverFlowFlg)
        {
            this.EndReceive(false);
        }
        else if (this.isOverFlowFlg)
        {
            this.userPresentListViewManager.showErrorResultDlg(new UserPresentListViewManager.ReceiveCallbackFunc(this.EndReceive));
        }
    }

    private bool checkNew(int svtId, bool isNew)
    {
        bool flag = false;
        if (isNew)
        {
            if (this.checkOverlapSvt(svtId))
            {
                return false;
            }
            flag = true;
            this.befSvtList.Add(svtId);
        }
        return flag;
    }

    public void checkNextSvt()
    {
        if (this.getSvtIdx <= (this.getSvtList.Length - 1))
        {
            this.myFsm.SendEvent("NEXT_SVT");
        }
        else
        {
            this.getSvtIdx = 0;
            this.myFsm.SendEvent("FINAL_SVT");
        }
    }

    private bool checkOverlapSvt(int svtId)
    {
        int count = this.befSvtList.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                int num3 = this.befSvtList[i];
                if (num3 == svtId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Close()
    {
        this.userPresentListViewManager.DestroyList();
        base.gameObject.SetActive(false);
    }

    private void EndEffect()
    {
        this.myFsm.SendEvent("END_EFFECT");
    }

    private void EndEffectReceive(bool res = false)
    {
        this.ReDisp();
        this.myFsm.SendEvent("CLOSE");
    }

    private void endPlay(System.Action end_act)
    {
        <endPlay>c__AnonStorey5F storeyf = new <endPlay>c__AnonStorey5F {
            end_act = end_act,
            <>f__this = this
        };
        this.svtGetAction.Play(new System.Action(storeyf.<>m__30), SceneManager.DEFAULT_FADE_TIME);
    }

    private void EndReceive(bool res = false)
    {
        this.ReDisp();
    }

    public void incereIdx()
    {
        this.getSvtIdx++;
    }

    public void OnClickAll()
    {
        Debug.LogWarning("!! ** OnClickAll");
        if (this.isReceiveFlg)
        {
            this.mIsScrlResetPosition = true;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.userPresentListViewManager.ReceiveMultiPresent(UserPresentListViewManager.KIND.ALL);
            this.userPresentListViewManager.SetMode(UserPresentListViewManager.InitMode.HOLD);
            this.SetBtnEnable(false);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void OnClickBack()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(false);
        }
    }

    public void OnClickCheckedItem()
    {
        if (this.isCheckedFlg)
        {
            this.mIsScrlResetPosition = true;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.userPresentListViewManager.ReceiveMultiPresent(UserPresentListViewManager.KIND.CHECK);
            this.userPresentListViewManager.SetMode(UserPresentListViewManager.InitMode.HOLD);
            this.SetBtnEnable(false);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void OnClickItem()
    {
        if (this.isItemReceiveFlg)
        {
            this.mIsScrlResetPosition = true;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.userPresentListViewManager.ReceiveMultiPresent(UserPresentListViewManager.KIND.ITEM);
            this.userPresentListViewManager.SetMode(UserPresentListViewManager.InitMode.HOLD);
            this.SetBtnEnable(false);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    private void OnDestroy()
    {
        if (this.titleInfo != null)
        {
            UnityEngine.Object.Destroy(this.titleInfo.gameObject);
            this.titleInfo = null;
        }
    }

    public void Open(bool isShowBg, ClickDelegate callback, System.Action redisp_act = null)
    {
        this.mIsScrlResetPosition = true;
        this.bgObject.SetActive(isShowBg);
        this.callbackFunc = callback;
        this.isReceiveFlg = false;
        this.SetButtonTxtColor(false, this.allReceiveBtn.gameObject);
        this.isItemReceiveFlg = false;
        this.SetButtonTxtColor(false, this.itemReceiveBtn.gameObject);
        this.isCheckedFlg = false;
        this.SetButtonTxtColor(false, this.checkedItemBtn.gameObject);
        if (this.titleInfo == null)
        {
            this.titleInfo = UnityEngine.Object.Instantiate<GameObject>(this.titlePrefab).GetComponent<TitleInfoControl>();
            this.titleInfo.SetParent(base.transform);
            this.titleInfo.setDepth(0x33);
            this.titleInfo.setBackBtnDepth(0x34);
            this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.PRESENTBOX);
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.PRESENTBOX);
            this.titleInfo.SetBackBtnAct(new System.Action(this.OnClickBack));
        }
        base.gameObject.SetActive(true);
        this.sortBtn.enabled = false;
        this.sortBtn.SetState(UICommonButtonColor.State.Disabled, true);
        this.RequestPresentList();
        this.mReDispAct = redisp_act;
        this.SetBtnEnable(false);
        this.userPresentListViewManager.SetDefaultSort();
    }

    public void receivePresent(long[] presentIds)
    {
        UserPresentReceiveRequest request = NetworkManager.getRequest<UserPresentReceiveRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackReceiveRequest));
        request.addField("a", presentIds[0]);
        request.addActionField("presentreceive");
        request.beginRequest(presentIds);
        this.titleInfo.setBackBtnColliderEnable(false);
    }

    private void ReDisp()
    {
        int num;
        int num2;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        Debug.Log("!!!! PresentBox ReDisp UserGameEntity : " + entity.userId);
        this.userServantMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        this.userServantMaster.getCount(out num, out num2);
        this.svtNumValLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), num, entity.svtKeep);
        this.svtEquipNumValLabel.text = string.Format(LocalizationManager.Get("SUM_INFO"), num2, entity.svtEquipKeep);
        this.stoneValLabel.text = LocalizationManager.GetUnitInfo(entity.stone);
        this.mpValLabel.text = LocalizationManager.GetUnitInfo(entity.mana);
        string str = string.Format(LocalizationManager.Get("CURRENT_QP_UNIT"), entity.qp);
        this.qpValLabel.text = str;
        this.qpValLabel.text = str;
        UserPresentBoxEntity[] presentList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(entity.userId);
        int length = presentList.Length;
        int presentBoxMax = BalanceConfig.PresentBoxMax;
        this.presentInfoLabel.text = string.Format(LocalizationManager.Get("PRESENT_LIST_INFO"), length, presentBoxMax);
        this.presentNoticeLabel.text = string.Format(LocalizationManager.Get("PRESENT_LIST_NOTICE"), presentBoxMax);
        if (length <= 0)
        {
            this.userPresentListViewManager.DestroyList();
            this.isReceiveFlg = false;
            this.SetButtonTxtColor(false, this.allReceiveBtn.gameObject);
            this.isItemReceiveFlg = false;
            this.SetButtonTxtColor(false, this.itemReceiveBtn.gameObject);
            this.isCheckedFlg = false;
            this.SetButtonTxtColor(false, this.checkedItemBtn.gameObject);
            this.nonPresentNoticeLabel.text = LocalizationManager.Get("RECEIVE_ALL_DONE");
            this.nonPresentNoticeLabel.gameObject.SetActive(true);
            this.userPresentListViewManager.gameObject.SetActive(false);
            this.sortBtn.enabled = false;
            this.sortBtn.SetState(UICommonButtonColor.State.Disabled, false);
        }
        else
        {
            this.nonPresentNoticeLabel.gameObject.SetActive(false);
            this.isReceiveFlg = true;
            this.SetButtonTxtColor(true, this.allReceiveBtn.gameObject);
            this.isItemReceiveFlg = false;
            this.SetButtonTxtColor(false, this.itemReceiveBtn.gameObject);
            this.isCheckedFlg = false;
            this.SetButtonTxtColor(false, this.checkedItemBtn.gameObject);
            foreach (UserPresentBoxEntity entity2 in presentList)
            {
                if (entity2.giftType == 2)
                {
                    this.isItemReceiveFlg = true;
                    this.SetButtonTxtColor(true, this.itemReceiveBtn.gameObject);
                    break;
                }
            }
            this.userPresentListViewManager.CreateList(presentList);
            if (!this.mIsScrlResetPosition && (this.userPresentListViewManager.ItemSum > 2))
            {
                int index = !this.isOverFlowFlg ? (this.userPresentListViewManager.select_idx - 1) : this.userPresentListViewManager.select_idx;
                if (index < 1)
                {
                    index = 1;
                }
                else if (index >= (this.userPresentListViewManager.ItemSum - 2))
                {
                    index = this.userPresentListViewManager.ItemSum - 2;
                }
                this.userPresentListViewManager.JumpItem(index);
            }
            this.mIsScrlResetPosition = false;
            this.userPresentListViewManager.SetMode(UserPresentListViewManager.InitMode.INPUT);
            this.SetBtnEnable(true);
            this.sortBtn.enabled = true;
            this.sortBtn.SetState(UICommonButtonColor.State.Normal, false);
        }
        this.mReDispAct.Call();
        this.titleInfo.setBackBtnColliderEnable(true);
    }

    private void RequestPresentList()
    {
        NetworkManager.getRequest<UserPresentListRequest>(new NetworkManager.ResultCallbackFunc(this.callbackPresentList)).beginRequest();
    }

    private void SetBtnEnable(bool is_enable)
    {
        this.sortBtn.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
        this.allReceiveBtn.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
        this.itemReceiveBtn.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
        this.titleInfo.setBackBtnColliderEnable(is_enable);
    }

    private void SetButtonTxtColor(bool isVaild, GameObject btnObject)
    {
        foreach (UIWidget widget in btnObject.GetComponentsInChildren<UIWidget>())
        {
            widget.color = !isVaild ? Color.gray : Color.white;
        }
    }

    public void SetCheckedItemsButtonEnable(bool how, bool isSetOtherButtonsToo = false)
    {
        this.isCheckedFlg = how;
        this.SetButtonTxtColor(how, this.checkedItemBtn.gameObject);
        if (isSetOtherButtonsToo)
        {
            if (how)
            {
                this.isReceiveFlg = false;
                this.SetButtonTxtColor(false, this.allReceiveBtn.gameObject);
                this.isItemReceiveFlg = false;
                this.SetButtonTxtColor(false, this.itemReceiveBtn.gameObject);
            }
            else
            {
                UserPresentBoxEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(NetworkManager.UserId);
                if (entityArray.Length > 0)
                {
                    this.isReceiveFlg = true;
                    this.SetButtonTxtColor(true, this.allReceiveBtn.gameObject);
                    foreach (UserPresentBoxEntity entity in entityArray)
                    {
                        if (entity.giftType == 2)
                        {
                            this.isItemReceiveFlg = true;
                            this.SetButtonTxtColor(true, this.itemReceiveBtn.gameObject);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void SetReDispAction(System.Action act)
    {
        this.mReDispAct = act;
    }

    public void SetReDispFromExpiredPresent()
    {
        this.ReDisp();
    }

    public void showEffect()
    {
        <showEffect>c__AnonStorey5E storeye = new <showEffect>c__AnonStorey5E {
            <>f__this = this
        };
        Debug.Log("**  !! showEffect getSvtIdx: " + this.getSvtIdx);
        if (!this.effectPanel.gameObject.activeSelf)
        {
            this.effectPanel.gameObject.SetActive(true);
        }
        Debug.Log("!! ** UserPresentBox CallbackReceiveRequest getSvtList: " + this.getSvtList[this.getSvtIdx].userSvtId);
        storeye.targetUsrSvtId = this.getSvtList[this.getSvtIdx].userSvtId;
        UserServantEntity entity = this.userServantMaster.getEntityFromId<UserServantEntity>(storeye.targetUsrSvtId);
        ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.svtId);
        this.targetSvtId = entity.svtId;
        this.targetLimitCnt = entity.limitCount;
        bool isNew = this.getSvtList[this.getSvtIdx].isNew;
        storeye.isCheckNewSvt = this.checkNew(this.targetSvtId, isNew);
        if ((entity2.IsExpUp || entity2.IsStatusUp) || !storeye.isCheckNewSvt)
        {
            this.EndEffect();
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(storeye.<>m__2F));
        }
    }

    private void ShowExpiredPresents(System.Action callback)
    {
        <ShowExpiredPresents>c__AnonStorey5D storeyd = new <ShowExpiredPresents>c__AnonStorey5D {
            callback = callback,
            <>f__this = this
        };
        string expiredPresents = this.userPresentListViewManager.expiredPresents;
        if ((expiredPresents != null) && (expiredPresents.Length > 0))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(null, expiredPresents + "\n\n" + LocalizationManager.Get("PRESENT_EXPIRED_ERROR_MESSAGE"), new System.Action(storeyd.<>m__2E), -1);
        }
        else
        {
            storeyd.callback.Call();
        }
    }

    public void showReceiveResultDlg()
    {
        if (!this.isOverFlowFlg)
        {
            this.EndEffectReceive(false);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, delegate {
            if (this.isOverFlowFlg)
            {
                this.userPresentListViewManager.showErrorResultDlg(new UserPresentListViewManager.ReceiveCallbackFunc(this.EndEffectReceive));
            }
        });
    }

    [CompilerGenerated]
    private sealed class <endPlay>c__AnonStorey5F
    {
        internal UserPresentBoxWindow <>f__this;
        internal System.Action end_act;

        internal void <>m__30()
        {
            UnityEngine.Object.DestroyImmediate(this.<>f__this.svtGetEffectGo);
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <showEffect>c__AnonStorey5E
    {
        internal UserPresentBoxWindow <>f__this;
        internal bool isCheckNewSvt;
        internal long targetUsrSvtId;

        internal void <>m__2F()
        {
            if (this.<>f__this.svtGetEffectGo == null)
            {
                this.<>f__this.svtGetEffectGo = this.<>f__this.createObject(this.<>f__this.svtGetEffectPrefab, this.<>f__this.effectPanel.transform, null);
                this.<>f__this.svtGetEffectGo.transform.localPosition = Vector3.zero;
                this.<>f__this.svtGetEffectGo.transform.localScale = Vector3.one;
            }
            this.<>f__this.svtGetAction = this.<>f__this.svtGetEffectGo.GetComponent<ServantRewardAction>();
            this.<>f__this.svtGetAction.Setup(this.<>f__this.targetSvtId, this.targetUsrSvtId, this.<>f__this.targetLimitCnt, 1, this.isCheckNewSvt, ServantRewardAction.PLAY_FLAG.FADE_OUT | ServantRewardAction.PLAY_FLAG.FADE_IN);
            this.<>f__this.endPlay(new System.Action(this.<>f__this.EndEffect));
        }
    }

    [CompilerGenerated]
    private sealed class <ShowExpiredPresents>c__AnonStorey5D
    {
        internal UserPresentBoxWindow <>f__this;
        internal System.Action callback;

        internal void <>m__2E()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
            this.<>f__this.userPresentListViewManager.resetCheckStatus();
            this.<>f__this.userPresentListViewManager.expiredPresents = string.Empty;
            this.callback.Call();
        }
    }

    public delegate void ClickDelegate(bool isDecide);

    protected enum QUESTTYPE
    {
        FRIENDSHIP = 3,
        HEROBALLAD = 6
    }

    public class resData
    {
        public GetSvts[] getSvts;
        public bool isOverflow;
    }
}

