using System;
using UnityEngine;

public class ServantStatusListViewItemDrawNp : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UICommonButton baseButton;
    protected BattleCommandComponent commandCard;
    [SerializeField]
    protected GameObject commandCardBase;
    [SerializeField]
    protected GameObject commandCardPrefab;
    [SerializeField]
    protected UILabel explanationLabel;
    [SerializeField]
    protected UISprite[] guageSpriteList;
    [SerializeField]
    protected UILabel maxGuageLabel;
    [SerializeField]
    protected UILabel nameLabel;
    [SerializeField]
    protected UILabel npLevelLabel;
    [SerializeField]
    protected UILabel npRankLabel;
    [SerializeField]
    protected UILabel npTypeLabel;
    [SerializeField]
    protected UILabel rubyLabel;
    protected int tdCardId;
    protected TreasureDvcEntity tdEntity;
    protected int tdId;
    protected int tdLv;
    protected int tdMaxLv;
    protected int tdMaxRank;
    protected int tdRank;

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.NP;

    public override void PlayBattle(ServantStatusListViewItem item)
    {
        if (this.tdEntity != null)
        {
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.commandCardPrefab);
            Transform transform = go.transform;
            Vector3 localScale = go.transform.localScale;
            go.name = "CommandCard";
            transform.parent = this.commandCardBase.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = localScale;
            go.AddComponent<UIDragScrollView>();
            BattleCommandComponent component = go.GetComponent<BattleCommandComponent>();
            component.setDepth(110);
            component.setTarget(base.gameObject);
            BattleCommandData indata = new BattleCommandData((BattleCommand.TYPE) this.tdCardId, item.Servant.id, item.LimitCount) {
                treasureDvc = this.tdEntity.id
            };
            component.setData(indata, null);
            component.setShader("Unlit/Transparent Colored");
            component.updateView(true);
            this.commandCard = component;
            NGUITools.SetLayer(go, this.commandCardBase.layer);
        }
    }

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        this.tdEntity = null;
        this.tdId = 0;
        this.tdLv = 0;
        this.tdMaxLv = 0;
        this.tdRank = 0;
        this.tdMaxRank = 0;
        this.tdCardId = 0;
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            string str;
            string str2;
            int num;
            item.GetNpInfo(out this.tdId, out this.tdLv, out this.tdMaxLv, out this.tdRank, out this.tdMaxRank, out str, out str2, out num, out this.tdCardId);
            this.nameLabel.text = string.Empty;
            this.rubyLabel.text = string.Empty;
            this.npRankLabel.text = string.Empty;
            this.npTypeLabel.text = string.Empty;
            this.npLevelLabel.text = string.Empty;
            this.maxGuageLabel.text = string.Empty;
            this.explanationLabel.text = string.Empty;
            for (int i = 0; i < this.guageSpriteList.Length; i++)
            {
                this.guageSpriteList[i].spriteName = "img_npgage_bg";
            }
            if (this.tdId > 0)
            {
                this.tdEntity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(this.tdId);
                if (this.tdEntity != null)
                {
                    this.rubyLabel.text = this.tdEntity.ruby;
                    this.nameLabel.text = this.tdEntity.name;
                    this.npRankLabel.text = this.tdEntity.rank;
                    this.npTypeLabel.text = this.tdEntity.typeText;
                    this.npLevelLabel.text = (this.tdLv <= 0) ? string.Empty : string.Concat(new object[] { string.Empty, this.tdLv, "/", this.tdMaxLv });
                    this.maxGuageLabel.text = string.Format(LocalizationManager.Get("SERVANT_STATUS_NP_GUAGE_MESSAGE"), string.Empty + (num * 100));
                    WrapControlText.textAdjust(this.explanationLabel, str2);
                    for (int j = 0; j < this.guageSpriteList.Length; j++)
                    {
                        if (j >= num)
                        {
                            break;
                        }
                        this.guageSpriteList[j].spriteName = "img_npgage_" + ((j + 1) * 100);
                    }
                }
            }
        }
    }
}

