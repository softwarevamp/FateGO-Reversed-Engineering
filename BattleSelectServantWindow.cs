using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleSelectServantWindow : BattleWindowComponent
{
    public Collider cnancelButton;
    public bool isSelected = true;
    public GameObject parentPanel;
    private SelectServantCallBack selectCallBack;
    public UITexture[] servantTop;
    private BattleServantData[] svtList;
    public UIGrid svtRoot;
    public UILabel title_label;
    public bool useClose = true;

    public override void Close(BattleWindowComponent.EndCall call = null)
    {
        this.parentPanel.SetActive(false);
        this.isSelected = true;
        base.Close(call);
    }

    public void onCloseButton()
    {
        if (this.useClose)
        {
            this.SelectServant(-1);
        }
    }

    public override void Open(BattleWindowComponent.EndCall call = null)
    {
        this.parentPanel.SetActive(true);
        this.isSelected = false;
        base.Open(call);
    }

    public void SelectA()
    {
        this.SelectServant(this.svtList[0].getUniqueID());
    }

    public void SelectB()
    {
        this.SelectServant(this.svtList[1].getUniqueID());
    }

    public void SelectC()
    {
        this.SelectServant(this.svtList[2].getUniqueID());
    }

    public void SelectServant(int uniqeId)
    {
        if (!this.isSelected)
        {
            if (0 < uniqeId)
            {
                SoundManager.playSe("ba19");
            }
            else
            {
                SoundManager.playSe("ba21");
            }
            this.useClose = true;
            if (this.selectCallBack != null)
            {
                this.selectCallBack(uniqeId);
            }
        }
    }

    public void SetCallBack(SelectServantCallBack callback)
    {
        this.selectCallBack = callback;
    }

    public override void setInitialPos()
    {
        base.gameObject.transform.localPosition = Vector3.zero;
        base.setInitialPos();
    }

    public void SetServantData(BattleServantData[] svtList)
    {
        this.svtList = svtList;
        this.title_label.text = LocalizationManager.Get("BATTLE_DIALOG_SELECT_SERVANT");
        for (int i = 0; i < this.servantTop.Length; i++)
        {
            if (this.servantTop[i] != null)
            {
                if (i < svtList.Length)
                {
                    this.servantTop[i].gameObject.SetActive(true);
                    BattleServantData data = svtList[i];
                    this.servantTop[i] = ServantAssetLoadManager.loadStatusFace(this.servantTop[i], data.getSvtId(), data.getDispLimitCount());
                    this.servantTop[i].gameObject.transform.parent = this.svtRoot.gameObject.transform;
                }
                else
                {
                    this.servantTop[i].gameObject.SetActive(false);
                    this.servantTop[i].gameObject.transform.parent = base.gameObject.transform;
                }
                this.servantTop[i].gameObject.transform.localScale = Vector3.one;
            }
        }
        this.svtRoot.Reposition();
    }

    public void setUseClose(bool flg)
    {
        this.useClose = flg;
        if (this.cnancelButton != null)
        {
            this.cnancelButton.enabled = flg;
        }
    }

    public delegate void SelectServantCallBack(int uniqueId);
}

