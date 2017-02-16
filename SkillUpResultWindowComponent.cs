using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class SkillUpResultWindowComponent : BaseDialog
{
    protected static readonly Color COLOR_VAL = new Color(0.99f, 0.945f, 0.316f);
    [SerializeField]
    protected UILabel currentLvLb;
    [SerializeField]
    protected UILabel currentNpLvLb;
    [SerializeField]
    protected UILabel detailLb;
    [SerializeField]
    protected UILabel friendshipCurrentLvLabel;
    [SerializeField]
    protected GameObject friendshipInfo;
    [SerializeField]
    protected UILabel friendshipLabel;
    [SerializeField]
    protected GameObject friendshipLvInfo;
    [SerializeField]
    protected UILabel friendshipResultLvLabel;
    [SerializeField]
    protected UISprite iconImg;
    [SerializeField]
    protected GameObject lvInfo;
    [SerializeField]
    protected UILabel npAftDetailLb;
    [SerializeField]
    protected UILabel npAftTitleLb;
    [SerializeField]
    protected UILabel npBefDetailLb;
    [SerializeField]
    protected UILabel npBefTitleLb;
    [SerializeField]
    protected UILabel npDetailLb;
    [SerializeField]
    protected GameObject npInfo;
    [SerializeField]
    protected GameObject npLvFirstInfo;
    [SerializeField]
    protected UISprite npLvImg;
    [SerializeField]
    protected GameObject npLvInfo;
    [SerializeField]
    protected UILabel npNameLb;
    [SerializeField]
    protected GameObject npRankInfo;
    [SerializeField]
    protected UILabel npRankupAfterLb;
    [SerializeField]
    protected UILabel npRankupBeforeLb;
    [SerializeField]
    protected UILabel npRubyNameLb;
    protected System.Action openCallBack;
    [SerializeField]
    protected GameObject powerupInfo;
    [SerializeField]
    protected UILabel powerupLabel;
    [SerializeField]
    protected UILabel resLvLb;
    [SerializeField]
    protected UILabel resNpLvFirstLb;
    [SerializeField]
    protected UILabel resNpLvLb;
    [SerializeField]
    protected UILabel skillAftdetailLb;
    [SerializeField]
    protected UILabel skillAftTitleLb;
    [SerializeField]
    protected UILabel skillBefdetailLb;
    [SerializeField]
    protected UILabel skillBefTitleLb;
    [SerializeField]
    protected SkillIconComponent skillIconComp;
    [SerializeField]
    protected GameObject skillInfo;
    protected State state;
    [SerializeField]
    protected UILabel targetNameLb;

    public void Close()
    {
        this.Close(new System.Action(this.EndClose));
    }

    public void Close(System.Action callback)
    {
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
        this.skillInfo.SetActive(false);
        this.npInfo.SetActive(false);
        this.friendshipInfo.SetActive(false);
        this.powerupInfo.SetActive(false);
        this.friendshipInfo.GetParent().gameObject.SetActive(false);
    }

    protected void EndOpen()
    {
        if (this.openCallBack != null)
        {
            System.Action openCallBack = this.openCallBack;
            this.openCallBack = null;
            openCallBack();
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        this.targetNameLb.text = string.Empty;
        this.currentLvLb.text = string.Empty;
        this.resLvLb.text = string.Empty;
        this.detailLb.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void OpenFriendshipUpResultInfo(UserServantEntity usrSvtData, int oldFriendShipRank, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
            base.gameObject.SetActive(true);
            this.friendshipInfo.SetActive(true);
            this.skillInfo.SetActive(false);
            this.npInfo.SetActive(false);
            this.powerupInfo.SetActive(false);
            this.friendshipInfo.GetParent().gameObject.SetActive(true);
            this.openCallBack = callback;
            this.friendshipLabel.text = string.Empty;
            StringBuilder builder = new StringBuilder();
            int svtId = usrSvtData.getSvtId();
            UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, svtId);
            List<QuestEntity> list = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestEntityByServantFriendShip(svtId, oldFriendShipRank, QuestEntity.TypeFlag.FRIENDSHIP);
            if (list != null)
            {
                foreach (QuestEntity entity2 in list)
                {
                    builder.AppendLine(string.Format(LocalizationManager.Get("RESULT_BOUNDS_OPENQUEST"), entity2.getQuestName()));
                }
            }
            if (ServantCommentManager.IsOpenByServantFriendShip(svtId, oldFriendShipRank))
            {
                builder.AppendLine(LocalizationManager.Get("RESULT_BOUNDS_UPDATE_MATERIAL"));
            }
            if (ServantVoiceMaster.isOpenByServantFriendShip(svtId, entity.maxLimitCount, oldFriendShipRank))
            {
                builder.AppendLine(LocalizationManager.Get("RESULT_BOUNDS_GETVOICE"));
            }
            this.friendshipLabel.text = builder.ToString();
            this.friendshipLvInfo.gameObject.SetActive(entity.getFriendShipRank() > oldFriendShipRank);
            this.friendshipCurrentLvLabel.text = oldFriendShipRank.ToString();
            this.friendshipResultLvLabel.text = entity.getFriendShipRank().ToString();
            this.friendshipResultLvLabel.GetComponent<UIWidget>().color = COLOR_VAL;
            if (string.IsNullOrEmpty(this.friendshipLabel.text))
            {
                this.friendshipInfo.GetParent().gameObject.SetActive(false);
            }
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenNpUpResultInfo(UserServantEntity usrSvtData, int targetId, int targetLv, System.Action callback, int targetIdOld, int targetLvOld, CombineResultEffectComponent.Kind kind)
    {
        if (this.state == State.INIT)
        {
            bool flag = kind == CombineResultEffectComponent.Kind.TREASUREDVCOPEN;
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
            base.gameObject.SetActive(true);
            this.npInfo.SetActive(true);
            this.skillInfo.SetActive(false);
            this.friendshipInfo.SetActive(false);
            this.powerupInfo.SetActive(false);
            this.npInfo.GetParent().gameObject.SetActive(true);
            this.openCallBack = callback;
            TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(targetId);
            TreasureDvcLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcLvMaster>(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(targetId, targetLv);
            TreasureDvcEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(targetIdOld);
            if (entity3 == null)
            {
                targetIdOld = 0;
            }
            if (entity != null)
            {
                this.npRubyNameLb.text = entity.ruby;
                this.npNameLb.text = entity.getName();
                int num = targetLvOld;
                if (flag || (targetIdOld > 0))
                {
                    this.npRankInfo.SetActive(!flag);
                    this.npLvInfo.SetActive(false);
                    this.npLvFirstInfo.SetActive(false);
                    this.npRankupBeforeLb.text = entity3.rank;
                    this.npRankupAfterLb.text = entity.rank;
                    this.npRankupAfterLb.GetComponent<UIWidget>().color = COLOR_VAL;
                    num = targetLvOld;
                }
                else if (num <= 0)
                {
                    this.npRankInfo.SetActive(false);
                    this.npLvInfo.SetActive(false);
                    this.npLvFirstInfo.SetActive(true);
                    this.resNpLvFirstLb.text = targetLv.ToString();
                }
                else
                {
                    this.currentNpLvLb.text = num.ToString();
                    this.resNpLvLb.text = targetLv.ToString();
                    this.resNpLvLb.GetComponent<UIWidget>().color = COLOR_VAL;
                    this.npRankInfo.SetActive(false);
                    this.npLvInfo.SetActive(true);
                    this.npLvFirstInfo.SetActive(false);
                }
                if (flag)
                {
                    this.npBefTitleLb.text = string.Empty;
                    this.npAftTitleLb.text = string.Empty;
                }
                else if (targetIdOld > 0)
                {
                    this.npBefTitleLb.text = LocalizationManager.Get("BEFORE_RESULT_TITLE_RANK");
                    this.npAftTitleLb.text = LocalizationManager.Get("AFTER_RESULT_TITLE_RANK");
                }
                else
                {
                    this.npBefTitleLb.text = LocalizationManager.Get("BEFORE_RESULT_TITLE");
                    this.npAftTitleLb.text = LocalizationManager.Get("AFTER_RESULT_TITLE");
                }
                if (entity2 != null)
                {
                    if (flag)
                    {
                        this.npBefDetailLb.text = entity2.getDetalShort(targetLv);
                        this.npAftDetailLb.text = string.Empty;
                    }
                    else if (targetIdOld > 0)
                    {
                        string str = string.Empty;
                        TreasureDvcLvEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcLvMaster>(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(targetIdOld, targetLvOld);
                        if (entity4 != null)
                        {
                            str = entity4.getDetalShort(num);
                        }
                        this.npBefDetailLb.text = str;
                        this.npAftDetailLb.text = entity2.getDetalShort(targetLv);
                    }
                    else
                    {
                        this.npBefDetailLb.text = entity2.getDetalShort(num);
                        this.npAftDetailLb.text = entity2.getDetalShort(targetLv);
                    }
                }
                else
                {
                    this.npBefDetailLb.text = string.Empty;
                    this.npAftDetailLb.text = string.Empty;
                }
            }
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenPowerUpResultInfo(UserServantEntity usrSvtData, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
            base.gameObject.SetActive(true);
            this.powerupInfo.SetActive(true);
            this.friendshipInfo.SetActive(false);
            this.skillInfo.SetActive(false);
            this.npInfo.SetActive(false);
            this.friendshipInfo.GetParent().gameObject.SetActive(true);
            this.openCallBack = callback;
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(usrSvtData.svtId);
            this.powerupLabel.text = string.Format(LocalizationManager.Get("RESULT_BOUNDS_POWERUP"), entity.battleName);
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenSkillUpResultInfo(int targetId, int targetLv, System.Action callback, int targetIdOld, int targetLvOld, bool isOpen = false)
    {
        if (this.state == State.INIT)
        {
            string str;
            string str2;
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
            base.gameObject.SetActive(true);
            this.skillInfo.SetActive(true);
            this.npInfo.SetActive(false);
            this.friendshipInfo.SetActive(false);
            this.powerupInfo.SetActive(false);
            this.skillInfo.GetParent().gameObject.SetActive(true);
            this.openCallBack = callback;
            SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(targetId);
            entity.getSkillMessageInfo(out str, out str2, targetLv);
            int lv = targetLv - 1;
            if (targetIdOld > 0)
            {
                lv = targetLvOld;
            }
            this.skillIconComp.Set(targetId, targetLv);
            this.targetNameLb.text = str;
            if ((lv <= 0) || (targetIdOld > 0))
            {
                this.lvInfo.SetActive(false);
            }
            else
            {
                this.currentLvLb.text = lv.ToString();
                this.resLvLb.text = targetLv.ToString();
                this.resLvLb.GetComponent<UIWidget>().color = COLOR_VAL;
                this.lvInfo.SetActive(true);
            }
            if (isOpen)
            {
                this.skillBefTitleLb.text = string.Empty;
                this.skillAftTitleLb.text = string.Empty;
                this.skillAftdetailLb.text = string.Empty;
            }
            else
            {
                if (targetIdOld > 0)
                {
                    this.skillBefTitleLb.text = LocalizationManager.Get("BEFORE_RESULT_TITLE_RANK");
                    this.skillAftTitleLb.text = LocalizationManager.Get("AFTER_RESULT_TITLE_RANK");
                }
                else
                {
                    this.skillBefTitleLb.text = LocalizationManager.Get("BEFORE_RESULT_TITLE");
                    this.skillAftTitleLb.text = LocalizationManager.Get("AFTER_RESULT_TITLE");
                }
                WrapControlText.textAdjust(this.skillAftdetailLb, str2);
                if (targetIdOld > 0)
                {
                    entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(targetIdOld);
                    if (entity != null)
                    {
                        entity.getSkillMessageInfo(out str, out str2, lv);
                    }
                }
                else
                {
                    entity.getSkillMessageInfo(out str, out str2, lv);
                }
            }
            WrapControlText.textAdjust(this.skillBefdetailLb, str2);
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    protected enum State
    {
        INIT
    }
}

