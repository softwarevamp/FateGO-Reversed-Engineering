using System;
using System.Runtime.CompilerServices;

public class ServantSortSelectMenu : BaseDialog
{
    protected ListViewSort baseSortInfo;
    public UICommonButton cancelButton;
    public UILabel cancelLabel;
    public UICommonButton clearButton;
    public UILabel clearLabel;
    protected System.Action closeCallbackFunc;
    protected static ListViewSort commonServantEquipSortInfo = new ListViewSort("ServantSortSelect2", ListViewSort.SortKind.LEVEL, false);
    protected static ListViewSort commonServantSortInfo = new ListViewSort("ServantSortSelect1", ListViewSort.SortKind.LEVEL, false);
    public UICommonButton decideButton;
    public UILabel decideLabel;
    public UILabel explanation2Label;
    public UILabel explanationLabel;
    public UICommonButton filter10Button;
    public UILabel filter10Label;
    public UICommonButton filter1Button;
    public UILabel filter1Label;
    public UICommonButton filter2Button;
    public UILabel filter2Label;
    public UICommonButton filter3Button;
    public UILabel filter3Label;
    public UICommonButton filter4Button;
    public UILabel filter4Label;
    public UICommonButton filter5Button;
    public UILabel filter5Label;
    public UICommonButton filter6Button;
    public UILabel filter6Label;
    public UICommonButton filter7Button;
    public UILabel filter7Label;
    public UICommonButton filter8Button;
    public UILabel filter8Label;
    public UICommonButton filter9Button;
    public UILabel filter9Label;
    protected Kind kind;
    protected ListViewSort operationSortInfo;
    protected const string SORT_SAVE_KEY = "ServantSortSelect";
    public UICommonButton sort10Button;
    public UILabel sort10Label;
    public UICommonButton sort1Button;
    public UILabel sort1Label;
    public UICommonButton sort2Button;
    public UILabel sort2Label;
    public UICommonButton sort3Button;
    public UILabel sort3Label;
    public UICommonButton sort4Button;
    public UILabel sort4Label;
    public UICommonButton sort5Button;
    public UILabel sort5Label;
    public UICommonButton sort6Button;
    public UILabel sort6Label;
    public UICommonButton sort7Button;
    public UILabel sort7Label;
    public UICommonButton sort8Button;
    public UILabel sort8Label;
    public UICommonButton sort9Button;
    public UILabel sort9Label;
    protected State state;
    public UILabel title2Label;
    public UILabel titleLabel;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result);
        }
    }

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.state = State.CLOSE;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.state = State.INPUT;
    }

    public static ListViewSort GetServantEquipSortInfo()
    {
        commonServantEquipSortInfo.Load();
        return commonServantEquipSortInfo;
    }

    public static ListViewSort GetServantSortInfo()
    {
        commonServantSortInfo.Load();
        return commonServantSortInfo;
    }

    public void Init()
    {
        this.state = State.INIT;
        base.Init();
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.Callback(false);
        }
    }

    public void OnClickClear()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.ClearFilter();
            if (this.kind == Kind.SERVANT_EQUIP)
            {
                this.operationSortInfo.SetServantEquip(this.kind == Kind.SERVANT_EQUIP);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_EXP_UP, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_STATUS_UP, false);
            }
            this.SetButtenSelect();
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.SetButtenEnable(false);
            this.state = State.SELECTED;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.baseSortInfo.Set(this.operationSortInfo);
            this.Callback(true);
        }
    }

    public void OnClickFilterClass1()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_1_13);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass10()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_STATUS_UP);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass2()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_2_14);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass3()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_3_15);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass4()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_4_16);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass5()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_5_17);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass6()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_6_18);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass7()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_7_19);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass8()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_ETC);
            this.SetButtenSelect();
        }
    }

    public void OnClickFilterClass9()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SwitchFilter(ListViewSort.FilterKind.CLASS_EXP_UP);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortAmount()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.AMOUNT);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortAttack()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.ATK);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortCost()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.COST);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortCreate()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.CREATE);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortFriendShip()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.FRIENDSHIP);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortHp()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.HP);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortLevel()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.LEVEL);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortNpLevel()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.NP_LEVEL);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortParty()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.PARTY);
            this.SetButtenSelect();
        }
    }

    public void OnClickSortRarity()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetKind(ListViewSort.SortKind.RARITY);
            this.SetButtenSelect();
        }
    }

    public void Open(Kind kind, ListViewSort sort, CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.kind = kind;
            this.callbackFunc = callback;
            if (sort != null)
            {
                this.baseSortInfo = sort;
            }
            else
            {
                this.baseSortInfo = (this.kind != Kind.SERVANT_EQUIP) ? commonServantSortInfo : commonServantEquipSortInfo;
                this.baseSortInfo.Load();
            }
            if (this.kind == Kind.SERVANT_EQUIP)
            {
                this.baseSortInfo.SetServantEquip(kind == Kind.SERVANT_EQUIP);
                this.baseSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_EXP_UP, false);
                this.baseSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_STATUS_UP, false);
            }
            this.operationSortInfo = new ListViewSort(this.baseSortInfo);
            base.gameObject.SetActive(true);
            this.titleLabel.text = LocalizationManager.Get("SERVANT_SORT_TITLE");
            this.title2Label.text = LocalizationManager.Get("SERVANT_SORT_TITLE2");
            this.explanationLabel.text = LocalizationManager.Get("SERVANT_SORT_EXPLANATION");
            this.explanation2Label.text = LocalizationManager.Get("SERVANT_SORT_EXPLANATION2");
            this.decideLabel.text = LocalizationManager.Get("SERVANT_SORT_DECIDE");
            this.clearLabel.text = LocalizationManager.Get("SERVANT_SORT_CLEAR");
            this.cancelLabel.text = LocalizationManager.Get("SERVANT_SORT_CANCEL");
            this.sort1Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.LEVEL);
            this.sort2Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.HP);
            this.sort3Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.ATK);
            this.sort4Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.COST);
            this.sort5Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.RARITY);
            this.sort6Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.NP_LEVEL);
            this.sort7Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.FRIENDSHIP);
            this.sort8Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.PARTY);
            this.sort9Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.AMOUNT);
            this.sort10Label.text = this.operationSortInfo.GetKindText(ListViewSort.SortKind.CREATE);
            this.filter1Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_1");
            this.filter2Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_2");
            this.filter3Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_3");
            this.filter4Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_4");
            this.filter5Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_5");
            this.filter6Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_6");
            this.filter7Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_7");
            if (this.kind == Kind.SERVANT_EQUIP)
            {
                this.filter8Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_8_2");
            }
            else
            {
                this.filter8Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_8");
            }
            this.filter9Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_9");
            this.filter10Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_10");
            this.SetButtenSelect();
            this.SetButtenEnable(true);
            this.state = State.OPEN;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    protected void SetButtenEnable(bool isEnable)
    {
        this.decideButton.enabled = isEnable;
        this.cancelButton.enabled = isEnable;
        this.clearButton.enabled = isEnable;
        this.sort1Button.enabled = isEnable;
        this.sort2Button.enabled = isEnable;
        this.sort3Button.enabled = isEnable;
        this.sort4Button.enabled = isEnable;
        this.sort5Button.enabled = isEnable;
        this.sort6Button.gameObject.SetActive(this.kind != Kind.SERVANT_EQUIP);
        this.sort6Button.enabled = (this.kind != Kind.SERVANT_EQUIP) ? isEnable : false;
        this.sort6Button.SetState((this.kind != Kind.SERVANT_EQUIP) ? UICommonButtonColor.State.Normal : UICommonButtonColor.State.Disabled, false);
        this.sort7Button.gameObject.SetActive(this.kind != Kind.SERVANT_EQUIP);
        this.sort7Button.enabled = (this.kind != Kind.SERVANT_EQUIP) ? isEnable : false;
        this.sort7Button.SetState((this.kind != Kind.SERVANT_EQUIP) ? UICommonButtonColor.State.Normal : UICommonButtonColor.State.Disabled, false);
        this.sort8Button.enabled = isEnable;
        this.sort9Button.enabled = isEnable;
        this.sort10Button.enabled = isEnable;
        this.filter1Button.enabled = isEnable;
        this.filter2Button.enabled = isEnable;
        this.filter3Button.enabled = isEnable;
        this.filter4Button.enabled = isEnable;
        this.filter5Button.enabled = isEnable;
        this.filter6Button.enabled = isEnable;
        this.filter7Button.enabled = isEnable;
        this.filter8Button.enabled = isEnable;
        this.filter9Button.gameObject.SetActive(this.kind != Kind.SERVANT_EQUIP);
        this.filter9Button.enabled = isEnable;
        this.filter9Button.SetState((this.kind != Kind.SERVANT_EQUIP) ? UICommonButtonColor.State.Normal : UICommonButtonColor.State.Disabled, false);
        this.filter10Button.gameObject.SetActive(this.kind != Kind.SERVANT_EQUIP);
        this.filter10Button.enabled = isEnable;
        this.filter10Button.SetState((this.kind != Kind.SERVANT_EQUIP) ? UICommonButtonColor.State.Normal : UICommonButtonColor.State.Disabled, false);
    }

    protected void SetButtenSelect()
    {
        this.sort1Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.LEVEL) ? "btn_bg_04" : "btn_bg_03";
        this.sort2Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.HP) ? "btn_bg_04" : "btn_bg_03";
        this.sort3Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.ATK) ? "btn_bg_04" : "btn_bg_03";
        this.sort4Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.COST) ? "btn_bg_04" : "btn_bg_03";
        this.sort5Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.RARITY) ? "btn_bg_04" : "btn_bg_03";
        this.sort6Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.NP_LEVEL) ? "btn_bg_04" : "btn_bg_03";
        this.sort7Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.FRIENDSHIP) ? "btn_bg_04" : "btn_bg_03";
        this.sort8Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.PARTY) ? "btn_bg_04" : "btn_bg_03";
        this.sort9Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.AMOUNT) ? "btn_bg_04" : "btn_bg_03";
        this.sort10Button.GetComponent<UISprite>().spriteName = (this.operationSortInfo.Kind != ListViewSort.SortKind.CREATE) ? "btn_bg_04" : "btn_bg_03";
        this.filter1Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_1_13) ? "btn_bg_04" : "btn_bg_03";
        this.filter2Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_2_14) ? "btn_bg_04" : "btn_bg_03";
        this.filter3Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_3_15) ? "btn_bg_04" : "btn_bg_03";
        this.filter4Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_4_16) ? "btn_bg_04" : "btn_bg_03";
        this.filter5Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_5_17) ? "btn_bg_04" : "btn_bg_03";
        this.filter6Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_6_18) ? "btn_bg_04" : "btn_bg_03";
        this.filter7Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_7_19) ? "btn_bg_04" : "btn_bg_03";
        this.filter8Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_ETC) ? "btn_bg_04" : "btn_bg_03";
        this.filter9Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_EXP_UP) ? "btn_bg_04" : "btn_bg_03";
        this.filter10Button.GetComponent<UISprite>().spriteName = !this.operationSortInfo.GetFilter(ListViewSort.FilterKind.CLASS_STATUS_UP) ? "btn_bg_04" : "btn_bg_03";
    }

    public delegate void CallbackFunc(bool result);

    public enum Kind
    {
        SERVANT,
        SERVANT_EQUIP
    }

    protected enum State
    {
        INIT,
        OPEN,
        INPUT,
        SELECTED,
        CLOSE
    }
}

