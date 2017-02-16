using System;
using System.Collections.Generic;
using UnityEngine;

public class LimitUpResultCheckComponent : MonoBehaviour
{
    private UserServantEntity baseUsrSvtData;
    [SerializeField]
    protected GameObject heroQuestInfo;
    [SerializeField]
    protected UILabel heroQuestInfoDetail;
    [SerializeField]
    protected UILabel heroQuestInfoTitle;
    private bool isGetNewSkill;
    private bool isOpenQuest;
    [SerializeField]
    protected OpenInfoWindowComponent openInfowindowComp;
    private List<GameObject> resInfoList;
    private UserServantEntity resUsrSvtData;
    [SerializeField]
    protected GameObject skillGetInfo;
    [SerializeField]
    protected UILabel skillGetInfoDetail;
    [SerializeField]
    protected UILabel skillGetInfoName;
    [SerializeField]
    protected UILabel skillGetInfoTitle;
    [SerializeField]
    protected GameObject storyQuestInfo;
    [SerializeField]
    protected UILabel storyQuestInfoDetail;
    [SerializeField]
    protected UILabel storyQuestInfoTitle;
    private int svtCollectionLimitCnt;

    private void checkGetSkill()
    {
        bool flag = true;
        this.isGetNewSkill = false;
        int[] numArray = this.baseUsrSvtData.getSkillIdList();
        int[] numArray2 = this.resUsrSvtData.getSkillIdList();
        int index = 0;
        for (int i = 0; i < numArray.Length; i++)
        {
            if (!numArray[i].Equals(numArray2[i]))
            {
                flag = false;
                index = i;
                break;
            }
        }
        if (!flag)
        {
            string str;
            string str2;
            int id = numArray2[index];
            SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(id);
            this.skillGetInfoTitle.text = LocalizationManager.Get("GET_SKILL_TITLE");
            int lv = this.resUsrSvtData.getSkillLevel(index);
            entity.getSkillMessageInfo(out str, out str2, lv);
            this.skillGetInfoName.text = string.Format(LocalizationManager.Get("GET_SKILL_NAME"), str);
            this.skillGetInfoDetail.text = str2;
            this.resInfoList.Add(this.skillGetInfo);
            this.isGetNewSkill = true;
        }
    }

    private void checkQuestOpen()
    {
        this.isOpenQuest = false;
        SingletonTemplate<clsQuestCheck>.Instance.mfInit();
        List<int> releaseQuestIdByServantLimit = new List<int>();
        if (this.svtCollectionLimitCnt >= 0)
        {
            releaseQuestIdByServantLimit = SingletonTemplate<clsQuestCheck>.Instance.GetReleaseQuestIdByServantLimit(this.resUsrSvtData.svtId, this.svtCollectionLimitCnt);
            QuestEntity entity = null;
            this.storyQuestInfoTitle.text = LocalizationManager.Get("OPEN_STORY_QUEST_TITLE");
            this.heroQuestInfoTitle.text = LocalizationManager.Get("OPEN_HERO_QUEST_TITLE");
            if ((releaseQuestIdByServantLimit != null) && (releaseQuestIdByServantLimit.Count > 0))
            {
                for (int i = 0; i < releaseQuestIdByServantLimit.Count; i++)
                {
                    Debug.Log("***!!*** checkQuestOpen openQuestIds ID: " + releaseQuestIdByServantLimit[i]);
                    if (releaseQuestIdByServantLimit[i] > 0)
                    {
                        entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(releaseQuestIdByServantLimit[i]);
                        if (entity.type == 3)
                        {
                            this.storyQuestInfoDetail.text = string.Format(LocalizationManager.Get("OPEN_QUEST_NAME"), entity.name);
                            this.resInfoList.Add(this.storyQuestInfo);
                        }
                        else if (entity.type == 6)
                        {
                            this.heroQuestInfoDetail.text = string.Format(LocalizationManager.Get("OPEN_QUEST_NAME"), entity.name);
                            this.resInfoList.Add(this.heroQuestInfo);
                        }
                    }
                }
                this.isOpenQuest = true;
            }
        }
    }

    public void checkResultLimitUp(UserServantEntity baseData, UserServantEntity resData, int baseCollectionLimitCnt)
    {
        this.baseUsrSvtData = baseData;
        this.resUsrSvtData = resData;
        this.svtCollectionLimitCnt = baseCollectionLimitCnt;
        this.resInfoList = new List<GameObject>();
        this.isGetNewSkill = false;
        this.isOpenQuest = false;
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.resUsrSvtData.svtId).IsServant)
        {
            this.checkQuestOpen();
            this.checkGetSkill();
        }
        if (this.isGetNewSkill || this.isOpenQuest)
        {
            this.openInfowindowComp.OpenResultInfo(this.resInfoList, null);
        }
    }

    protected enum QUESTTYPE
    {
        FRIENDSHIP = 3,
        HEROBALLAD = 6
    }
}

