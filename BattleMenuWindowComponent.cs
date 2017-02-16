using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleMenuWindowComponent : BattleWindowComponent
{
    public UILabel checkGiveUpLabel;
    public BattleWindowComponent checkRetireWindow;
    public BattleData data;
    public BattleViewItemlistComponent itemWindow;
    public PlayMakerFSM myFsm;
    public GameObject reserveitemlist_object;
    public GameObject RetireButton;

    public void Close(BattleWindowComponent.EndCall call = null)
    {
        this.itemWindow.setHide();
        base.Close(call);
    }

    public void CompClose()
    {
        base.CompClose();
    }

    public void CompOpen()
    {
        this.itemWindow.setShow();
        base.CompOpen();
    }

    public void endCloseCkRetire()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void endOpenCkRetire()
    {
        this.myFsm.SendEvent("END_PROC");
    }

    public void modeCkRetire()
    {
        this.checkRetireWindow.Close(new BattleWindowComponent.EndCall(this.endCloseCkRetire));
    }

    public void modeRetire()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.checkRetireWindow.Open(new BattleWindowComponent.EndCall(this.endOpenCkRetire));
    }

    public void Open(BattleWindowComponent.EndCall call = null)
    {
        this.checkGiveUpLabel.text = LocalizationManager.Get("BATTLE_RETIRE_CHECKSTR");
        this.modeCkRetire();
        this.itemWindow.setListData(this.data.getDropItems(), new BattleDropItemComponent.ClickDelegate(this.showConf), 200);
        this.itemWindow.setHide();
        this.checkRetireWindow.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.2f, false);
        this.checkRetireWindow.setClose();
        if (this.data.isTutorial())
        {
            this.RetireButton.GetComponent<UISprite>().color = Color.gray;
            this.RetireButton.GetComponent<Collider>().enabled = false;
        }
        else
        {
            this.RetireButton.GetComponent<UISprite>().color = Color.white;
            this.RetireButton.GetComponent<Collider>().enabled = true;
        }
        base.Open(call);
    }

    public void showConf(BattleDropItem drop)
    {
    }
}

