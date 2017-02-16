using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantStatusListViewItemDrawCommand : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected UICommonButton[] battleCharaLevelButtonList;
    [SerializeField]
    protected UISprite[] battleCharaLevelSpriteList;
    [SerializeField]
    protected UISprite[] battleCharaLevelTitleSpriteList;
    [SerializeField]
    protected GameObject[] commandCardBaseList;
    protected BattleCommandData[] commandCardDataList;
    protected BattleCommandComponent[] commandCardList;
    [SerializeField]
    protected GameObject commandCardPrefab;
    [SerializeField]
    protected UILabel explanationLabel;
    protected bool isBattlePlay;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.COMMAND;

    public override void ModifyCommandCard(ServantStatusListViewItem item)
    {
        if (this.commandCardList == null)
        {
            this.commandCardList = new BattleCommandComponent[this.commandCardBaseList.Length];
            this.commandCardDataList = new BattleCommandData[this.commandCardBaseList.Length];
            for (int i = 0; i < this.commandCardBaseList.Length; i++)
            {
                int svtId = item.SvtId;
                int limitCountByImageLimit = ImageLimitCount.GetLimitCountByImageLimit(item.CommandCardLimitCount);
                int loadLimitCount = ImageLimitCount.GetLimitCountByImageLimit(item.DispLimitCount);
                BattleCommand.TYPE nONE = BattleCommand.TYPE.NONE;
                if ((item.Servant.cardIds != null) && (i < item.Servant.cardIds.Length))
                {
                    nONE = (BattleCommand.TYPE) item.Servant.cardIds[i];
                }
                switch (nONE)
                {
                    case BattleCommand.TYPE.ARTS:
                    case BattleCommand.TYPE.BUSTER:
                    case BattleCommand.TYPE.QUICK:
                    {
                        GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.commandCardPrefab);
                        Transform transform = go.transform;
                        Vector3 localScale = go.transform.localScale;
                        go.name = "CommandCard(" + (i + 1) + ")";
                        transform.parent = this.commandCardBaseList[i].transform;
                        transform.localPosition = Vector3.zero;
                        transform.localRotation = Quaternion.identity;
                        transform.localScale = localScale;
                        go.AddComponent<UIDragScrollView>();
                        BattleCommandComponent component = go.GetComponent<BattleCommandComponent>();
                        component.setDepth(110);
                        component.setTarget(base.gameObject);
                        this.commandCardDataList[i] = new BattleCommandData(nONE, svtId, limitCountByImageLimit, loadLimitCount);
                        component.setData(this.commandCardDataList[i], null);
                        component.setShader("Unlit/Transparent Colored");
                        component.updateView(true);
                        this.commandCardList[i] = component;
                        NGUITools.SetLayer(go, this.commandCardBaseList[i].layer);
                        break;
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < this.commandCardBaseList.Length; j++)
            {
                BattleCommandComponent component2 = this.commandCardList[j];
                if (component2 != null)
                {
                    int num6 = ImageLimitCount.GetLimitCountByImageLimit(item.CommandCardLimitCount);
                    this.commandCardDataList[j].svtlimit = num6;
                    component2.setData(this.commandCardDataList[j], null);
                    component2.updateView(true);
                }
            }
        }
        this.SetupBattleButton(item, false);
    }

    public override void PlayBattle(ServantStatusListViewItem item)
    {
        this.isBattlePlay = true;
    }

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            if (item.UserServant != null)
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_COMMAND_CARD");
            }
            else if (item.UserServantCollection != null)
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_COMMAND_CARD2");
            }
            else
            {
                this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_COMMAND_CARD3");
            }
            this.SetupBattleButton(item, true);
        }
    }

    protected void SetupBattleButton(ServantStatusListViewItem item, bool isInit = false)
    {
        int commandCardLimitCount = item.CommandCardLimitCount;
        int maxCommandCardLimitCount = item.MaxCommandCardLimitCount;
        for (int i = 0; i < this.battleCharaLevelButtonList.Length; i++)
        {
            bool flag = i == commandCardLimitCount;
            bool flag2 = i <= maxCommandCardLimitCount;
            bool flag3 = (this.isBattlePlay && flag2) && ((item.UserServant != null) || (item.UserServantCollection != null));
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
            if (flag && flag3)
            {
                this.battleCharaLevelButtonList[i].SetColliderEnable(false, isInit || !flag3);
            }
            else
            {
                this.battleCharaLevelButtonList[i].SetButtonEnable(!flag && flag3, isInit || !flag3);
            }
        }
    }
}

