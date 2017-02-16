using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantStatusDialog : BaseMonoBehaviour
{
    protected BattleFBXComponent battleActor;
    [SerializeField]
    protected UICommonButton battleButton;
    [SerializeField]
    protected UISprite[] battleCharaCloseSpriteList;
    [SerializeField]
    protected UICommonButton[] battleCharaLevelButtonList;
    [SerializeField]
    protected UISprite[] battleCharaLevelSpriteList;
    [SerializeField]
    protected UISprite[] battleCharaLevelTitleSpriteList;
    protected GameObject battleChr;
    [SerializeField]
    protected Camera battleChrCamera;
    protected string BattleChrId;
    [SerializeField]
    protected UILabel battleExplanationLabel;
    protected List<string> battleLoadList;
    [SerializeField]
    protected UISprite battleSprite;
    [SerializeField]
    protected GameObject battleTabBase;
    [SerializeField]
    protected UISprite battleTitleSprite;
    protected ClickDelegate callbackFunc;
    [SerializeField]
    protected ServantStatusCharaGraphListViewManager charaGraphListViewManager;
    [SerializeField]
    protected UISprite equipSprite;
    [SerializeField]
    protected UICommonButton favoriteButton;
    [SerializeField]
    protected UISprite favoriteSprite;
    protected bool isBattlePlay;
    protected bool isBgmLow;
    protected bool isExit;
    protected bool isInit;
    protected bool isInitTab;
    protected bool isModify;
    protected bool isUseFavorite;
    protected Kind kind;
    [SerializeField]
    protected GameObject loadingObject;
    [SerializeField]
    protected UICommonButton lockButton;
    [SerializeField]
    protected UISprite lockSprite;
    protected ServantStatusListViewItem mainInfo;
    [SerializeField]
    protected GameObject markBase;
    [SerializeField]
    protected UICommonButton profileButton;
    [SerializeField]
    protected ShiningIconComponent profileNewIcon;
    protected int[] profileNewList;
    [SerializeField]
    protected UISprite profileSprite;
    [SerializeField]
    protected GameObject profileTabBase;
    [SerializeField]
    protected ServantStatusFlavorTextListViewManager profileTabListViewManager;
    [SerializeField]
    protected UISprite profileTitleSprite;
    protected List<string> requestVoiceDataList = new List<string>();
    [SerializeField]
    protected UILabel servantClassNameLabel;
    [SerializeField]
    protected UILabel servantNameLabel;
    [SerializeField]
    protected UICommonButton statusButton;
    protected string StatusChrId;
    [SerializeField]
    protected UISprite statusSprite;
    [SerializeField]
    protected GameObject statusTabBase;
    [SerializeField]
    protected ServantStatusListViewManager statusTabListViewManager;
    [SerializeField]
    protected UISprite statusTitleSprite;
    protected bool[] tabInitList = new bool[4];
    protected TabKind tabKind;
    private Coroutine Talkdata;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected UICommonButton voiceButton;
    protected List<string> voiceDataList = new List<string>();
    protected int voiceListIndex;
    protected string voicePlayAssetName;
    protected SePlayer voicePlayer;
    protected ServantVoiceData[] voicePlayList;
    protected int voicePlayNum;
    [SerializeField]
    protected UISprite voiceSprite;
    [SerializeField]
    protected GameObject voiceTabBase;
    [SerializeField]
    protected ServantStatusVoiceListViewManager voiceTabListViewManager;
    [SerializeField]
    protected UISprite voiceTitleSprite;

    protected void ChangeBattleResource(int dispLv)
    {
        this.mainInfo.DispLimitCount = dispLv;
        int limitCountByImageLimit = ImageLimitCount.GetLimitCountByImageLimit(dispLv);
        string str = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD).getBattleChrId(this.mainInfo.SvtId, limitCountByImageLimit);
        string item = "Servants/" + str;
        if (this.battleLoadList.Contains(item))
        {
            this.EndChangeBattleResource();
        }
        else
        {
            if (this.BattleChrId != this.StatusChrId)
            {
                string str3 = "Servants/" + this.BattleChrId;
                string[] strArray = new string[] { str3 };
                AssetManager.releaseAssetStorage(strArray);
                this.battleLoadList.Remove(str3);
            }
            this.battleLoadList.Add(item);
            this.BattleChrId = str;
            string[] nameList = new string[] { item };
            AssetManager.loadAssetStorage(nameList, new System.Action(this.EndChangeBattleResource));
        }
    }

    protected void ChangeCommandResource(int dispLv)
    {
        this.mainInfo.CommandCardLimitCount = dispLv;
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.COMMAND);
    }

    protected void ChangeFaceResource(int dispLv)
    {
        this.mainInfo.FaceLimitCount = dispLv;
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.FACE);
    }

    public void Close(System.Action callback)
    {
        this.statusTabListViewManager.DestroyList();
        this.profileTabListViewManager.DestroyList();
        this.charaGraphListViewManager.DestroyList();
        this.mainInfo = null;
        if ((this.battleLoadList != null) && (this.battleLoadList.Count > 0))
        {
            AssetManager.releaseAssetStorage(this.battleLoadList.ToArray());
            this.battleLoadList = null;
        }
        this.requestVoiceDataList.Clear();
        this.StopVoice();
        if (this.voiceDataList.Count > 0)
        {
            foreach (string str in this.voiceDataList)
            {
                SoundManager.releaseAudioAssetStorage(str);
            }
            this.voiceDataList.Clear();
        }
        base.gameObject.SetActive(false);
        if (callback != null)
        {
            callback();
        }
    }

    protected void EndChangeBattleResource()
    {
        this.isBattlePlay = false;
        this.PlayBattleEffect(true);
        int limitCountByImageLimit = ImageLimitCount.GetLimitCountByImageLimit(this.mainInfo.DispLimitCount);
        this.battleActor.SetEvolutionLevel(this.mainInfo.SvtId, limitCountByImageLimit);
        this.SetupBattleButton(false);
    }

    protected void EndCloseConfirmSelectFavorite()
    {
    }

    protected void EndCloseSelectEquip()
    {
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.INPUT, new ServantStatusListViewManager.CallbackFunc(this.OnSelectStatus));
    }

    protected void EndCloseSelectEquipStatus()
    {
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.INPUT, new ServantStatusListViewManager.CallbackFunc(this.OnSelectStatus));
    }

    protected void EndeCardFavoriteRequest(string result)
    {
        this.EquipRequest();
    }

    protected void EndeRequest()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(this.isModify);
        }
    }

    protected void EndLoadBattleResource()
    {
        if (this.battleLoadList != null)
        {
            this.loadingObject.SetActive(false);
            this.PlayBattleEffect(false);
            if (this.tabKind == TabKind.BATTLE)
            {
                this.SetupBattleButton(false);
            }
        }
    }

    protected void EndLoadVoice()
    {
        if ((this.requestVoiceDataList != null) && (this.requestVoiceDataList.Count > 0))
        {
            this.voiceDataList.Add(this.requestVoiceDataList[0]);
            this.requestVoiceDataList.RemoveAt(0);
            if (this.requestVoiceDataList.Count > 0)
            {
                SoundManager.loadAudioAssetStorage(this.requestVoiceDataList[0], new System.Action(this.EndLoadVoice), SoundManager.CueType.ALL);
            }
            else if (this.kind == Kind.ENEMY_COLLECTION_DETAIL)
            {
                this.loadingObject.SetActive(false);
            }
            else
            {
                ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
                this.StatusChrId = master.getBattleChrId(this.mainInfo.SvtId, this.mainInfo.LimitCount);
                int limitCountByImageLimit = ImageLimitCount.GetLimitCountByImageLimit(this.mainInfo.DispLimitCount);
                this.BattleChrId = master.getBattleChrId(this.mainInfo.SvtId, limitCountByImageLimit);
                this.battleLoadList.Add("Servants/" + this.BattleChrId);
                if (this.StatusChrId != this.BattleChrId)
                {
                    this.battleLoadList.Add("Servants/" + this.StatusChrId);
                }
                AssetManager.loadAssetStorage(this.battleLoadList.ToArray(), new System.Action(this.EndLoadBattleResource));
            }
        }
    }

    protected void EndPlayVoice()
    {
        this.voicePlayer = null;
        if (this.voicePlayList == null)
        {
            if ((this.tabKind == TabKind.VOICE) && (this.voiceListIndex >= 0))
            {
                this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.PLAY, -1);
                this.voiceListIndex = -1;
                if (this.Talkdata != null)
                {
                    SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
                }
            }
        }
        else
        {
            this.voicePlayNum++;
            if (this.voicePlayNum < this.voicePlayList.Length)
            {
                base.Invoke("EndWaitVoice", this.voicePlayList[this.voicePlayNum].delay);
            }
            else
            {
                if ((this.tabKind == TabKind.VOICE) && (this.voiceListIndex >= 0))
                {
                    this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.PLAY, -1);
                    this.voiceListIndex = -1;
                    if (this.Talkdata != null)
                    {
                        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
                    }
                }
                this.voicePlayList = null;
                this.voicePlayNum = 0;
                this.voicePlayAssetName = null;
            }
        }
    }

    protected void EndRequestEquipSet(string result)
    {
        this.EndeRequest();
    }

    protected void EndWaitVoice()
    {
        if (this.voicePlayList != null)
        {
            if (this.voicePlayNum < this.voicePlayList.Length)
            {
                this.voicePlayer = SoundManager.playVoice(this.voicePlayAssetName, this.voicePlayList[this.voicePlayNum].id, SoundManager.DEFAULT_VOLUME, new System.Action(this.EndPlayVoice));
            }
            else
            {
                if ((this.tabKind == TabKind.VOICE) && (this.voiceListIndex >= 0))
                {
                    this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.PLAY, -1);
                    this.voiceListIndex = -1;
                }
                this.voicePlayList = null;
                this.voicePlayNum = 0;
                this.voicePlayAssetName = null;
            }
        }
    }

    protected void EquipRequest()
    {
        this.EndeRequest();
    }

    protected string GetVoiceAssetName(VoiceAssetType assetType, int svtId, int limitCount, int seqId = 0)
    {
        int num = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD).getVoiceId(svtId, limitCount);
        switch (assetType)
        {
            case VoiceAssetType.BATTLE:
                return ("Servants_" + num);

            case VoiceAssetType.HOME:
                return ("ChrVoice_" + num);

            case VoiceAssetType.NP:
                return ("NoblePhantasm_" + num);
        }
        return null;
    }

    protected void Init()
    {
        this.isInit = true;
        base.gameObject.SetActive(true);
        SvtType.Type type = (SvtType.Type) this.mainInfo.Servant.type;
        this.isUseFavorite = (TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2) && (this.mainInfo.FavoriteUserSvtId > 0L)) && SvtType.IsOrganization(type);
        if (SvtType.IsEnemyCollectionDetail(type))
        {
            this.kind = Kind.ENEMY_COLLECTION_DETAIL;
        }
        else if (SvtType.IsServantEquip(type))
        {
            if ((this.kind == Kind.FRIEND) || (this.kind == Kind.FRIEND_SERVANT_EQUIP))
            {
                this.kind = Kind.FRIEND_SERVANT_EQUIP;
            }
            else if ((this.kind == Kind.FOLLOWER) || (this.kind == Kind.FOLLOWER_SERVANT_EQUIP))
            {
                this.kind = Kind.FOLLOWER_SERVANT_EQUIP;
            }
            else if ((this.kind == Kind.FIRST_SINGLE_GET) || (this.kind == Kind.FIRST_SINGLE_GET_SERVANT_EQUIP))
            {
                this.kind = Kind.FIRST_SINGLE_GET_SERVANT_EQUIP;
            }
            else if ((this.kind == Kind.DROP) || (this.kind == Kind.DROP_SERVANT_EQUIP))
            {
                this.kind = Kind.DROP_SERVANT_EQUIP;
            }
            else if ((this.kind == Kind.SUMMON) || (this.kind == Kind.SUMMON_SERVANT_EQUIP))
            {
                this.kind = Kind.SUMMON_SERVANT_EQUIP;
            }
            else if ((this.kind == Kind.COMBINE) || (this.kind == Kind.COMBINE_SERVANT_EQUIP))
            {
                this.kind = Kind.COMBINE_SERVANT_EQUIP;
            }
            else
            {
                this.kind = (this.kind != Kind.COMBINE_MATERIAL_EQUIP) ? Kind.SERVANT_EQUIP : Kind.COMBINE_MATERIAL_EQUIP;
            }
        }
        else if (!SvtType.IsServant(type))
        {
            this.kind = (this.kind != Kind.COMBINE_MATERIAL_SVT) ? Kind.COMBINE_MATERIAL : Kind.COMBINE_MATERIAL_NOSVT;
        }
        this.isInitTab = false;
        this.isExit = false;
        this.isBgmLow = false;
        for (int i = 0; i < 4; i++)
        {
            this.tabInitList[i] = false;
        }
        if (this.mainInfo.UserServantCollection != null)
        {
            this.profileNewList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetNewList(this.mainInfo.SvtId);
            this.profileNewIcon.Set(this.profileNewList.Length > 0);
        }
        else
        {
            this.profileNewIcon.Clear();
        }
        this.titleInfo.setTitleInfo(null, true, null, TitleInfoControl.TitleKind.NONE);
        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.NONE);
        this.servantNameLabel.text = this.mainInfo.Servant.name;
        this.servantClassNameLabel.text = !this.mainInfo.Servant.IsServantEquip ? this.mainInfo.Servant.getClassName() : LocalizationManager.Get("SERVANT_EQUIP_TAKE");
        this.battleExplanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_BATTLE_EXPLANATION");
        this.charaGraphListViewManager.CreateList(this.mainInfo);
        this.SetMark();
        this.SetTabKind((((this.kind != Kind.COMBINE_MATERIAL) && (this.kind != Kind.COMBINE_MATERIAL_NOSVT)) && (this.kind != Kind.ENEMY_COLLECTION_DETAIL)) ? TabKind.STATUS : TabKind.PROFILE);
        this.battleLoadList = new List<string>();
        this.isBattlePlay = false;
        switch (this.kind)
        {
            case Kind.COMBINE_MATERIAL:
            case Kind.SERVANT_EQUIP:
            case Kind.FRIEND_SERVANT_EQUIP:
            case Kind.FOLLOWER_SERVANT_EQUIP:
            case Kind.FIRST_SINGLE_GET_SERVANT_EQUIP:
            case Kind.DROP_SERVANT_EQUIP:
            case Kind.SUMMON_SERVANT_EQUIP:
            case Kind.COMBINE_SERVANT_EQUIP:
            case Kind.COMBINE_MATERIAL_NOSVT:
            case Kind.COMBINE_MATERIAL_EQUIP:
                this.loadingObject.SetActive(false);
                break;

            default:
                this.loadingObject.SetActive(true);
                if ((this.requestVoiceDataList.Count == 0) && (this.voiceDataList.Count == 0))
                {
                    int[] numArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD).getVoiceLimitCountList(this.mainInfo.SvtId);
                    for (int j = 0; j < numArray.Length; j++)
                    {
                        this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.BATTLE, this.mainInfo.SvtId, numArray[j], 0));
                        this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.NP, this.mainInfo.SvtId, numArray[j], 0));
                        this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.HOME, this.mainInfo.SvtId, numArray[j], 0));
                        if (this.mainInfo.SvtId == BalanceConfig.ServantIdJekyll)
                        {
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.BATTLE, BalanceConfig.ServantIdHyde, numArray[j], 0));
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.NP, BalanceConfig.ServantIdHyde, numArray[j], 0));
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.HOME, BalanceConfig.ServantIdHyde, numArray[j], 0));
                        }
                        else if (this.mainInfo.SvtId == BalanceConfig.ServantIdMashu1)
                        {
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.BATTLE, BalanceConfig.ServantIdMashu2, numArray[j], 0));
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.NP, BalanceConfig.ServantIdMashu2, numArray[j], 0));
                            this.requestVoiceDataList.Add(this.GetVoiceAssetName(VoiceAssetType.HOME, BalanceConfig.ServantIdMashu2, numArray[j], 0));
                        }
                    }
                    this.StartVoiceLoad();
                }
                break;
        }
        this.InitList();
        this.isInit = false;
        if (this.isUseFavorite && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialog(LocalizationManager.Get("TUTORIAL_MESSAGE_FAVORITE1"), TutorialFlag.Id.TUTORIAL_LABEL_FAVORITE2, null);
        }
    }

    protected void InitList()
    {
        this.charaGraphListViewManager.SetMode(ServantStatusCharaGraphListViewManager.InitMode.INPUT, new ServantStatusCharaGraphListViewManager.CallbackFunc(this.OnSelectCharaGraph));
    }

    public void OnClickBattleCharaLevel1()
    {
        if ((!this.isExit && this.isBattlePlay) && (this.mainInfo.DispLimitCount != 0))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ChangeBattleResource(0);
        }
    }

    public void OnClickBattleCharaLevel2()
    {
        if ((!this.isExit && this.isBattlePlay) && (this.mainInfo.DispLimitCount != 1))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ChangeBattleResource(1);
        }
    }

    public void OnClickBattleCharaLevel3()
    {
        if ((!this.isExit && this.isBattlePlay) && (this.mainInfo.DispLimitCount != 2))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ChangeBattleResource(2);
        }
    }

    public void OnClickCancel()
    {
        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
        this.Talkdata = null;
        if (!this.isExit)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.QuitList();
            this.isModify = false;
            if (((this.mainInfo.UserServant != null) && (this.kind != Kind.FIRST_SINGLE_GET)) && (this.kind != Kind.FIRST_SINGLE_GET_SERVANT_EQUIP))
            {
                this.mainInfo.UserServant.SetOld();
            }
            if (this.mainInfo.UserServantCollection != null)
            {
                this.mainInfo.UserServantCollection.SetOld();
            }
            if ((this.mainInfo.ServantLeaderData != null) && (this.kind == Kind.FRIEND))
            {
                OtherUserNewManager.SetOld(this.mainInfo.ServantLeaderData.userId);
            }
            this.isModify |= UserServantLockManager.WriteData();
            this.isModify |= UserServantNewManager.WriteData();
            this.isModify |= UserServantCollectionManager.WriteData();
            this.isModify |= ServantCommentManager.WriteData();
            this.isModify |= OtherUserNewManager.WriteData();
            if (this.mainInfo.UserServant != null)
            {
                int otherImageLimitCount;
                ServantStatusCharaGraphListViewItem selectItem = this.charaGraphListViewManager.GetSelectItem();
                int imageLimitCount = selectItem.ImageLimitCount;
                bool flag = ((!this.mainInfo.IsCardReal && this.mainInfo.IsChangeImageLimitCount) && (selectItem != null)) && (imageLimitCount != this.mainInfo.CardImageLimitCount);
                if (imageLimitCount == BalanceConfig.OtherImageLimitCount)
                {
                    otherImageLimitCount = BalanceConfig.OtherImageLimitCount;
                }
                else if (flag)
                {
                    otherImageLimitCount = imageLimitCount + 1;
                }
                else
                {
                    otherImageLimitCount = this.mainInfo.UserServant.imageLimitCount;
                }
                bool isModifyDispLimitCount = this.mainInfo.IsModifyDispLimitCount;
                int dispLimitCount = !isModifyDispLimitCount ? this.mainInfo.UserServant.dispLimitCount : (this.mainInfo.DispLimitCount + 1);
                bool isModifyCommandCardLimitCount = this.mainInfo.IsModifyCommandCardLimitCount;
                int commandCardLimitCount = !isModifyCommandCardLimitCount ? this.mainInfo.UserServant.commandCardLimitCount : (this.mainInfo.CommandCardLimitCount + 1);
                bool isModifyFaceLimitCount = this.mainInfo.IsModifyFaceLimitCount;
                int iconLimitCount = !isModifyFaceLimitCount ? this.mainInfo.UserServant.iconLimitCount : (this.mainInfo.FaceLimitCount + 1);
                bool isFavorite = this.isUseFavorite && this.mainInfo.IsModifyFavoriteUserSvtId();
                bool isModifyLock = this.mainInfo.IsModifyLock;
                if ((isFavorite || flag) || ((isModifyDispLimitCount || isModifyCommandCardLimitCount) || isModifyFaceLimitCount))
                {
                    this.isModify = true;
                    NetworkManager.getRequest<CardFavoriteRequest>(new NetworkManager.ResultCallbackFunc(this.EndeCardFavoriteRequest)).beginRequest(this.mainInfo.UserServant.id, otherImageLimitCount, dispLimitCount, commandCardLimitCount, iconLimitCount, isFavorite);
                    return;
                }
            }
            this.EquipRequest();
        }
    }

    public void OnClickCommandCharaLevel(int dispLv)
    {
        if (((!this.isExit && !this.isInit) && this.isBattlePlay) && (this.mainInfo.CommandCardLimitCount != dispLv))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ChangeCommandResource(dispLv);
        }
    }

    public void OnClickFaceCharaLevel(int dispLv)
    {
        if ((!this.isExit && !this.isInit) && (this.mainInfo.FaceLimitCount != dispLv))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.ChangeFaceResource(dispLv);
        }
    }

    public void OnClickFavorite()
    {
        if (!this.isExit && ((this.mainInfo.UserGame != null) && (this.mainInfo.UserServant != null)))
        {
            long favoriteUserSvtId = this.mainInfo.FavoriteUserSvtId;
            if (favoriteUserSvtId == this.mainInfo.UserServant.id)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
            else if (favoriteUserSvtId <= 0L)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.mainInfo.FavoriteUserSvtId = this.mainInfo.UserServant.id;
                this.SetMark();
            }
            else if (this.mainInfo.IsEventJoin)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_EVENT_JOIN_TITLE"), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_EVENT_JOIN_MESSAGE"), new System.Action(this.EndCloseConfirmSelectFavorite), -1);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(favoriteUserSvtId);
                ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.svtId);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDialog(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_TITLE"), string.Format(LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_MESSAGE"), new object[] { entity2.getClassName(), entity2.name, this.mainInfo.Servant.getClassName(), this.mainInfo.Servant.name }), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_DECIDE"), LocalizationManager.Get("SERVANT_STATUS_FAVORITE_CONFIRM_CANCEL"), new CommonConfirmDialog.ClickDelegate(this.OnConfirmSelectFavorite));
            }
        }
    }

    public void OnClickLock()
    {
        if (!this.isExit && ((this.mainInfo.UserGame != null) && (this.mainInfo.UserServant != null)))
        {
            NetworkManager.getRequest<UserServantLockRequest>(new NetworkManager.ResultCallbackFunc(this.OnLockStateCallBack)).beginRequest(this.mainInfo.UserServant.id, (long) this.mainInfo.UserServant.getSvtId(), !this.mainInfo.UserServant.IsLock() ? 1 : 0);
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
        }
    }

    public void OnClickTabBattle()
    {
        if (!this.isExit)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.BATTLE);
        }
    }

    public void OnClickTabProfile()
    {
        if (!this.isExit)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.PROFILE);
        }
    }

    public void OnClickTabStatus()
    {
        if (!this.isExit)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.STATUS);
        }
    }

    public void OnClickTabVoice()
    {
        if (!this.isExit)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SetTabKind(TabKind.VOICE);
        }
    }

    protected void OnConfirmSelectFavorite(bool isDecide)
    {
        if (isDecide)
        {
            this.mainInfo.FavoriteUserSvtId = this.mainInfo.UserServant.id;
            this.SetMark();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.EndCloseConfirmSelectFavorite));
    }

    public void OnLockStateCallBack(string result)
    {
        if (result != "ng")
        {
            this.mainInfo.UserServant.ChangeLock();
            this.SetMark();
        }
    }

    protected void OnSelectCharaGraph(int result)
    {
    }

    protected void OnSelectEquip(EquipGraphListMenu.ResultKind result, EquipGraphListViewItem item)
    {
        if (result == EquipGraphListMenu.ResultKind.DECIDE)
        {
            this.mainInfo.SetEquipTargetId1((item == null) ? 0L : item.UserServant.id);
            this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseEquipGraphListMenu(new System.Action(this.EndCloseSelectEquip));
    }

    protected void OnSelectEquipStatus(bool isDecide)
    {
        if (isDecide)
        {
            this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.MODIFY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(new System.Action(this.EndCloseSelectEquipStatus));
    }

    protected void OnSelectFlavor(int result)
    {
        this.profileTabListViewManager.SetMode(ServantStatusFlavorTextListViewManager.InitMode.INPUT, new ServantStatusFlavorTextListViewManager.CallbackFunc(this.OnSelectFlavor));
    }

    protected void OnSelectStatus(ServantStatusListViewItemDraw.Kind kind, ServantStatusListViewManager.ResultKind result)
    {
        if ((result == ServantStatusListViewManager.ResultKind.EQUIP1) && !this.mainInfo.IsEquipChangeMode)
        {
            result = ServantStatusListViewManager.ResultKind.EQUIP1_STATUS;
        }
        Debug.LogError("AAAAAAAAAAAAAAA:restult:" + result);
        switch (result)
        {
            case ServantStatusListViewManager.ResultKind.SELECT:
                if (kind == ServantStatusListViewItemDraw.Kind.MAIN)
                {
                    if (this.mainInfo.UserGame != null)
                    {
                    }
                    break;
                }
                break;

            case ServantStatusListViewManager.ResultKind.EQUIP1:
            case ServantStatusListViewManager.ResultKind.EQUIP1_STATUS:
                if ((this.mainInfo.UserServant == null) || (this.mainInfo.EquipTargetId1 <= 0L))
                {
                    if (((this.mainInfo.ServantLeaderData != null) && (this.mainInfo.ServantLeaderData.equipTarget1 != null)) && (this.mainInfo.ServantLeaderData.equipTarget1.userSvtId > 0L))
                    {
                        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                        if (this.kind == Kind.FRIEND)
                        {
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(Kind.FRIEND_SERVANT_EQUIP, this.mainInfo.ServantLeaderData.equipTarget1, new ClickDelegate(this.OnSelectEquipStatus));
                        }
                        else if (this.kind == Kind.FOLLOWER)
                        {
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(Kind.FOLLOWER_SERVANT_EQUIP, this.mainInfo.ServantLeaderData.equipTarget1, new ClickDelegate(this.OnSelectEquipStatus));
                        }
                        else
                        {
                            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(Kind.DROP_SERVANT_EQUIP, this.mainInfo.ServantLeaderData.equipTarget1, new ClickDelegate(this.OnSelectEquipStatus));
                        }
                        return;
                    }
                    break;
                }
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(Kind.SERVANT_EQUIP, this.mainInfo.EquipTargetId1, true, new ClickDelegate(this.OnSelectEquipStatus));
                return;

            case ServantStatusListViewManager.ResultKind.COMMAND1:
                this.OnClickCommandCharaLevel(0);
                break;

            case ServantStatusListViewManager.ResultKind.COMMAND2:
                this.OnClickCommandCharaLevel(1);
                break;

            case ServantStatusListViewManager.ResultKind.COMMAND3:
                this.OnClickCommandCharaLevel(2);
                break;

            case ServantStatusListViewManager.ResultKind.FACE1:
                this.OnClickFaceCharaLevel(0);
                break;

            case ServantStatusListViewManager.ResultKind.FACE2:
                this.OnClickFaceCharaLevel(1);
                break;

            case ServantStatusListViewManager.ResultKind.FACE3:
                this.OnClickFaceCharaLevel(2);
                break;

            case ServantStatusListViewManager.ResultKind.FACE4:
                this.OnClickFaceCharaLevel(3);
                break;
        }
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.INPUT, new ServantStatusListViewManager.CallbackFunc(this.OnSelectStatus));
    }

    protected void OnSelectVoice(ServantStatusVoiceListViewManager.ResultKind kind, int result)
    {
        this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.INPUT, new ServantStatusVoiceListViewManager.CallbackFunc(this.OnSelectVoice));
        if (kind != ServantStatusVoiceListViewManager.ResultKind.PLAY)
        {
            if (kind == ServantStatusVoiceListViewManager.ResultKind.STOP)
            {
                this.StopVoice();
            }
            return;
        }
        ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        ServantVoiceMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantVoiceMaster>(DataNameKind.Kind.SERVANT_VOICE);
        ServantStatusVoiceListViewItem item = this.voiceTabListViewManager.GetItem(result);
        int svtId = item.SvtId;
        int limitCount = item.LimitCount;
        int num3 = master.getVoicePrefix(svtId, limitCount);
        object[] objArray1 = new object[] { string.Empty, num3, "_", item.LabelName };
        string labelName = string.Concat(objArray1);
        List<ServantVoiceData[]> voicePlayListList = null;
        switch (item.PlayType)
        {
            case SvtVoiceType.Type.HOME:
            case SvtVoiceType.Type.GROETH:
            case SvtVoiceType.Type.FIRST_GET:
                switch (item.CondType)
                {
                    case CondType.Kind.SVT_LEVEL:
                        voicePlayListList = master2.getLevelUpVoiceList(svtId, limitCount);
                        goto Label_01D3;

                    case CondType.Kind.SVT_LIMIT:
                        if ((item.CondValue == 1) || (item.CondValue == 3))
                        {
                            voicePlayListList = master2.getSpecificLimitCntUpVoiceList(svtId, item.CondValue);
                        }
                        else
                        {
                            voicePlayListList = master2.getLimitCntUpVoiceList(svtId, limitCount);
                        }
                        goto Label_01D3;

                    case CondType.Kind.SVT_GET:
                        voicePlayListList = master2.getFirstGetVoiceList(svtId, limitCount);
                        goto Label_01D3;

                    case CondType.Kind.SVT_FRIENDSHIP:
                        voicePlayListList = master2.getRankUpFriendShip(svtId, limitCount, item.CondValue);
                        goto Label_01D3;

                    case CondType.Kind.SVT_COUNT_STOP:
                        voicePlayListList = master2.getCntStopVoiceList(svtId, limitCount);
                        goto Label_01D3;
                }
                break;

            case SvtVoiceType.Type.EVENT_JOIN:
                voicePlayListList = master2.getEventJoinVoiceList(svtId, limitCount);
                this.PlayChrVoice(svtId, limitCount, voicePlayListList, result);
                goto Label_0221;

            case SvtVoiceType.Type.EVENT_REWARD:
                voicePlayListList = master2.getEventRewardVoiceList(svtId, limitCount, labelName);
                this.PlayChrVoice(svtId, limitCount, voicePlayListList, result);
                goto Label_0221;

            case SvtVoiceType.Type.BATTLE:
                voicePlayListList = master2.getBattleVoiceList(svtId, limitCount, labelName);
                this.PlayBattleVoice(svtId, limitCount, voicePlayListList, result);
                goto Label_0221;

            case SvtVoiceType.Type.TREASURE_DEVICE:
                voicePlayListList = master2.getNpVoiceList(svtId, limitCount, labelName);
                this.PlayNpVoice(svtId, limitCount, voicePlayListList, result);
                goto Label_0221;

            default:
                goto Label_0221;
        }
        voicePlayListList = master2.getHomeVoiceList(svtId, limitCount, labelName);
    Label_01D3:
        this.PlayChrVoice(svtId, limitCount, voicePlayListList, result);
    Label_0221:
        if (this.Talkdata != null)
        {
            SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
            this.Talkdata = null;
        }
        string text = string.Empty;
        float delay = 0f;
        if (voicePlayListList.Count > 0)
        {
            delay = voicePlayListList[0][0].delay;
            for (int i = 0; i < voicePlayListList[0].Length; i++)
            {
                text = text + voicePlayListList[0][i].text;
            }
        }
        if (this.Talkdata == null)
        {
            this.Talkdata = base.StartCoroutine(SingletonMonoBehaviour<ScriptManager>.Instance.settalk(0x3e7, text, delay));
        }
    }

    public void Open(Kind kind, EquipTargetInfo equipTargetInfo, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(equipTargetInfo);
        this.Init();
    }

    public void Open(Kind kind, ServantLeaderInfo servantLeaderInfo, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(servantLeaderInfo);
        this.Init();
    }

    public void Open(Kind kind, long userSvtId, ClickDelegate callback)
    {
        UserServantEntity userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        this.Open(kind, userSvtEntity, callback);
    }

    public void Open(Kind kind, UserServantCollectionEntity userSvtCollectionEntity, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(userSvtCollectionEntity);
        this.Init();
    }

    public void Open(Kind kind, UserServantEntity userSvtEntity, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(userSvtEntity, null, kind);
        this.Init();
    }

    public void Open(Kind kind, PartyListViewItem partyItem, int member, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(partyItem, member, kind);
        this.Init();
    }

    public void Open(Kind kind, long userSvtId, long[] equipIdList, ClickDelegate callback)
    {
        UserServantEntity userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        this.Open(kind, userSvtEntity, equipIdList, callback);
    }

    public void Open(Kind kind, long userSvtId, bool isUse, ClickDelegate callback)
    {
        UserServantEntity userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        this.Open(kind, userSvtEntity, isUse, callback);
    }

    public void Open(Kind kind, UserServantEntity userSvtEntity, long[] equipIdList, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(userSvtEntity, equipIdList, kind);
        this.Init();
    }

    public void Open(Kind kind, UserServantEntity userSvtEntity, bool isUse, ClickDelegate callback)
    {
        this.kind = kind;
        this.callbackFunc = callback;
        this.mainInfo = new ServantStatusListViewItem(userSvtEntity, isUse);
        this.Init();
    }

    protected void PlayBattleEffect(bool noupdate = false)
    {
        if (!this.isBattlePlay)
        {
            this.isBattlePlay = true;
            Transform parent = this.battleChrCamera.transform;
            float x = 1f;
            while (true)
            {
                UIRoot root = parent.GetComponent<UIRoot>();
                if (root != null)
                {
                    x = 1f / root.transform.localScale.x;
                    break;
                }
                parent = parent.parent;
            }
            this.battleChrCamera.transform.localScale = new Vector3(x, x, x);
            int id = this.mainInfo.Servant.id;
            int limitCountByImageLimit = ImageLimitCount.GetLimitCountByImageLimit(this.mainInfo.DispLimitCount);
            if (this.battleChr != null)
            {
                UnityEngine.Object.Destroy(this.battleChr);
            }
            GameObject go = ServantAssetLoadManager.loadBattleActor(this.battleChrCamera.gameObject, id, limitCountByImageLimit);
            this.battleChr = go;
            BattleFBXComponent component = go.AddComponent<BattleFBXComponent>();
            component.RootTransform = go.transform;
            float num4 = 0f;
            float num5 = 0f;
            ServantLimitAddMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
            long[] args = new long[] { (long) id, (long) limitCountByImageLimit };
            if (master.isEntityExistsFromId(args))
            {
                ServantLimitAddEntity entity = master.getEntityFromId<ServantLimitAddEntity>(id, limitCountByImageLimit);
                num4 = entity.battleCharaOffsetX * 0.01f;
                num5 = entity.battleCharaOffsetY * 0.01f;
            }
            go.transform.localPosition = new Vector3(0.85f + num4, -1.55f + num5, 4.55f);
            go.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            go.transform.localScale = Vector3.one;
            if ((this.mainInfo != null) && (this.mainInfo.Servant != null))
            {
                int battleSize = this.mainInfo.Servant.battleSize;
                Dictionary<int, float> dictionary = new Dictionary<int, float> {
                    { 
                        5,
                        0.75f
                    }
                };
                if (dictionary.ContainsKey(battleSize))
                {
                    go.transform.localScale = new Vector3(dictionary[battleSize], dictionary[battleSize], dictionary[battleSize]);
                }
            }
            component.SetWrapMode("wait", WrapMode.Loop);
            component.playAnimation("wait");
            component.SetEvolutionLevel(id, limitCountByImageLimit);
            this.battleActor = component;
            NGUITools.SetLayer(go, LayerMask.NameToLayer("Battle2D"));
            if (!noupdate)
            {
                this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.BATTLE);
                this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.COMMAND);
                if (this.isExit)
                {
                    this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.VALID);
                }
                else
                {
                    this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.INPUT, new ServantStatusListViewManager.CallbackFunc(this.OnSelectStatus));
                }
            }
        }
    }

    protected bool PlayBattleVoice(int svtId, int limitCount, List<ServantVoiceData[]> voicePlayListList, int listIndex = -1)
    {
        if (voicePlayListList == null)
        {
            return false;
        }
        if (voicePlayListList.Count <= 0)
        {
            return false;
        }
        string assetName = this.GetVoiceAssetName(VoiceAssetType.BATTLE, svtId, limitCount, 0);
        return this.PlayVoice(assetName, voicePlayListList[0], listIndex);
    }

    protected bool PlayChrVoice(int svtId, int limitCount, List<ServantVoiceData[]> voicePlayListList, int listIndex = -1)
    {
        if (voicePlayListList == null)
        {
            return false;
        }
        if (voicePlayListList.Count <= 0)
        {
            return false;
        }
        string assetName = this.GetVoiceAssetName(VoiceAssetType.HOME, svtId, limitCount, 0);
        return this.PlayVoice(assetName, voicePlayListList[0], listIndex);
    }

    protected bool PlayChrVoice(int svtId, int limitCount, ServantVoiceData[] voicePlayList, int listIndex = -1)
    {
        string assetName = this.GetVoiceAssetName(VoiceAssetType.HOME, svtId, limitCount, 0);
        return this.PlayVoice(assetName, voicePlayList, listIndex);
    }

    protected bool PlayNpVoice(int svtId, int limitCount, List<ServantVoiceData[]> voicePlayListList, int listIndex = -1)
    {
        if (voicePlayListList == null)
        {
            return false;
        }
        if (voicePlayListList.Count <= 0)
        {
            return false;
        }
        string assetName = this.GetVoiceAssetName(VoiceAssetType.NP, svtId, limitCount, 0);
        return this.PlayVoice(assetName, voicePlayListList[0], listIndex);
    }

    protected bool PlayVoice(string assetName, ServantVoiceData[] voicePlayList, int listIndex = -1)
    {
        if (voicePlayList != null)
        {
            if (voicePlayList.Length == 0)
            {
                return false;
            }
            foreach (string str in this.voiceDataList)
            {
                if (assetName.Equals(str))
                {
                    this.StopVoice();
                    if (this.tabKind == TabKind.VOICE)
                    {
                        this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.PLAY, listIndex);
                        this.voiceListIndex = listIndex;
                    }
                    this.voicePlayList = voicePlayList;
                    this.voicePlayAssetName = assetName;
                    this.voicePlayNum = 0;
                    this.EndWaitVoice();
                    return true;
                }
            }
        }
        return false;
    }

    protected void QuitList()
    {
        this.isExit = true;
        if (this.isBgmLow)
        {
            this.isBgmLow = false;
            OptionManager.Recover();
        }
        this.charaGraphListViewManager.SetMode(ServantStatusCharaGraphListViewManager.InitMode.VALID);
        this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.VALID);
        this.profileTabListViewManager.SetMode(ServantStatusFlavorTextListViewManager.InitMode.VALID);
    }

    protected void SetMark()
    {
        if ((((this.mainInfo.UserGame != null) && (this.mainInfo.UserServant != null)) && ((this.kind != Kind.COMBINE_MATERIAL_SVT) && (this.kind != Kind.COMBINE_MATERIAL_NOSVT))) && (this.kind != Kind.COMBINE_MATERIAL_EQUIP))
        {
            this.markBase.SetActive(true);
            this.lockSprite.spriteName = !this.mainInfo.UserServant.IsLock() ? "button_lock_unreg" : "button_lock_reg";
            this.favoriteButton.gameObject.SetActive(this.isUseFavorite);
            this.favoriteSprite.spriteName = (this.mainInfo.FavoriteUserSvtId != this.mainInfo.UserServant.id) ? "button_favorite_unreg" : "button_favorite_reg";
            this.equipSprite.gameObject.SetActive(this.mainInfo.IsUse);
        }
        else
        {
            this.markBase.SetActive(false);
        }
    }

    protected void SetTabKind(TabKind kind)
    {
        if ((this.tabKind == TabKind.VOICE) && (kind != TabKind.VOICE))
        {
            this.StopVoice();
        }
        int index = (int) kind;
        this.tabKind = kind;
        switch (kind)
        {
            case TabKind.STATUS:
                this.statusTabBase.SetActive(true);
                this.profileTabBase.SetActive(false);
                this.battleTabBase.SetActive(false);
                this.voiceTabBase.SetActive(false);
                break;

            case TabKind.PROFILE:
                this.statusTabBase.SetActive(false);
                this.profileTabBase.SetActive(true);
                this.battleTabBase.SetActive(false);
                this.voiceTabBase.SetActive(false);
                break;

            case TabKind.BATTLE:
                this.statusTabBase.SetActive(false);
                this.profileTabBase.SetActive(false);
                this.battleTabBase.SetActive(true);
                this.voiceTabBase.SetActive(false);
                this.SetupBattleButton(true);
                if (this.battleActor != null)
                {
                    this.battleActor.playAnimation("wait");
                }
                break;

            case TabKind.VOICE:
                this.statusTabBase.SetActive(false);
                this.profileTabBase.SetActive(false);
                this.battleTabBase.SetActive(false);
                this.voiceTabBase.SetActive(true);
                break;
        }
        switch (this.kind)
        {
            case Kind.FRIEND:
            case Kind.FOLLOWER:
            case Kind.DROP:
                this.statusButton.gameObject.SetActive(true);
                this.statusButton.isEnabled = true;
                this.statusButton.enabled = kind != TabKind.STATUS;
                this.statusTitleSprite.spriteName = (kind == TabKind.STATUS) ? "btn_txt_status_on" : "btn_txt_status_off";
                this.statusTitleSprite.MakePixelPerfect();
                this.statusSprite.spriteName = (kind == TabKind.STATUS) ? "btn_bg_19" : "btn_bg_12";
                this.statusButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = false;
                this.profileTitleSprite.spriteName = "btn_txt_profile_off";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = "btn_bg_12";
                this.profileButton.SetState(UICommonButtonColor.State.Disabled, true);
                this.battleButton.gameObject.SetActive(true);
                this.battleButton.isEnabled = true;
                this.battleButton.enabled = kind != TabKind.BATTLE;
                this.battleTitleSprite.spriteName = (kind == TabKind.BATTLE) ? "btn_txt_battlecharacter_on" : "btn_txt_battlecharacter_off";
                this.battleTitleSprite.MakePixelPerfect();
                this.battleSprite.spriteName = (kind == TabKind.BATTLE) ? "btn_bg_19" : "btn_bg_12";
                this.battleButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.voiceButton.gameObject.SetActive(true);
                this.voiceButton.isEnabled = true;
                this.voiceButton.enabled = false;
                this.voiceTitleSprite.spriteName = "btn_txt_voice_off";
                this.voiceTitleSprite.MakePixelPerfect();
                this.voiceSprite.spriteName = "btn_bg_12";
                this.voiceButton.SetState(UICommonButtonColor.State.Disabled, true);
                break;

            case Kind.COMBINE_MATERIAL:
            case Kind.COMBINE_MATERIAL_NOSVT:
                this.statusButton.gameObject.SetActive(false);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = false;
                this.profileTitleSprite.spriteName = "btn_txt_detail_on";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = "btn_bg_19";
                this.profileButton.SetState(UICommonButtonColor.State.Normal, true);
                this.battleButton.gameObject.SetActive(false);
                this.voiceButton.gameObject.SetActive(false);
                break;

            case Kind.SERVANT_EQUIP:
            case Kind.FIRST_SINGLE_GET_SERVANT_EQUIP:
            case Kind.SUMMON_SERVANT_EQUIP:
            case Kind.COMBINE_SERVANT_EQUIP:
            case Kind.COMBINE_MATERIAL_EQUIP:
                this.statusButton.gameObject.SetActive(true);
                this.statusButton.isEnabled = true;
                this.statusButton.enabled = kind != TabKind.STATUS;
                this.statusTitleSprite.spriteName = (kind == TabKind.STATUS) ? "btn_txt_status_on" : "btn_txt_status_off";
                this.statusTitleSprite.MakePixelPerfect();
                this.statusSprite.spriteName = (kind == TabKind.STATUS) ? "btn_bg_19" : "btn_bg_12";
                this.statusButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = kind != TabKind.PROFILE;
                this.profileTitleSprite.spriteName = (kind == TabKind.PROFILE) ? "btn_txt_detail_on" : "btn_txt_detail_off";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = (kind == TabKind.PROFILE) ? "btn_bg_19" : "btn_bg_12";
                this.profileButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.battleButton.gameObject.SetActive(false);
                this.voiceButton.gameObject.SetActive(false);
                break;

            case Kind.FRIEND_SERVANT_EQUIP:
            case Kind.FOLLOWER_SERVANT_EQUIP:
            case Kind.DROP_SERVANT_EQUIP:
                this.statusButton.gameObject.SetActive(true);
                this.statusButton.isEnabled = true;
                this.statusButton.enabled = kind != TabKind.STATUS;
                this.statusTitleSprite.spriteName = (kind == TabKind.STATUS) ? "btn_txt_detail_on" : "btn_txt_detail_off";
                this.statusTitleSprite.MakePixelPerfect();
                this.statusSprite.spriteName = (kind == TabKind.STATUS) ? "btn_bg_19" : "btn_bg_12";
                this.statusButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = false;
                this.profileTitleSprite.spriteName = "btn_txt_profile_off";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = "btn_bg_12";
                this.profileButton.SetState(UICommonButtonColor.State.Disabled, true);
                this.battleButton.gameObject.SetActive(false);
                this.voiceButton.gameObject.SetActive(false);
                break;

            case Kind.ENEMY_COLLECTION_DETAIL:
                this.statusButton.gameObject.SetActive(false);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = false;
                this.profileTitleSprite.spriteName = "btn_txt_profile_off";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = "btn_bg_19";
                this.profileButton.SetState(UICommonButtonColor.State.Normal, true);
                this.battleButton.gameObject.SetActive(false);
                this.voiceButton.gameObject.SetActive(false);
                break;

            default:
                string str;
                string str2;
                bool flag;
                this.mainInfo.GetVoiceInfo(out str, out str2, out flag);
                this.statusButton.gameObject.SetActive(true);
                this.statusButton.isEnabled = true;
                this.statusButton.enabled = kind != TabKind.STATUS;
                this.statusTitleSprite.spriteName = (kind == TabKind.STATUS) ? "btn_txt_status_on" : "btn_txt_status_off";
                this.statusTitleSprite.MakePixelPerfect();
                this.statusSprite.spriteName = (kind == TabKind.STATUS) ? "btn_bg_19" : "btn_bg_12";
                this.statusButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.profileButton.gameObject.SetActive(true);
                this.profileButton.isEnabled = true;
                this.profileButton.enabled = kind != TabKind.PROFILE;
                this.profileTitleSprite.spriteName = (kind == TabKind.PROFILE) ? "btn_txt_profile_on" : "btn_txt_profile_off";
                this.profileTitleSprite.MakePixelPerfect();
                this.profileSprite.spriteName = (kind == TabKind.PROFILE) ? "btn_bg_19" : "btn_bg_12";
                this.profileButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                this.battleButton.gameObject.SetActive(true);
                this.battleButton.isEnabled = true;
                this.battleButton.enabled = kind != TabKind.BATTLE;
                this.battleTitleSprite.spriteName = (kind == TabKind.BATTLE) ? "btn_txt_battlecharacter_on" : "btn_txt_battlecharacter_off";
                this.battleTitleSprite.MakePixelPerfect();
                this.battleSprite.spriteName = (kind == TabKind.BATTLE) ? "btn_bg_19" : "btn_bg_12";
                this.battleButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                if (flag)
                {
                    this.voiceButton.gameObject.SetActive(true);
                    this.voiceButton.isEnabled = true;
                    this.voiceButton.enabled = kind != TabKind.VOICE;
                    this.voiceTitleSprite.spriteName = (kind == TabKind.VOICE) ? "btn_txt_voice_on" : "btn_txt_voice_off";
                    this.voiceTitleSprite.MakePixelPerfect();
                    this.voiceSprite.spriteName = (kind == TabKind.VOICE) ? "btn_bg_19" : "btn_bg_12";
                    this.battleButton.SetState(UICommonButtonColor.State.Normal, this.isInitTab);
                }
                else
                {
                    this.voiceButton.gameObject.SetActive(true);
                    this.voiceButton.isEnabled = true;
                    this.voiceButton.enabled = false;
                    this.voiceTitleSprite.spriteName = "btn_txt_voice_off";
                    this.voiceTitleSprite.MakePixelPerfect();
                    this.voiceSprite.spriteName = "btn_bg_12";
                    this.voiceButton.SetState(UICommonButtonColor.State.Disabled, true);
                }
                break;
        }
        if (!this.tabInitList[index])
        {
            this.tabInitList[index] = true;
            switch (kind)
            {
                case TabKind.STATUS:
                    this.statusTabListViewManager.CreateList(this.mainInfo);
                    break;

                case TabKind.PROFILE:
                    this.profileTabListViewManager.CreateList(this.mainInfo);
                    if (this.profileNewList != null)
                    {
                        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).SetOpen(this.mainInfo.Servant.id, this.profileNewList);
                    }
                    break;

                case TabKind.VOICE:
                    this.voiceTabListViewManager.CreateList(this.mainInfo);
                    break;
            }
        }
        switch (kind)
        {
            case TabKind.STATUS:
                this.statusTabListViewManager.SetMode(ServantStatusListViewManager.InitMode.INPUT, new ServantStatusListViewManager.CallbackFunc(this.OnSelectStatus));
                break;

            case TabKind.PROFILE:
                this.profileTabListViewManager.SetMode(ServantStatusFlavorTextListViewManager.InitMode.INPUT, new ServantStatusFlavorTextListViewManager.CallbackFunc(this.OnSelectFlavor));
                break;

            case TabKind.VOICE:
                if (!this.isBgmLow && (OptionManager.GetBgmVolume() > BgmManager.LOW_VOLUME))
                {
                    this.isBgmLow = true;
                    OptionManager.TestBgmVolume(BgmManager.LOW_VOLUME);
                }
                this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.INPUT, new ServantStatusVoiceListViewManager.CallbackFunc(this.OnSelectVoice));
                break;
        }
        if (this.isBgmLow && (kind != TabKind.VOICE))
        {
            this.isBgmLow = false;
            OptionManager.Recover();
        }
        this.isInitTab = true;
    }

    protected void SetupBattleButton(bool isInit = false)
    {
        for (int i = 0; i < this.battleCharaLevelButtonList.Length; i++)
        {
            bool flag = i == this.mainInfo.DispLimitCount;
            bool flag2 = (i <= this.mainInfo.MaxDispLimitCount) && this.isBattlePlay;
            if (flag2)
            {
                this.battleCharaLevelTitleSpriteList[i].spriteName = !flag ? ("btn_txt_" + (i + 1) + "_off") : ("btn_txt_" + (i + 1) + "_on");
            }
            else
            {
                this.battleCharaLevelTitleSpriteList[i].spriteName = "btn_txt_4";
            }
            this.battleCharaLevelTitleSpriteList[i].MakePixelPerfect();
            this.battleCharaLevelSpriteList[i].spriteName = !flag ? "btn_bg_20" : "btn_bg_21";
            this.battleCharaCloseSpriteList[i].gameObject.SetActive(false);
            this.battleCharaLevelButtonList[i].SetState(!flag2 ? UICommonButtonColor.State.Disabled : UICommonButtonColor.State.Normal, isInit);
            this.battleCharaLevelButtonList[i].enabled = !flag;
        }
    }

    public void SetVisibleHighPriorityObject(bool isVisible)
    {
        if (this.tabKind == TabKind.BATTLE)
        {
            this.battleChrCamera.gameObject.SetActive(isVisible);
            if (isVisible && (this.battleActor != null))
            {
                this.battleActor.playAnimation("wait");
            }
        }
    }

    protected bool StartVoiceLoad()
    {
        if (this.voiceDataList.Count > 0)
        {
            foreach (string str in this.voiceDataList)
            {
                SoundManager.releaseAudioAssetStorage(str);
            }
            this.voiceDataList.Clear();
        }
        if (this.requestVoiceDataList.Count > 0)
        {
            SoundManager.loadAudioAssetStorage(this.requestVoiceDataList[0], new System.Action(this.EndLoadVoice), SoundManager.CueType.ALL);
            return true;
        }
        return false;
    }

    protected void StopVoice()
    {
        if (this.Talkdata != null)
        {
            SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
        }
        if ((this.tabKind == TabKind.VOICE) && (this.voiceListIndex >= 0))
        {
            this.voiceTabListViewManager.SetMode(ServantStatusVoiceListViewManager.InitMode.PLAY, -1);
            this.voiceListIndex = -1;
        }
        if (this.voicePlayList != null)
        {
            base.CancelInvoke("EndWaitVoice");
            this.voicePlayList = null;
            this.voicePlayNum = 0;
            this.voicePlayAssetName = null;
        }
        if (this.voicePlayer != null)
        {
            this.voicePlayer.RemoveCallback(new System.Action(this.EndPlayVoice));
            this.voicePlayer.StopSe(0f);
            this.voicePlayer = null;
        }
    }

    public delegate void ClickDelegate(bool isDecide);

    public enum Kind
    {
        NORMAL,
        PARTY,
        COLLECTION,
        FRIEND,
        FOLLOWER,
        FIRST_SINGLE_GET,
        DROP,
        SUMMON,
        COMBINE,
        COMBINE_MATERIAL,
        SERVANT_EQUIP,
        FRIEND_SERVANT_EQUIP,
        FOLLOWER_SERVANT_EQUIP,
        FIRST_SINGLE_GET_SERVANT_EQUIP,
        DROP_SERVANT_EQUIP,
        SUMMON_SERVANT_EQUIP,
        COMBINE_SERVANT_EQUIP,
        ENEMY_COLLECTION_DETAIL,
        COMBINE_MATERIAL_SVT,
        COMBINE_MATERIAL_NOSVT,
        COMBINE_MATERIAL_EQUIP
    }

    protected enum TabKind
    {
        STATUS,
        PROFILE,
        BATTLE,
        VOICE,
        SUM
    }

    protected enum VoiceAssetType
    {
        BATTLE,
        HOME,
        NP
    }
}

