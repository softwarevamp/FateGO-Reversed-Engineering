using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleSelectMainSubServantWindow : BattleWindowComponent
{
    public UIButton actButton;
    private SelectedCallBack callBack;
    private bool isSelected = true;
    private int mainSelect_uniqueId;
    public BattleSelectServantComponent[] mainSvtList;
    public GameObject parentPanel;
    private int subSelect_uniqueId;
    public BattleSelectServantComponent[] subSvtList;
    public UILabel title_label;

    public override void Close(BattleWindowComponent.EndCall call = null)
    {
        this.parentPanel.SetActive(false);
        this.isSelected = true;
        base.Close(call);
    }

    public void endErrorDialog(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.isSelected = false;
    }

    public void onActionButton()
    {
        if (!this.isSelected)
        {
            if ((0 < this.mainSelect_uniqueId) && (0 < this.subSelect_uniqueId))
            {
                this.callBack(true, this.mainSelect_uniqueId, this.subSelect_uniqueId);
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("BATTLE_SELECTSUBERROR_NOSELECT_TITLE"), LocalizationManager.Get("BATTLE_SELECTSUBERROR_NOSELECT_CONF"), new NotificationDialog.ClickDelegate(this.endErrorDialog), -1);
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.isSelected = true;
        }
    }

    public void onCloseButton()
    {
        if (!this.isSelected)
        {
            SoundManager.playSe("ba19");
            this.callBack(false, this.mainSelect_uniqueId, this.subSelect_uniqueId);
            this.isSelected = true;
        }
    }

    public override void Open(BattleWindowComponent.EndCall call = null)
    {
        this.parentPanel.SetActive(true);
        this.actButton.isEnabled = false;
        this.actButton.SetState(UIButtonColor.State.Disabled, true);
        this.mainSelect_uniqueId = 0;
        this.subSelect_uniqueId = 0;
        this.isSelected = false;
        base.Open(call);
    }

    public void selectSvt(POSITION position, int uniqueId)
    {
        if (position == POSITION.MAIN)
        {
            if (this.mainSelect_uniqueId == uniqueId)
            {
                uniqueId = 0;
            }
            for (int i = 0; i < this.mainSvtList.Length; i++)
            {
                this.mainSvtList[i].setTarget(uniqueId);
            }
            this.mainSelect_uniqueId = uniqueId;
        }
        else if (position == POSITION.SUB)
        {
            if (this.subSelect_uniqueId == uniqueId)
            {
                uniqueId = 0;
            }
            for (int j = 0; j < this.subSvtList.Length; j++)
            {
                this.subSvtList[j].setTarget(uniqueId);
            }
            this.subSelect_uniqueId = uniqueId;
        }
        if ((0 < this.mainSelect_uniqueId) && (0 < this.subSelect_uniqueId))
        {
            this.actButton.isEnabled = true;
            this.actButton.SetState(UIButtonColor.State.Normal, true);
        }
        else
        {
            this.actButton.isEnabled = false;
            this.actButton.SetState(UIButtonColor.State.Disabled, true);
        }
    }

    public void SetCallBack(SelectedCallBack callback)
    {
        this.callBack = callback;
    }

    public override void setInitialPos()
    {
        base.gameObject.transform.localPosition = Vector3.zero;
        base.setInitialPos();
    }

    public void SetServantData(BattleServantData[] mainList, BattleServantData[] subList)
    {
        this.title_label.text = LocalizationManager.Get("BATTLE_DIALOG_SELECTMAINSUB_SERVANT");
        for (int i = 0; i < this.mainSvtList.Length; i++)
        {
            if (i < mainList.Length)
            {
                this.mainSvtList[i].setData(mainList[i], POSITION.MAIN, i, new BattleSelectServantComponent.CallBack(this.selectSvt));
            }
            else
            {
                this.mainSvtList[i].setNone();
            }
        }
        for (int j = 0; j < this.subSvtList.Length; j++)
        {
            if (j < subList.Length)
            {
                this.subSvtList[j].setData(subList[j], POSITION.SUB, j + 3, new BattleSelectServantComponent.CallBack(this.selectSvt));
            }
            else
            {
                this.subSvtList[j].setNone();
            }
        }
    }

    public enum POSITION
    {
        MAIN = 1,
        SUB = 2
    }

    public delegate void SelectedCallBack(bool flg, int mainUniqueId, int subUniqueId);
}

