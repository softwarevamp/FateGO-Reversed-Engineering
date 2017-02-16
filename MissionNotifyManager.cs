using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionNotifyManager : SingletonTemplate<MissionNotifyManager>
{
    private List<MissionNotifyDispInfo> mDispInfos = new List<MissionNotifyDispInfo>();
    private List<MissionNotifyComponent> mMissionNotifyComps = new List<MissionNotifyComponent>();
    private NoticeNumberComponent mNoticeNumberComp;
    private int mPauseCount;
    private const float NOTICE_NUMBER_SCALE = 0.75f;
    private const int NOTICE_NUMBER_X = 270;
    private const int NOTICE_NUMBER_Y = 0x113;

    public void CancelPause()
    {
        this.mPauseCount = 0;
        this.StartDisp();
        if (this.mNoticeNumberComp != null)
        {
            this.mNoticeNumberComp.SetDisp(true);
        }
    }

    public void ClearRequest()
    {
        this.mDispInfos.Clear();
        if (this.mNoticeNumberComp != null)
        {
            this.mNoticeNumberComp.SetNumber(this.GetDispInfoCount());
        }
    }

    public void Destroy()
    {
        this.ClearRequest();
        this.DestroyAllMissionNotifyComponentObject();
        if (this.mNoticeNumberComp != null)
        {
            UnityEngine.Object.Destroy(this.mNoticeNumberComp.gameObject);
            this.mNoticeNumberComp = null;
        }
    }

    private void DestroyAllMissionNotifyComponentObject()
    {
        for (int i = this.mMissionNotifyComps.Count - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(this.mMissionNotifyComps[i].gameObject);
            this.mMissionNotifyComps.RemoveAt(i);
        }
    }

    private void DragStartCallback()
    {
        if (this.mNoticeNumberComp != null)
        {
            this.mNoticeNumberComp.SetDisp(false);
        }
    }

    public void EndPause()
    {
        if (this.mPauseCount != 0)
        {
            this.mPauseCount--;
            if (this.mPauseCount == 0)
            {
                this.CancelPause();
            }
        }
    }

    private void FrameOutEndCallback(MissionNotifyComponent comp)
    {
        if (this.mMissionNotifyComps.Count >= 1)
        {
            this.mMissionNotifyComps.Remove(comp);
            UnityEngine.Object.Destroy(comp.gameObject);
        }
    }

    private MissionNotifyComponent FrameOutStartCallback() => 
        this.StartDisp();

    public int GetDispInfoCount() => 
        this.mDispInfos.Count;

    public void Init()
    {
        this.Destroy();
        this.mPauseCount = 0;
    }

    private bool IsBusy()
    {
        if (this.IsPause())
        {
            return true;
        }
        foreach (MissionNotifyComponent component in this.mMissionNotifyComps)
        {
            if (component.IsBusy())
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPause() => 
        (this.mPauseCount > 0);

    public void Reboot()
    {
        this.Init();
    }

    public void RequestDisp(MissionNotifyDispInfo disp_info)
    {
        this.mDispInfos.Add(disp_info);
        if (this.mNoticeNumberComp != null)
        {
            this.mNoticeNumberComp.SetNumber(this.GetDispInfoCount());
        }
        this.StartDisp();
    }

    private MissionNotifyComponent StartDisp()
    {
        if (this.mNoticeNumberComp != null)
        {
            this.mNoticeNumberComp.SetDisp(!this.IsPause());
        }
        if (this.IsBusy())
        {
            return null;
        }
        if (this.mDispInfos.Count <= 0)
        {
            return null;
        }
        GameObject self = SingletonMonoBehaviour<CommonUI>.Instance.CreateMissionNotify();
        if (self == null)
        {
            return null;
        }
        MissionNotifyDispInfo info = this.mDispInfos[0];
        MissionNotifyComponent item = self.GetComponent<MissionNotifyComponent>();
        item.SetupAndPlay(info, new System.Action(this.DragStartCallback), new Func<MissionNotifyComponent>(this.FrameOutStartCallback), new Action<MissionNotifyComponent>(this.FrameOutEndCallback));
        if (this.mNoticeNumberComp == null)
        {
            NoticeNumberComponent component2 = UnityEngine.Object.Instantiate<NoticeNumberComponent>(item.NoticeNumberPrefab);
            GameObject gameObject = item.gameObject.GetParent().gameObject;
            self = component2.gameObject;
            self.SafeSetParent(gameObject);
            self.SetLocalPosition(new Vector2(270f, 275f));
            self.SetLocalScale((float) 0.75f);
            int depth = gameObject.GetComponentInChildren<UIWidget>().depth;
            foreach (UIWidget widget in self.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += depth;
            }
            this.mNoticeNumberComp = component2;
        }
        this.mMissionNotifyComps.Add(item);
        this.mDispInfos.RemoveAt(0);
        this.mNoticeNumberComp.SetNumber(this.GetDispInfoCount());
        return item;
    }

    public void StartPause()
    {
        this.mPauseCount++;
    }
}

