using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FriendIconComponent : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton baseButton;
    protected int classPos;
    [SerializeField]
    protected UILabel loginDataLabel;
    [SerializeField]
    protected UIIconLabel playerLevelIconLabel;
    [SerializeField]
    protected UILabel playerNameLabel;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UILabel servantNameLabel;
    [SerializeField]
    protected GameObject[] skillBaseList = new GameObject[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UICommonButton[] skillButtonList;
    [SerializeField]
    protected SkillIconComponent[] skillIconList = new SkillIconComponent[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UIIconLabel[] skillLevelIconLabelList = new UIIconLabel[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected UILabel svtNpTitleLabel;
    [SerializeField]
    protected UISprite tdLevelIconSprite;
    protected OtherUserGameEntity userGameEntity;

    protected void EndShowServant(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
    }

    public void OnClickServantStatus()
    {
        if (this.userGameEntity != null)
        {
            ServantLeaderInfo servantLeaderInfo = this.userGameEntity.getServantLeaderInfo(this.classPos);
            if (servantLeaderInfo != null)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FOLLOWER, servantLeaderInfo, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
            }
        }
    }

    public void OnClickSkill1()
    {
    }

    public void OnClickSkill13()
    {
    }

    public void OnClickSkill2()
    {
    }

    public void OnClickSupportInfo()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        SupportInfoJump data = new SupportInfoJump(this.userGameEntity, FriendStatus.Kind.SEARCH, false);
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.SupportSelect, SceneManager.FadeType.BLACK, data);
    }

    public void OnLongPushSkill1()
    {
        this.OpenSkillInfoDialog(0);
    }

    public void OnLongPushSkill2()
    {
        this.OpenSkillInfoDialog(1);
    }

    public void OnLongPushSkill3()
    {
        this.OpenSkillInfoDialog(2);
    }

    protected void OpenSkillInfoDialog(int skillIndex)
    {
        if (this.userGameEntity != null)
        {
            ServantLeaderInfo info = this.userGameEntity.getServantLeaderInfo(this.classPos);
            if ((info != null) && (info.userSvtId != 0))
            {
                int[] numArray;
                int[] numArray2;
                int[] numArray3;
                string[] strArray;
                string[] strArray2;
                string str;
                string str2;
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                info.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(numArray[skillIndex]);
                SkillLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(numArray[skillIndex], numArray2[skillIndex]);
                entity.getSkillMessageInfo(out str, out str2, numArray2[skillIndex]);
                str = str + " " + string.Format(LocalizationManager.Get("MASTER_EQSKILL_LV_TXT"), numArray2[skillIndex]);
                string str3 = string.Format(LocalizationManager.Get("BATTLE_SKILLCHARGETURN"), entity2.chargeTurn);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailLongInfoDialog(str, str3, str2);
                return;
            }
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
    }

    public void Set(OtherUserGameEntity userGameEntity, bool isUseServantStatus = false, int classPos = 0)
    {
        this.userGameEntity = !isUseServantStatus ? null : userGameEntity;
        this.classPos = classPos;
        if (userGameEntity != null)
        {
            ServantEntity entity;
            if (userGameEntity.getSvtId(this.classPos) != 0)
            {
                entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(userGameEntity.getSvtId(this.classPos));
            }
            else
            {
                entity = null;
            }
            ServantLeaderInfo servantLeaderInfo = userGameEntity.getServantLeaderInfo(this.classPos);
            if ((servantLeaderInfo != null) && (servantLeaderInfo.userSvtId == 0))
            {
                servantLeaderInfo = null;
            }
            this.servantFaceIcon.Set(servantLeaderInfo, null, false);
            this.playerNameLabel.text = userGameEntity.userName;
            this.playerLevelIconLabel.Set(IconLabelInfo.IconKind.LEVEL, userGameEntity.userLv, 0, 0, 0L, false, false);
            if (entity != null)
            {
                int num;
                int num2;
                int num3;
                int num4;
                int num5;
                string str;
                string str2;
                int num6;
                int num7;
                this.servantNameLabel.text = entity.name;
                userGameEntity.getTreasureDeviceInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7, this.classPos);
                int num8 = userGameEntity.getTreasureDeviceLevelIcon(this.classPos);
                this.svtNpTitleLabel.text = string.Format(LocalizationManager.Get((num8 <= 1) ? "NP_COLOR_NAME" : "NP_MAX_COLOR_NAME"), str);
                if (num8 > 1)
                {
                    this.tdLevelIconSprite.spriteName = "icon_nplv_2";
                }
                else if (num8 == 1)
                {
                    this.tdLevelIconSprite.spriteName = "icon_nplv_1";
                }
                else
                {
                    this.tdLevelIconSprite.spriteName = null;
                }
            }
            else
            {
                this.servantNameLabel.text = LocalizationManager.Get("COMMON_NO_ENTRY");
                this.svtNpTitleLabel.text = LocalizationManager.Get("NO_ENTRY_NAME");
                this.tdLevelIconSprite.spriteName = null;
            }
            this.loginDataLabel.text = LocalizationManager.GetBeforeTime(userGameEntity.getUpdatedAt(this.classPos));
            if (servantLeaderInfo != null)
            {
                int[] numArray;
                int[] numArray2;
                int[] numArray3;
                string[] strArray;
                string[] strArray2;
                servantLeaderInfo.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                int num9 = 1;
                for (int i = 0; i < this.skillIconList.Length; i++)
                {
                    if ((i < numArray.Length) && (numArray[i] > 0))
                    {
                        num9 = i + 1;
                    }
                }
                for (int j = 0; j < this.skillIconList.Length; j++)
                {
                    if (this.skillBaseList[j] != null)
                    {
                        if (j < num9)
                        {
                            this.skillBaseList[j].SetActive(true);
                            this.skillIconList[j].Set(numArray[j], numArray2[j]);
                            this.skillLevelIconLabelList[j].Set(IconLabelInfo.IconKind.FOLLOWER_SKILL_LEVEL, numArray2[j], 0, 0, 0L, false, false);
                        }
                        else
                        {
                            this.skillBaseList[j].SetActive(false);
                        }
                    }
                }
            }
            else
            {
                for (int k = 0; k < this.skillIconList.Length; k++)
                {
                    if (this.skillBaseList[k] != null)
                    {
                        this.skillBaseList[k].SetActive(false);
                    }
                }
            }
        }
        if (this.baseButton != null)
        {
            bool isEnable = this.userGameEntity != null;
            this.baseButton.SetColliderEnable(isEnable, true);
        }
    }
}

