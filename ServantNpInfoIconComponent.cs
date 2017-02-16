using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantNpInfoIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite backSprite;
    private UserServantEntity baseUsrSvtData;
    protected ClickDelegate callbackFunc;
    protected BattleCommandComponent commandCard;
    [SerializeField]
    protected GameObject commandCardBase;
    [SerializeField]
    protected GameObject commandCardPrefab;
    [SerializeField]
    protected UISprite frameSprite;
    [SerializeField]
    protected UIIconLabel iconLabel;
    [HideInInspector]
    private int index;
    [SerializeField]
    protected UILabel levelLabel;
    [SerializeField]
    protected UISprite noSelectMskImg;
    [SerializeField]
    protected UISprite skillIconSprite;
    protected int tdCardId;
    [SerializeField]
    protected UILabel tdDetailLabel;
    protected int tdId;
    protected int tdLv;
    [SerializeField]
    protected UILabel tdNameLabel;
    [SerializeField]
    protected UILabel tdNameRubyLabel;

    public void getNpInfo()
    {
        Debug.Log("!!!!! GetNpInfo : " + this.index);
    }

    public void OnClickNp()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (this.callbackFunc != null)
        {
            this.callbackFunc(true, this.index);
        }
    }

    public void setDispSelectMskImg(bool isShow)
    {
    }

    private void setNpIconImg()
    {
        if (this.tdId > 0)
        {
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.commandCardPrefab);
            Transform transform = go.transform;
            Vector3 localScale = go.transform.localScale;
            go.name = "CommandCard";
            transform.parent = this.commandCardBase.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = localScale;
            BattleCommandComponent component = go.GetComponent<BattleCommandComponent>();
            component.setDepth(110);
            component.setTarget(base.gameObject);
            BattleCommandData indata = new BattleCommandData((BattleCommand.TYPE) this.tdCardId, this.baseUsrSvtData.svtId, this.baseUsrSvtData.limitCount) {
                treasureDvc = this.tdId
            };
            component.setData(indata, null);
            component.setShader("Unlit/Transparent Colored");
            component.updateView(true);
            this.commandCard = component;
            NGUITools.SetLayer(go, this.commandCardBase.layer);
        }
    }

    public void SetNpInfo(int idx, UserServantEntity baseData, int npId, int npLv, string npRuby, string npName, string npDetail, int npCardId, ClickDelegate callback)
    {
        this.backSprite.gameObject.SetActive(false);
        this.index = idx;
        this.baseUsrSvtData = baseData;
        this.tdId = npId;
        this.tdCardId = npCardId;
        this.tdNameRubyLabel.text = npRuby;
        this.tdNameLabel.text = npName;
        this.tdDetailLabel.text = npDetail;
        this.setNpLv(npLv);
        this.setNpIconImg();
        this.callbackFunc = callback;
    }

    private void setNpLv(int lv)
    {
        this.levelLabel.text = string.Format(LocalizationManager.Get("LEVEL_INFO"), lv);
        this.levelLabel.gameObject.SetActive(true);
    }

    public delegate void ClickDelegate(bool isDecide, int idx);
}

