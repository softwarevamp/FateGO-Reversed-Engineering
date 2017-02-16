using System;
using System.Collections.Generic;
using UnityEngine;

public class ServantCombineResultInfoComponent : BaseDialog
{
    [SerializeField]
    protected UILabel befLevelLb;
    protected Vector3 center;
    [SerializeField]
    protected UILabel currentAtkLb;
    [SerializeField]
    protected UILabel currentHpLb;
    [SerializeField]
    protected UILabel currentLevelLb;
    [SerializeField]
    protected UILabel increAtkLb;
    [SerializeField]
    protected UILabel increHpLb;
    [SerializeField]
    protected UILabel levelUpTitleLb;
    [SerializeField]
    protected GameObject levleUpInfo;
    protected System.Action openCallBack;
    protected List<GameObject> resInfoList;
    [SerializeField]
    protected UIGrid resultInfoGrid;
    protected State state;

    protected void clearInfoGrid()
    {
        int childCount = this.resultInfoGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.resultInfoGrid.transform.GetChild(i).gameObject);
            }
            this.resInfoList = null;
        }
    }

    public void Close()
    {
        this.Close(new System.Action(this.EndClose));
    }

    public void Close(System.Action callback)
    {
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
    }

    protected void EndOpen()
    {
        if (this.openCallBack != null)
        {
            System.Action openCallBack = this.openCallBack;
            this.openCallBack = null;
            openCallBack();
        }
    }

    public void Init()
    {
        base.gameObject.SetActive(false);
        this.clearInfoGrid();
        this.levleUpInfo.SetActive(false);
        this.befLevelLb.text = string.Empty;
        this.currentLevelLb.text = string.Empty;
        this.currentHpLb.text = string.Empty;
        this.increHpLb.text = string.Empty;
        this.currentAtkLb.text = string.Empty;
        this.increAtkLb.text = string.Empty;
        this.state = State.INIT;
        base.Init();
    }

    public void OpenLevelUpInfo(LevelUpInfoData infoData, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            base.gameObject.SetActive(true);
            this.levleUpInfo.SetActive(true);
            this.openCallBack = callback;
            this.levelUpTitleLb.text = string.Empty;
            if (infoData.oldLv < infoData.currentLv)
            {
                this.levelUpTitleLb.text = LocalizationManager.Get("LEVELUP_NOTICE_TITLE");
                SoundManager.playSystemSe(SeManager.SystemSeKind.LEVEL_UP);
            }
            this.befLevelLb.text = string.Format(LocalizationManager.Get("LEVEL_INFO"), infoData.oldLv);
            this.currentLevelLb.text = infoData.currentLv.ToString();
            this.currentHpLb.text = infoData.currentHp.ToString();
            this.increHpLb.text = string.Format(LocalizationManager.Get("INCREMENT_SVTLEVEL"), infoData.increHpVal);
            this.currentAtkLb.text = infoData.currentAtk.ToString();
            this.increAtkLb.text = string.Format(LocalizationManager.Get("INCREMENT_SVTLEVEL"), infoData.increAtkVal);
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    public void OpenResultInfo(List<GameObject> resInfo, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.STATUS_OPEN);
            base.gameObject.SetActive(true);
            this.resInfoList = resInfo;
            this.openCallBack = callback;
            this.setCenter();
            float num = this.resultInfoGrid.cellHeight * 0.5f;
            float x = this.resultInfoGrid.transform.localPosition.x;
            int count = this.resInfoList.Count;
            for (int i = 0; i < count; i++)
            {
                base.createObject(this.resInfoList[i], this.resultInfoGrid.transform, null).SetActive(true);
            }
            float num5 = 25f;
            float y = (num * this.resInfoList.Count) - num5;
            this.resultInfoGrid.transform.localPosition = new Vector3(x, y, this.center.z);
            this.resultInfoGrid.repositionNow = true;
            base.Open(new System.Action(this.EndOpen), true);
        }
    }

    protected void setCenter()
    {
        UIPanel basePanel = base.basePanel;
        if (((basePanel == null) && (base.basePanelList != null)) && (base.basePanelList.Length > 0))
        {
            basePanel = base.basePanelList[0];
        }
        Vector3[] worldCorners = basePanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = basePanel.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        Debug.Log("**!! Center: " + this.center);
    }

    protected enum State
    {
        INIT
    }
}

