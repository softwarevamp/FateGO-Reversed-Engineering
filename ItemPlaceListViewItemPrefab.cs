using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ItemPlaceListViewItemPrefab : BaseScrollViewItem
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache8;
    public UILabel m_ap;
    public UISprite m_bg;
    private clsMapCtrl_QuestInfo m_info;
    public UILabel m_level;
    public UILabel m_name;
    public UILabel m_time;
    [SerializeField]
    private ServantClassIconComponent[] mClassIcons;
    public static readonly Color OVER_AP_COLOR = new Color(0.9019608f, 0f, 0f);

    public void Click()
    {
        Debug.Log("button click :: " + this.m_info.mfGetMine().name);
        int num = this.m_info.mfGetWarID();
        int num2 = this.m_info.mfGetQuestID();
        int num3 = this.m_info.mfGetQuestPhase();
        QuestPhaseEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(num2, num3 + 1);
        UserGameEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        BattleSetupInfo data = new BattleSetupInfo {
            warId = num,
            questId = num2,
            questPhase = (entity == null) ? num3 : (num3 + 1),
            isQuestNew = SingletonTemplate<clsQuestCheck>.Instance.mfCheck_IsQuestNew(num2),
            deckId = (entity2 == null) ? 0L : entity2.activeDeckId,
            followerId = 0L,
            isChildFollower = false
        };
        int num4 = entity2.getActMax();
        int num5 = entity2.getAct();
        int apCalcVal = this.m_info.GetApCalcVal();
        int needAp = this.m_info.mfGetMine().getActConsume(apCalcVal);
        if (needAp > num4)
        {
            string title = LocalizationManager.Get("SHORT_DLG_TITLE");
            string message = LocalizationManager.Get("QUEST_AP_MAX_OVER");
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, <>f__am$cache8, -1);
        }
        else
        {
            int num8;
            int num9;
            UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
            master.getCount(out num8, out num9);
            if (master.CheckServantAdd(1))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num8, entity2.svtKeep, false, true, delegate (ServantFrameShortDlgComponent.resultClicked result) {
                    <Click>c__AnonStoreyB1 yb = new <Click>c__AnonStoreyB1 {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(yb.<>m__1A4));
                });
            }
            else if (master.CheckEquipAdd(1))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num9, entity2.svtEquipKeep, true, true, delegate (ServantFrameShortDlgComponent.resultClicked result) {
                    <Click>c__AnonStoreyB2 yb = new <Click>c__AnonStoreyB2 {
                        result = result,
                        <>f__this = this
                    };
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(new System.Action(yb.<>m__1A5));
                });
            }
            else if (needAp > num5)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenApRecoverItemListDialog(needAp, new ApRecoverDlgComponent.CallbackFunc(this.EndRecoverUserGameRecover));
            }
            else
            {
                SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.Follower, SceneManager.FadeType.BLACK, data);
            }
        }
    }

    private void EndPurchaseSvtEquipFrame(ServantEquipFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipFramePurchaseMenu();
    }

    private void EndPurchaseSvtFrame(ServantFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFramePurchaseMenu();
    }

    private void EndRecoverUserGameRecover(ApRecoverDlgComponent.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseApRecoverItemListDialog();
    }

    private static string GetRestTimeText(long end_time) => 
        (LocalizationManager.Get("TIME_REST_QUEST") + LocalizationManager.GetRestTime(end_time));

    public override void SetItem(params object[] o)
    {
        clsMapCtrl_QuestInfo info = (clsMapCtrl_QuestInfo) o[0];
        this.m_info = info;
        this.m_name.text = info.mfGetMine().getQuestName().ToString();
        int apCalcVal = info.GetApCalcVal();
        int num2 = info.mfGetMine().getActConsume(apCalcVal);
        this.m_ap.text = string.Empty + num2;
        int num3 = info.mfGetQuestPhase() + 1;
        if (num3 > info.mfGetPhaseMax())
        {
            num3 = info.mfGetPhaseMax();
        }
        QuestPhaseEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestPhaseMaster>(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(info.mfGetMine().getQuestId(), num3);
        if (((entity != null) && (entity.classIds != null)) && (entity.classIds.Length > 0))
        {
            int[] classIds = entity.classIds;
            for (int i = 0; i < this.mClassIcons.Length; i++)
            {
                ServantClassIconComponent component = this.mClassIcons[i];
                component.gameObject.SetActive(i < classIds.Length);
                if (component.gameObject.activeSelf)
                {
                    component.Set(classIds[i]);
                }
            }
        }
        this.m_level.text = info.mfGetMine().getRecommendLv().ToString();
        UserGameEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        int num5 = entity2.getActMax();
        int num6 = entity2.getAct();
        if ((num2 > num5) || (num2 > num6))
        {
            Color color = OVER_AP_COLOR;
            if (info.mfGetDispType() == clsMapCtrl_QuestInfo.enDispType.Closed)
            {
                color = (Color) (color * 0.5f);
            }
            this.m_ap.color = color;
        }
        if (info.mfGetMine().type == 2)
        {
            this.m_time.gameObject.SetActive(false);
        }
        else
        {
            this.m_time.gameObject.SetActive(true);
            string restTimeText = GetRestTimeText(info.GetEndTime());
            this.m_time.text = restTimeText;
        }
        if (info.mfGetTouchType() == clsMapCtrl_QuestInfo.enTouchType.Disable)
        {
            base.gameObject.GetComponent<UISprite>().color = new Color(0.372549f, 0.372549f, 0.372549f);
            base.gameObject.GetComponent<UIButton>().enabled = false;
        }
        else
        {
            base.gameObject.GetComponent<UISprite>().color = new Color(1f, 1f, 1f);
            base.gameObject.GetComponent<UIButton>().enabled = true;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <Click>c__AnonStoreyB1
    {
        internal ItemPlaceListViewItemPrefab <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1A4()
        {
            switch (this.result)
            {
                case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFramePurchaseMenu(new ServantFramePurchaseMenu.CallbackFunc(this.<>f__this.EndPurchaseSvtFrame), null);
                    break;

                case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantCombine"));
                    break;

                case ServantFrameShortDlgComponent.resultClicked.SELL:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 0));
                    break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Click>c__AnonStoreyB2
    {
        internal ItemPlaceListViewItemPrefab <>f__this;
        internal ServantFrameShortDlgComponent.resultClicked result;

        internal void <>m__1A5()
        {
            switch (this.result)
            {
                case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipFramePurchaseMenu(new ServantEquipFramePurchaseMenu.CallbackFunc(this.<>f__this.EndPurchaseSvtEquipFrame), null);
                    break;

                case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantEQCombine"));
                    break;

                case ServantFrameShortDlgComponent.resultClicked.SELL:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 1));
                    break;
            }
        }
    }
}

