using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MasterFormationComponent : BaseMonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache26;
    [CompilerGenerated]
    private static Comparison<UserEquipEntity> <>f__am$cache27;
    [SerializeField]
    protected GameObject arrowInfo;
    [SerializeField]
    protected UICenterOnChild centerChild;
    private int cmdCnt;
    [SerializeField]
    protected CommandSpellIconComponent cmdSpellIconComp;
    [SerializeField]
    protected GameObject cmdSpellRecTimeInfo;
    [SerializeField]
    protected UITexture cmdSpellTexture;
    [SerializeField]
    protected CommandSpellWindowComponent cmdSpellWinComp;
    [SerializeField]
    protected GameObject cmdSpellWinInfo;
    [SerializeField]
    protected UIButton cmdSpepllInfoBtn;
    private GameObject currentEquipEffectObj;
    private int currentEquipId;
    private int currentIdx;
    private List<UserEquipEntity> currentList;
    private int currentMoveIdx;
    private long currentUsrEquipId;
    private MasterEquipInfoComponent equipInfoComp;
    [SerializeField]
    protected PlayMakerFSM fsm;
    private int genderType;
    public UIPanel indexPanel;
    private bool isChange;
    private bool isModify;
    [SerializeField]
    protected UIButton leftArrowBtn;
    [SerializeField]
    protected UIWrapContent loopCtr;
    private UIMasterFigureTextureOld masterFigure;
    [SerializeField]
    protected UIScrollView mScroll;
    private List<MasterEquipInfoComponent> mstEqInfoList;
    [SerializeField]
    protected GameObject mstEquipInfoPrefab;
    [SerializeField]
    protected GameObject mstImgBase;
    [SerializeField]
    protected UILabel recoverTimeLb;
    [SerializeField]
    protected UILabel recoverTitleLb;
    [SerializeField]
    protected UIButton rightArrowBtn;
    [SerializeField]
    protected UIGrid sliderGrid;
    [SerializeField]
    protected GameObject sliderIndexPrefab;
    [SerializeField]
    protected GameObject sliderInfo;
    private UserEquipEntity userEquipEntity;
    private long usrEquipId;
    private UserGameEntity usrGameEnt;
    private int usrLv;

    public void changeCmdSpellImg()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int cmdSpellImgId = this.getSpellImageId(this.genderType, (long) this.currentEquipId);
        this.cmdSpellIconComp.SetChangeCurrentCmdSepll(cmdSpellImgId, entity.getCommandSpell(), delegate {
            Vector2 sz = new Vector2(130f, 130f);
            this.cmdSpellIconComp.SetSize(sz);
            this.cmdSpellTexture.alpha = 1f;
        });
    }

    public void CloseCmdSpell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.cmdSpellWinInfo.SetActive(false);
        this.cmdSpellWinComp.Close(null);
    }

    public void closeMasterFormation()
    {
        this.isModify = false;
        this.destroyMasterFigure();
        int childCount = this.loopCtr.transform.childCount;
        int num2 = this.sliderGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(this.loopCtr.transform.GetChild(i).gameObject);
            }
        }
        if (num2 > 0)
        {
            for (int j = num2 - 1; j >= 0; j--)
            {
                UnityEngine.Object.DestroyImmediate(this.sliderGrid.transform.GetChild(j).gameObject);
            }
        }
        this.userEquipEntity.SetOld();
        this.isModify |= UserEquipNewManager.WriteData();
        this.mScroll.ResetPosition();
        base.gameObject.SetActive(false);
    }

    private void destroyMasterFigure()
    {
        if (this.masterFigure != null)
        {
            UnityEngine.Object.Destroy(this.masterFigure.gameObject);
            this.masterFigure = null;
        }
    }

    public long getCurrentUsrEquipId() => 
        this.currentUsrEquipId;

    private int getSpellImageId(int genderType, long equipId)
    {
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
        if (genderType == 2)
        {
            return entity.femaleSpellId;
        }
        return entity.maleSpellId;
    }

    public bool isChangeEquip() => 
        this.isChange;

    private void OnCenterOnChildFinished()
    {
        this.leftArrowBtn.enabled = true;
        this.rightArrowBtn.enabled = true;
        MasterEquipInfoComponent component = this.centerChild.centeredObject.GetComponent<MasterEquipInfoComponent>();
        this.currentEquipId = component.getEquipId();
        this.currentUsrEquipId = component.getUsrEquipId();
        this.isChange = component.isChangeEquip();
        component.setDispEffectObj(true);
        if (this.currentList.Count > 1)
        {
            this.currentIdx = component.getCurrentIdx();
            this.currentMoveIdx = component.getMoveBannerIdx();
            this.setSliderIcon(this.currentIdx);
        }
        this.fsm.SendEvent("CHANGE_INFO");
    }

    public void OnClickCmdSpell()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        Debug.Log("!! ** OnClickCmdSpell");
        this.cmdSpellWinInfo.SetActive(true);
        this.cmdSpellWinComp.Open(null);
    }

    public void onClickLeftArrow()
    {
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.setDisEquipEffect();
        int childCount = this.loopCtr.transform.childCount;
        int index = this.currentMoveIdx - 1;
        Debug.Log("!! ** onClickChangeBanner currentIdx: " + this.currentIdx);
        if (index < 0)
        {
            index = childCount - 1;
        }
        this.centerChild.CenterOn(this.loopCtr.transform.GetChild(index));
    }

    public void onClickRightArrow()
    {
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.setDisEquipEffect();
        int childCount = this.loopCtr.transform.childCount;
        int index = this.currentMoveIdx + 1;
        Debug.Log("!! ** onClickChangeBanner currentIdx: " + this.currentIdx);
        if (index >= childCount)
        {
            index = 0;
        }
        this.centerChild.CenterOn(this.loopCtr.transform.GetChild(index));
    }

    private void OnDragStarted()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.setDisEquipEffect();
    }

    public void setChangeMasterInfo()
    {
        this.setMstImg(this.currentEquipId);
        this.fsm.SendEvent("END_SET");
    }

    public void setCmdSpellImg()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.cmdSpellIconComp.SetData(entity);
        Vector2 sz = new Vector2(130f, 130f);
        this.cmdSpellIconComp.SetSize(sz);
        this.cmdSpellWinComp.InitializeCommandSpell(CommandSpellWindowComponent.MODE.NO_BATTLE);
        this.cmdSpellWinComp.setMode(CommandSpellWindowComponent.MODE.NO_BATTLE);
        this.cmdSpellWinComp.setCallBackPushClose(new CommandSpellWindowComponent.CloseButtonCallBack(this.CloseCmdSpell));
    }

    private void setCmdSpellRecoverTime(long recoverTime)
    {
        if (recoverTime <= 0L)
        {
            this.cmdSpellRecTimeInfo.SetActive(false);
        }
        else
        {
            int num = (int) (recoverTime / 0xe10L);
            int num2 = ((int) (recoverTime / 60L)) % 60;
            int num3 = (int) (recoverTime % 60L);
            this.recoverTitleLb.text = LocalizationManager.Get("COMMNAD_SPELL_RECV_TXT");
            this.recoverTimeLb.text = string.Format(LocalizationManager.Get("COMMAND_SPELL_RECOVER_TIME"), num, num2, num3);
        }
    }

    private void setCommandSpellInfo()
    {
        int num;
        long num2;
        this.usrGameEnt.getCmdSpellInfo(out num, out num2);
        this.setCmdSpellRecoverTime(num2);
        if (this.cmdCnt != num)
        {
            this.cmdCnt = num;
            this.setCmdSpellImg();
        }
    }

    private void setDisEquipEffect()
    {
        foreach (MasterEquipInfoComponent component in this.mstEqInfoList)
        {
            component.setDispEffectObj(false);
        }
    }

    public void setDispRePosition(int idx)
    {
        this.loopCtr.setScrollPos(idx);
        if (this.sliderInfo.activeSelf)
        {
            this.setSliderIcon(idx);
        }
        this.currentIdx = idx;
        this.currentMoveIdx = idx;
    }

    public void setMasterFormation(UserGameEntity userData)
    {
        this.usrGameEnt = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        base.gameObject.SetActive(true);
        this.arrowInfo.SetActive(false);
        this.sliderInfo.SetActive(false);
        this.mScroll.ResetPosition();
        this.usrGameEnt = userData;
        this.usrEquipId = userData.userEquipId;
        this.usrLv = userData.lv;
        int equipId = 0;
        this.currentIdx = 0;
        this.currentMoveIdx = 0;
        if (this.usrEquipId > 0L)
        {
            this.userEquipEntity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.usrEquipId);
            equipId = this.userEquipEntity.equipId;
            this.currentEquipId = equipId;
        }
        this.setMstImg(equipId);
        this.setUsrEquipData();
    }

    private void setMstEquipList()
    {
        this.mstEqInfoList = new List<MasterEquipInfoComponent>();
        bool flag = false;
        if (this.centerChild == null)
        {
            this.centerChild = this.loopCtr.gameObject.AddComponent<UICenterOnChild>();
        }
        this.mScroll.onDragStarted = (UIScrollView.OnDragNotification) Delegate.Combine(this.mScroll.onDragStarted, new UIScrollView.OnDragNotification(this.OnDragStarted));
        this.centerChild.onFinished = (SpringPanel.OnFinished) Delegate.Combine(this.centerChild.onFinished, new SpringPanel.OnFinished(this.OnCenterOnChildFinished));
        this.loopCtr.itemSize = 580;
        int count = this.currentList.Count;
        if (this.currentList.Count > 1)
        {
            count = this.currentList.Count * 2;
            flag = true;
        }
        int num2 = this.currentList.Count;
        int idx = 0;
        for (int i = 0; i < count; i++)
        {
            GameObject obj2 = base.createObject(this.mstEquipInfoPrefab, this.loopCtr.transform, null);
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = this.loopCtr.transform.localPosition;
            int num5 = i + 1;
            obj2.name = "0" + num5;
            if (num5 > 9)
            {
                obj2.name = "1" + num5;
            }
            MasterEquipInfoComponent item = obj2.GetComponent<MasterEquipInfoComponent>();
            this.mstEqInfoList.Add(item);
            if (flag)
            {
                idx = ((count / 2) - num2) + i;
                if (idx > (num2 - 1))
                {
                    idx -= num2;
                }
            }
            item.setEquipInfo(this.currentList[idx], this.usrEquipId, this.usrLv, idx, i);
        }
        this.loopCtr.SortAlphabetically();
        this.loopCtr.resetScroll();
        this.loopCtr.WrapContent();
        float num6 = this.sliderGrid.cellWidth * 0.5f;
        for (int j = 0; j < num2; j++)
        {
            base.createObject(this.sliderIndexPrefab, this.sliderGrid.transform, null).transform.localScale = Vector3.one;
        }
        if (flag)
        {
            this.arrowInfo.SetActive(true);
            this.sliderInfo.SetActive(true);
            this.sliderGrid.transform.localPosition = new Vector3(-(num6 * (num2 - 1)), 0f, 0f);
            this.sliderGrid.repositionNow = true;
            this.setSliderIcon(this.currentIdx);
        }
        this.setPosCurrentEq();
    }

    private void setMstImg(int equipId)
    {
        this.genderType = this.usrGameEnt.genderType;
        Debug.Log("***** Master equipId: " + equipId);
        Debug.Log("***** Master GenderType: " + this.genderType);
        if (this.masterFigure == null)
        {
            this.masterFigure = MasterFigureManagerOld.CreatePrefab(this.mstImgBase, UIMasterFigureRenderOld.DispType.USER_SELECT, this.genderType, equipId, 10, null);
        }
        else
        {
            Debug.Log("!!*** Master SetCharacter");
            if (<>f__am$cache26 == null)
            {
                <>f__am$cache26 = delegate {
                };
            }
            this.masterFigure.SetCharacter(UIMasterFigureRenderOld.DispType.USER_SELECT, this.genderType, equipId, <>f__am$cache26);
        }
    }

    private void setPosCurrentEq()
    {
        if ((this.usrEquipId > 0L) && (this.mstEqInfoList != null))
        {
            int count = this.mstEqInfoList.Count;
            for (int i = 0; i < count; i++)
            {
                MasterEquipInfoComponent component = this.mstEqInfoList[i];
                if (component.getUsrEquipId() == this.usrEquipId)
                {
                    int idx = component.getMoveBannerIdx();
                    this.setDispRePosition(idx);
                    break;
                }
            }
        }
    }

    private void setSliderIcon(int idx)
    {
        int childCount = this.sliderGrid.transform.childCount;
        Debug.Log("**  !!! setSliderIcon Idx: " + idx);
        for (int i = 0; i < childCount; i++)
        {
            SelectBannerSliderIcon componentInChildren = this.sliderGrid.transform.GetChild(i).GetComponentInChildren<SelectBannerSliderIcon>();
            componentInChildren.setEnableOnImg(false);
            if (i == idx)
            {
                componentInChildren.setEnableOnImg(true);
            }
        }
    }

    private void setUsrEquipData()
    {
        UserEquipEntity[] collection = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).getList(this.usrGameEnt.userId);
        this.currentList = new List<UserEquipEntity>();
        if (collection.Length > 0)
        {
            List<UserEquipEntity> list = new List<UserEquipEntity>(collection);
            if (this.usrEquipId > 0L)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    UserEquipEntity item = list[i];
                    this.currentList.Add(item);
                }
                if (<>f__am$cache27 == null)
                {
                    <>f__am$cache27 = (a, b) => a.equipId - b.equipId;
                }
                this.currentList.Sort(<>f__am$cache27);
            }
        }
        this.setMstEquipList();
    }

    private void Update()
    {
        if (this.usrGameEnt != null)
        {
            this.setCommandSpellInfo();
        }
    }
}

