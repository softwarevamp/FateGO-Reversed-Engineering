using System;
using UnityEngine;

public class ServantStatusCharaGraphListViewIndicator : ListViewIndicator
{
    [SerializeField]
    protected GameObject leftObject;
    protected ListViewManager manager;
    [SerializeField]
    protected GameObject pageBaseObject;
    protected int pageIndex;
    protected int pageMax;
    [SerializeField]
    protected UISprite[] pageSpriteList = new UISprite[4];
    [SerializeField]
    protected GameObject rightObject;

    public int GetPageIndex() => 
        this.pageIndex;

    public void OnClickLeft()
    {
        if ((this.manager != null) && (this.pageIndex > 0))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.manager.MoveCenterItem(this.pageIndex - 1, true);
        }
    }

    public void OnClickRight()
    {
        if ((this.manager != null) && ((this.pageIndex >= 0) && (this.pageIndex < (this.pageMax - 1))))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
            this.manager.MoveCenterItem(this.pageIndex + 1, true);
        }
    }

    public override void OnModifyCenterItem(ListViewManager manager, ListViewItem item, bool isTop, bool isBottom, bool isLeft, bool isRight)
    {
        this.manager = manager;
        this.leftObject.SetActive(isLeft);
        this.rightObject.SetActive(isRight);
        this.SetPageIndex((item == null) ? -1 : item.Index);
    }

    public override void OnModifyPosition(ListViewManager manager, ListViewItem item)
    {
        bool flag;
        bool flag2;
        bool flag3;
        bool flag4;
        manager.GetCanScrollList(out flag, out flag2, out flag3, out flag4);
        this.leftObject.SetActive(flag3);
        this.rightObject.SetActive(flag4);
    }

    public override void SetIndexMax(int max)
    {
        this.SetPageMax(max);
        this.leftObject.SetActive(false);
        this.rightObject.SetActive(false);
    }

    public void SetPageIndex(int index)
    {
        this.pageIndex = index;
        for (int i = 0; i < this.pageMax; i++)
        {
            this.pageSpriteList[i].spriteName = (i != index) ? "img_slider_off" : "img_slider_on";
        }
    }

    public void SetPageMax(int max)
    {
        this.pageMax = (max <= this.pageSpriteList.Length) ? max : this.pageSpriteList.Length;
        for (int i = 0; i < this.pageSpriteList.Length; i++)
        {
            this.pageSpriteList[i].spriteName = (i >= this.pageMax) ? null : "img_slider_off";
        }
        Vector3 localPosition = this.pageBaseObject.transform.localPosition;
        localPosition.x = -10 * (this.pageMax - 1);
        this.pageBaseObject.transform.localPosition = localPosition;
    }
}

