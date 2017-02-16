using System;
using System.Runtime.CompilerServices;

public class ServantFilterSelectMenu : BaseDialog
{
    protected ListViewSort baseSortInfo;
    public UICommonButton cancelButton;
    public UILabel cancelLabel;
    public UICommonButton clearButton;
    public UILabel clearLabel;
    protected System.Action closeCallbackFunc;
    protected static ListViewSort commonServantSortInfo = new ListViewSort("ServantSortSelect1", ListViewSort.SortKind.LEVEL, false);
    public UICommonButton decideButton;
    public UILabel decideLabel;
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
    public UICommonButton initializeButton;
    public UILabel initializeLabel;
    protected Kind kind;
    protected ListViewSort operationSortInfo;
    protected const string SORT_SAVE_KEY = "ServantSortSelect";
    protected State state;
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

    public static ListViewSort GetServantFilterInfo()
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
            this.baseSortInfo.Save();
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

    public void OnClickInitialize()
    {
        if (this.state == State.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_EXP_UP, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_STATUS_UP, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_1_13, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_2_14, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_3_15, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_4_16, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_5_17, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_6_18, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_7_19, false);
            this.operationSortInfo.SetFilter(ListViewSort.FilterKind.CLASS_ETC, false);
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
                this.baseSortInfo.Load();
            }
            else
            {
                this.baseSortInfo = commonServantSortInfo;
                this.baseSortInfo.Load();
            }
            this.operationSortInfo = new ListViewSort(this.baseSortInfo);
            base.gameObject.SetActive(true);
            this.titleLabel.text = LocalizationManager.Get("SERVANT_SORT_TITLE2");
            this.explanationLabel.text = LocalizationManager.Get("SERVANT_SORT_EXPLANATION2");
            this.decideLabel.text = LocalizationManager.Get("SERVANT_SORT_DECIDE");
            this.clearLabel.text = LocalizationManager.Get("SERVANT_SORT_CLEAR");
            this.cancelLabel.text = LocalizationManager.Get("SERVANT_SORT_CANCEL");
            this.initializeLabel.text = LocalizationManager.Get("SERVANT_SORT_RESET");
            this.filter1Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_1");
            this.filter2Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_2");
            this.filter3Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_3");
            this.filter4Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_4");
            this.filter5Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_5");
            this.filter6Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_6");
            this.filter7Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_7");
            this.filter8Label.text = LocalizationManager.Get("SERVANT_SORT_FILTER_KIND_8");
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
        this.initializeButton.enabled = isEnable;
        this.filter1Button.enabled = isEnable;
        this.filter2Button.enabled = isEnable;
        this.filter3Button.enabled = isEnable;
        this.filter4Button.enabled = isEnable;
        this.filter5Button.enabled = isEnable;
        this.filter6Button.enabled = isEnable;
        this.filter7Button.enabled = isEnable;
        this.filter8Button.enabled = isEnable;
        this.filter9Button.enabled = isEnable;
        this.filter10Button.enabled = isEnable;
    }

    protected void SetButtenSelect()
    {
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
        SERVANT
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

