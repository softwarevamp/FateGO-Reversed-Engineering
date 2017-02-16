using System;
using System.Collections.Generic;
using UnityEngine;

public class CombineItemPlaceListViewManager : BaseScrollViewContrller
{
    public const float EXIT_TIME = 0.25f;
    protected InitMode initMode = InitMode.INPUT;
    public const float INTO_TIME = 0.5f;
    protected ItemEntity itemInfo;
    public UILabel m_EmptyLabel;
    private float mBaseClipRange;
    private BoxCollider mBoxCollider;
    private bool mIsDoing_Slide;
    private GameObject mNoneLabelParent;
    private List<clsMapCtrl_QuestInfo> mQestInfs_Caldea = new List<clsMapCtrl_QuestInfo>();
    public const float OUT_POS_OFS_X = 532f;

    public void CreateList()
    {
        base.DestroyItems();
        this.ShowQuestScrollList(this.itemInfo);
    }

    public InitMode GetInitMode() => 
        this.initMode;

    public void Init(ItemEntity itemData)
    {
        this.itemInfo = itemData;
    }

    public bool IsArrayContain(long[] array, long value)
    {
        bool flag = false;
        foreach (long num in array)
        {
            if (num == value)
            {
                flag = true;
            }
        }
        return flag;
    }

    public void ShowQuestScrollList(ItemEntity itemData)
    {
        GiftEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT).getEntitys<GiftEntity>();
        QuestEntity[] entityArray2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntitys<QuestEntity>();
        NpcServantEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcServantMaster>(DataNameKind.Kind.NPC_SERVANT).getEntitys<NpcServantEntity>();
        DropEntity[] entityArray4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<DropMaster>(DataNameKind.Kind.DROP).getEntitys<DropEntity>();
        NpcDeckEntity[] entityArray5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcDeckMaster>(DataNameKind.Kind.NPC_DECK).getEntitys<NpcDeckEntity>();
        StageEntity[] entityArray6 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<StageMaster>(DataNameKind.Kind.STAGE).getEntitys<StageEntity>();
        List<GiftEntity> list = new List<GiftEntity>();
        List<QuestEntity> list2 = new List<QuestEntity>();
        List<NpcDeckEntity> list3 = new List<NpcDeckEntity>();
        List<NpcServantEntity> list4 = new List<NpcServantEntity>();
        List<DropEntity> list5 = new List<DropEntity>();
        List<StageEntity> list6 = new List<StageEntity>();
        foreach (GiftEntity entity in entityArray)
        {
            if (entity.objectId == itemData.id)
            {
                list.Add(entity);
            }
        }
        Debug.LogError("gift :: " + list.Count);
        if (list.Count > 0)
        {
            foreach (DropEntity entity2 in entityArray4)
            {
                foreach (GiftEntity entity3 in list)
                {
                    if (entity3.id == entity2.giftId)
                    {
                        list5.Add(entity2);
                    }
                }
            }
            if (list5.Count > 0)
            {
                foreach (NpcServantEntity entity4 in entityArray3)
                {
                    foreach (DropEntity entity5 in list5)
                    {
                        foreach (long num4 in entity4.dropIds)
                        {
                            if (num4 == entity5.id)
                            {
                                list4.Add(entity4);
                            }
                        }
                    }
                }
            }
            Debug.LogError("svt :: " + list4.Count);
            if (list4.Count > 0)
            {
                foreach (NpcDeckEntity entity6 in entityArray5)
                {
                    foreach (NpcServantEntity entity7 in list4)
                    {
                        if ((entity6.npcSvtId == entity7.id) && !list3.Contains(entity6))
                        {
                            list3.Add(entity6);
                        }
                    }
                }
            }
            Debug.LogError("deckList :: " + list3.Count);
            if (list3.Count > 0)
            {
                foreach (StageEntity entity8 in entityArray6)
                {
                    foreach (NpcDeckEntity entity9 in list3)
                    {
                        foreach (long num8 in entity8.npcDeckIds)
                        {
                            if ((entity9.id == num8) && !list6.Contains(entity8))
                            {
                                list6.Add(entity8);
                            }
                        }
                    }
                }
            }
            Debug.LogError("stageList :: " + list6.Count);
            if (list6.Count > 0)
            {
                foreach (QuestEntity entity10 in entityArray2)
                {
                    foreach (StageEntity entity11 in list6)
                    {
                        if ((entity10.id == entity11.questId) && !list2.Contains(entity10))
                        {
                            list2.Add(entity10);
                        }
                    }
                }
            }
            foreach (QuestEntity entity12 in entityArray2)
            {
                foreach (GiftEntity entity13 in list)
                {
                    if (entity12.giftId == entity13.id)
                    {
                        list2.Add(entity12);
                    }
                }
            }
        }
        List<clsMapCtrl_QuestInfo> list7 = SingletonTemplate<QuestTree>.Instance.mfGetQuestInfoListP();
        List<clsMapCtrl_QuestInfo> list8 = this.mQestInfs_Caldea;
        list8.Clear();
        foreach (QuestEntity entity14 in list2)
        {
            foreach (clsMapCtrl_QuestInfo info in list7)
            {
                if ((info.mfGetQuestID() == entity14.id) && (entity14.type == 2))
                {
                    list8.Add(info);
                }
            }
        }
        if (list2.Count > 0)
        {
            List<clsMapCtrl_QuestInfo> list9 = new List<clsMapCtrl_QuestInfo>();
            List<clsMapCtrl_QuestInfo> list10 = new List<clsMapCtrl_QuestInfo>();
            int count = this.mQestInfs_Caldea.Count;
            Debug.LogError("mQestInfs_Caldea Count :: " + this.mQestInfs_Caldea.Count);
            for (int i = 0; i < count; i++)
            {
                clsMapCtrl_QuestInfo item = null;
                if (i < this.mQestInfs_Caldea.Count)
                {
                    item = this.mQestInfs_Caldea[i];
                }
                list9.Add(item);
            }
            foreach (clsMapCtrl_QuestInfo info3 in list9)
            {
                if (info3.mfGetTouchType() != clsMapCtrl_QuestInfo.enTouchType.Disable)
                {
                    list10.Add(info3);
                }
            }
            if (list10.Count > 0)
            {
                this.m_EmptyLabel.gameObject.SetActive(false);
                foreach (clsMapCtrl_QuestInfo info4 in list10)
                {
                    object[] o = new object[] { info4 };
                    base.Add(o);
                }
            }
            else
            {
                this.m_EmptyLabel.gameObject.SetActive(true);
                this.m_EmptyLabel.text = LocalizationManager.Get("COMBINE_LIST_VIEW_EMPTY");
            }
        }
    }

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        EXIT
    }
}

