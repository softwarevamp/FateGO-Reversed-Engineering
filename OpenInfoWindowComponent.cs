using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenInfoWindowComponent : BaseDialog
{
    protected Vector3 center;
    protected System.Action openCallBack;
    protected List<GameObject> resInfoList;
    [SerializeField]
    protected UIGrid resultInfoGrid;

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
        base.Init();
    }

    public void OpenResultInfo(List<GameObject> resInfo, System.Action callback)
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
        this.resultInfoGrid.transform.localPosition = new Vector3(x, y, 0f);
        this.resultInfoGrid.repositionNow = true;
        base.Open(new System.Action(this.EndOpen), true);
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
    }
}

