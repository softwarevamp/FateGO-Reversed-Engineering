using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SetMoveBannerControll : BaseMonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<EventEntity> <>f__am$cacheE;
    public UIGrid bannerGrid;
    private GameObject[] bannerList;
    public UIPanel bannerPanel;
    public GameObject bannerPb;
    public PlayMakerFSM fsm;
    private int idx;
    private bool isClickLf;
    private bool isClickRh;
    public UIButton lfBtn;
    private float moveLeftPos;
    private float moveRightPos;
    private float moveSpeed = 0.2f;
    public UIButton rgBtn;
    private TweenPosition tp;

    private void autoMoveLeft()
    {
        GameObject target = this.bannerList[this.idx];
        Vector3 localPosition = target.transform.localPosition;
        localPosition.x += this.moveLeftPos;
        this.moveBanner(target, localPosition);
    }

    private void buildMoveBanner()
    {
        if (this.isClickLf)
        {
            this.OnMoveLeft();
        }
        if (this.isClickRh)
        {
            this.OnMoveRight();
        }
        this.setEnabledBtn();
    }

    private void initEnableBtn(int cnt)
    {
        if (cnt <= 1)
        {
            this.lfBtn.gameObject.SetActive(false);
            this.rgBtn.gameObject.SetActive(false);
        }
    }

    public void initSetBanner(int[] bannerIds)
    {
    }

    private void moveBanner(GameObject target, Vector3 pos)
    {
        EventDelegate.Set(this.tp.onFinished, new EventDelegate.Callback(this.OnMoveFinish));
        this.tp = target.GetComponent<TweenPosition>();
        this.tp.enabled = true;
        TweenPosition.Begin(target, this.moveSpeed, pos);
        this.idx++;
    }

    public void OnClickLeftBtn()
    {
        this.isClickLf = true;
        this.buildMoveBanner();
    }

    public void OnClickRightBtn()
    {
        this.isClickRh = true;
        this.buildMoveBanner();
    }

    private void OnMoveFinish()
    {
        if (!this.lfBtn.isEnabled)
        {
            this.lfBtn.enabled = true;
        }
        if (!this.rgBtn.isEnabled)
        {
            this.rgBtn.enabled = true;
        }
    }

    private void OnMoveLeft()
    {
        this.rePositionLeft();
        for (int i = 0; i < this.bannerList.Length; i++)
        {
            GameObject target = this.bannerList[i];
            Vector3 localPosition = target.transform.localPosition;
            localPosition.x += this.moveLeftPos;
            this.moveBanner(target, localPosition);
        }
        this.isClickLf = false;
    }

    private void OnMoveRight()
    {
        this.rePositionRight();
        for (int i = 0; i < this.bannerList.Length; i++)
        {
            GameObject target = this.bannerList[i];
            Vector3 localPosition = target.transform.localPosition;
            localPosition.x += this.moveRightPos;
            this.moveBanner(target, localPosition);
        }
        this.isClickRh = false;
    }

    private void rePositionLeft()
    {
        for (int i = 0; i < this.bannerList.Length; i++)
        {
            Vector3 localPosition = this.bannerList[i].transform.localPosition;
            float num2 = this.bannerList.Length - 1f;
            if (localPosition.x == (this.moveLeftPos * num2))
            {
                Debug.Log("****** RePositionLeft");
                GameObject obj2 = this.bannerList[i];
                obj2.transform.localPosition = new Vector3(this.moveRightPos, 0f, 0f);
                this.tp = obj2.GetComponent<TweenPosition>();
                this.tp.from = obj2.transform.localPosition;
                this.tp.enabled = false;
            }
        }
    }

    private void rePositionRight()
    {
        for (int i = 0; i < this.bannerList.Length; i++)
        {
            Vector3 localPosition = this.bannerList[i].transform.localPosition;
            float num2 = this.bannerList.Length - 1f;
            if (localPosition.x == (this.moveRightPos * num2))
            {
                GameObject obj2 = this.bannerList[i];
                obj2.transform.localPosition = new Vector3(this.moveLeftPos, 0f, 0f);
                this.tp = obj2.GetComponent<TweenPosition>();
                this.tp.from = obj2.transform.localPosition;
                this.tp.enabled = false;
            }
        }
    }

    public void setBanner(List<EventEntity> eventDataList)
    {
        float width = 0f;
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = (a, b) => b.getEventBannerPriority() - a.getEventBannerPriority();
        }
        eventDataList.Sort(<>f__am$cacheE);
        int count = eventDataList.Count;
        this.bannerList = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            GameObject obj2 = base.createObject(this.bannerPb, this.bannerPanel.transform, null);
            if ((i % 2) == 1)
            {
                obj2.GetComponent<UISprite>().spriteName = "banner_event_002";
            }
            width = obj2.GetComponent<UISprite>().width;
            Vector3 localPosition = this.bannerPanel.transform.localPosition;
            localPosition.x += i * width;
            obj2.transform.localPosition = localPosition;
            obj2.transform.localScale = Vector3.one;
            this.tp = obj2.GetComponent<TweenPosition>();
            this.tp.enabled = false;
            this.tp.from = obj2.transform.localPosition;
            this.bannerList[i] = obj2;
        }
        this.moveLeftPos = -width;
        this.moveRightPos = width;
        this.initEnableBtn(count);
    }

    private void setEnabledBtn()
    {
        this.lfBtn.enabled = false;
        this.rgBtn.enabled = false;
    }
}

