using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class QuestBoardListViewManager : ListViewManager
{
    private static readonly float ALPHA_ANIM_SPD_RATE = 0.5f;
    private static readonly float ALPHA_ANIM_TIME_INTERVAL = 3f;
    public static readonly float EXIT_TIME = 0.25f;
    protected InitMode initMode;
    public static readonly float INTO_TIME = 0.5f;
    private float mAlphaAnimNow;
    private float mAlphaAnimTgt;
    private float mAlphaAnimTimeOld;
    private float mBaseClipRange;
    private BoxCollider mBoxCollider;
    private bool mIsDoing_Slide;
    [SerializeField]
    private UILabel mNoneLabel;
    private GameObject mNoneLabelParent;
    [SerializeField]
    private ScrTerminalListTop mTerminalList;
    [SerializeField]
    private TerminalSceneComponent mTerminalScene;
    [SerializeField]
    private Camera mUICamera;
    public static readonly float OUT_POS_OFS_X = 532f;
    private BattleResultComponent resultwindow;
    [SerializeField]
    private GameObject resultwindowPrefab;
    [SerializeField]
    private GameObject showItemwindowPrefab;

    private void Awake()
    {
        this.mBaseClipRange = base.gameObject.GetComponent<UIWidget>().height;
        this.mBoxCollider = base.gameObject.GetComponent<BoxCollider>();
        this.mBoxCollider.enabled = false;
        this.mNoneLabelParent = this.mNoneLabel.gameObject.GetParent().gameObject;
        this.mNoneLabelParent.SetActive(false);
    }

    public void callbackRequest(string result)
    {
        if (result != "ng")
        {
            this.CreateResultWindow();
            this.setResult(result);
            this.showResult(base.gameObject, "END_PROC");
        }
    }

    public void CreateList(QuestBoardListViewItem.InfoKind info_kind, List<clsMapCtrl_QuestInfo> qinf_list, float pos_itvl_y, float clip_w)
    {
        this.DestroyList();
        base.CreateList(0);
        UIWidget component = base.gameObject.GetComponent<UIWidget>();
        BoxCollider collider = base.gameObject.GetComponent<BoxCollider>();
        float y = pos_itvl_y * (QuestBoardListViewItemDraw.SCRL_OBJ_DUMMY_NUM + 1);
        UIPanel panel = base.scrollView.gameObject.GetComponent<UIPanel>();
        Vector4 baseClipRegion = panel.baseClipRegion;
        float num2 = this.mBaseClipRange - y;
        base.seed.arrangementPich.y = -pos_itvl_y;
        component.height = (int) y;
        collider.size = new Vector3(collider.size.x, y, 0f);
        this.clipRange.y = y + QuestBoardListViewItemDraw.CLIP_RANGE_MARGIN;
        baseClipRegion.y = num2 / 2f;
        baseClipRegion.z = clip_w;
        baseClipRegion.w = y;
        panel.baseClipRegion = baseClipRegion;
        this.mNoneLabelParent.SetActive(qinf_list.Count <= 0);
        this.mNoneLabel.text = LocalizationManager.Get("QUEST_NONE");
        int num3 = qinf_list.Count + QuestBoardListViewItemDraw.SCRL_OBJ_DUMMY_NUM;
        for (int i = 0; i < num3; i++)
        {
            clsMapCtrl_QuestInfo qinf = null;
            if (i < qinf_list.Count)
            {
                qinf = qinf_list[i];
            }
            QuestBoardListViewItem item = new QuestBoardListViewItem(i, info_kind, qinf, this);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    private void CreateResultWindow()
    {
        BattleResultComponent component = NGUITools.AddChild(base.transform.parent.parent.gameObject, this.resultwindowPrefab).GetComponent<BattleResultComponent>();
        if (component != null)
        {
            this.resultwindow = component;
        }
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public Camera GetCamera() => 
        this.mUICamera;

    public InitMode GetInitMode() => 
        this.initMode;

    public PartyServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyServantListViewItem);
        }
        return null;
    }

    public UIScrollView GetScrollView() => 
        base.scrollView;

    private void OnChangeAlphaAnim()
    {
        List<QuestBoardListViewObject> objectList = this.ObjectList;
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].OnChangeAlphaAnim();
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
        QuestBoardListViewItem item = obj.GetItem() as QuestBoardListViewItem;
        clsMapCtrl_QuestInfo qinf = item.quest_info;
        if (qinf != null)
        {
            AreaBoardInfo mAreaBoardInfo = qinf.mAreaBoardInfo;
            bool flag = qinf.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Closed;
            SeManager.SystemSeKind kind = !flag ? SeManager.SystemSeKind.DECIDE : SeManager.SystemSeKind.WARNING;
            SoundManager.playSystemSe(kind);
            if (!flag)
            {
                switch (item.info_kind)
                {
                    case QuestBoardListViewItem.InfoKind.AREA:
                        this.mTerminalList.Click_Area(mAreaBoardInfo);
                        break;

                    case QuestBoardListViewItem.InfoKind.MAP:
                    case QuestBoardListViewItem.InfoKind.CALDEA:
                        this.mTerminalList.Click_Quest(qinf);
                        break;

                    case QuestBoardListViewItem.InfoKind.STORY:
                    case QuestBoardListViewItem.InfoKind.HERO:
                        TerminalPramsManager.SpotId = qinf.mfGetSpotID();
                        this.mTerminalList.Click_Shortcut(qinf.mfGetWarID(), qinf.mfGetQuestID());
                        break;
                }
            }
        }
    }

    protected void OnPressListView(ListViewObject obj)
    {
        QuestBoardListViewItem item = obj.GetItem() as QuestBoardListViewItem;
        List<GiftEntity> itemList = new List<GiftEntity>();
        StageEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<StageMaster>(DataNameKind.Kind.STAGE).getEntitys<StageEntity>();
        NpcDeckEntity[] entityArray2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcDeckMaster>(DataNameKind.Kind.NPC_DECK).getEntitys<NpcDeckEntity>();
        GiftEntity[] entityArray3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT).getEntitys<GiftEntity>();
        DropEntity[] entityArray4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<DropMaster>(DataNameKind.Kind.DROP).getEntitys<DropEntity>();
        foreach (StageEntity entity in entityArray)
        {
            if (entity.questId == item.quest_info.mfGetQuestID())
            {
                foreach (long num2 in entity.npcDeckIds)
                {
                    foreach (NpcDeckEntity entity2 in entityArray2)
                    {
                        if (entity2.id == num2)
                        {
                            foreach (long num5 in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcServantMaster>(DataNameKind.Kind.NPC_SERVANT).getEntityFromId<NpcServantEntity>(entity2.npcSvtId).dropIds)
                            {
                                foreach (DropEntity entity4 in entityArray4)
                                {
                                    if (entity4.id == num5)
                                    {
                                        foreach (GiftEntity entity5 in entityArray3)
                                        {
                                            if (entity5.id == entity4.giftId)
                                            {
                                                itemList.Add(entity5);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        ShowQuestItemComponent component = this.showItemwindowPrefab.GetComponent<ShowQuestItemComponent>();
        component.setResultData(itemList);
        component.Open();
    }

    protected void RequestListObject(QuestBoardListViewObject.InitMode mode, System.Action end_act = null)
    {
        <RequestListObject>c__AnonStoreyBF ybf = new <RequestListObject>c__AnonStoreyBF {
            end_act = end_act,
            <>f__this = this
        };
        List<QuestBoardListViewObject> objectList = this.ObjectList;
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].Init(mode);
        }
        if (this.initMode == InitMode.INTO)
        {
            this.mIsDoing_Slide = true;
            SlideFadeObject obj4 = base.gameObject.SafeGetComponent<SlideFadeObject>();
            float time = INTO_TIME;
            obj4.SlideIn(OUT_POS_OFS_X, time, 0f, new System.Action(ybf.<>m__1BF));
            this.ResetAlphaAnimTime();
        }
        else if (this.initMode == InitMode.EXIT)
        {
            this.mIsDoing_Slide = true;
            SlideFadeObject obj6 = base.gameObject.gameObject.SafeGetComponent<SlideFadeObject>();
            float num3 = EXIT_TIME;
            obj6.SlideOut(OUT_POS_OFS_X, num3, 0f, new System.Action(ybf.<>m__1C0));
            this.mNoneLabelParent.SetActive(false);
        }
        else
        {
            ybf.end_act.Call();
        }
    }

    private void ResetAlphaAnimTime()
    {
        this.mAlphaAnimNow = 1f;
        this.mAlphaAnimTgt = 1f;
        this.mAlphaAnimTimeOld = Time.realtimeSinceStartup;
    }

    public void SetFocusItem(int item_index)
    {
        base.MoveCenterItem(item_index + QuestBoardListViewItemDraw.FOCUS_IDX_OFFSET, true);
    }

    public bool SetMode(InitMode mode, System.Action end_act = null)
    {
        bool flag = false;
        if (this.mIsDoing_Slide)
        {
            flag = true;
        }
        if ((mode == InitMode.INTO) && (this.initMode != InitMode.NONE))
        {
            flag = true;
        }
        if ((mode == InitMode.EXIT) && (this.initMode != InitMode.INPUT))
        {
            flag = true;
        }
        if (flag)
        {
            end_act.Call();
            return false;
        }
        this.initMode = mode;
        base.IsInput = mode == InitMode.INPUT;
        this.mBoxCollider.enabled = base.IsInput;
        switch (mode)
        {
            case InitMode.VALID:
            case InitMode.INTO:
            case InitMode.EXIT:
                this.RequestListObject(QuestBoardListViewObject.InitMode.VALID, end_act);
                break;

            case InitMode.INPUT:
                this.RequestListObject(QuestBoardListViewObject.InitMode.INPUT, end_act);
                break;

            default:
                this.RequestListObject(QuestBoardListViewObject.InitMode.INVISIBLE, end_act);
                break;
        }
        return true;
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        QuestBoardListViewObject obj2 = obj as QuestBoardListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(QuestBoardListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(QuestBoardListViewObject.InitMode.VALID);
        }
    }

    public void setResult(string str)
    {
        this.resultwindow.Set(str);
    }

    public void SetupDisp()
    {
        List<QuestBoardListViewObject> objectList = this.ObjectList;
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].SetupDisp();
        }
    }

    public void showResult(GameObject target, string endevent)
    {
        this.resultwindow.Init();
        this.resultwindow.StartResult(target, endevent, BattleResultComponent.BattleResultType.BATTLE_DROP, null);
    }

    private void Update()
    {
        base.Update();
        this.UpdateAlphaAnim();
    }

    private void UpdateAlphaAnim()
    {
        this.mAlphaAnimNow += (this.mAlphaAnimTgt - this.mAlphaAnimNow) * ALPHA_ANIM_SPD_RATE;
        float num2 = Time.realtimeSinceStartup - this.mAlphaAnimTimeOld;
        if (num2 >= ALPHA_ANIM_TIME_INTERVAL)
        {
            this.mAlphaAnimTgt = 0f;
            this.mAlphaAnimTimeOld = Time.realtimeSinceStartup;
        }
        if ((this.mAlphaAnimTgt <= 0f) && (this.mAlphaAnimNow <= 0.01f))
        {
            this.mAlphaAnimNow = 0f;
            this.mAlphaAnimTgt = 1f;
            this.mAlphaAnimTimeOld = Time.realtimeSinceStartup;
            this.OnChangeAlphaAnim();
        }
    }

    public float AlphaAnimNow =>
        this.mAlphaAnimNow;

    public ScrTerminalListTop MTerminalList =>
        this.mTerminalList;

    public List<QuestBoardListViewObject> ObjectList
    {
        get
        {
            List<QuestBoardListViewObject> list = new List<QuestBoardListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    QuestBoardListViewObject component = obj2.GetComponent<QuestBoardListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    [CompilerGenerated]
    private sealed class <RequestListObject>c__AnonStoreyBF
    {
        internal QuestBoardListViewManager <>f__this;
        internal System.Action end_act;

        internal void <>m__1BF()
        {
            this.<>f__this.mIsDoing_Slide = false;
            QuestBoardListViewManager.InitMode mode = !this.<>f__this.mTerminalScene.IsTutorialActive ? QuestBoardListViewManager.InitMode.INPUT : QuestBoardListViewManager.InitMode.VALID;
            this.<>f__this.SetMode(mode, this.end_act);
        }

        internal void <>m__1C0()
        {
            this.<>f__this.mIsDoing_Slide = false;
            this.<>f__this.SetMode(QuestBoardListViewManager.InitMode.NONE, () => this.end_act.Call());
        }

        internal void <>m__1C1()
        {
            this.end_act.Call();
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

