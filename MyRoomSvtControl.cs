using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyRoomSvtControl : BaseMonoBehaviour
{
    private string asstName;
    private int beforeVoiceIdx;
    private System.Action callbackStopVoice;
    private System.Action callbakEndPlay;
    public PlayMakerFSM fsm;
    private int maxPlayCnt;
    private int playCnt;
    private SePlayer player;
    private ServantVoiceData[] randomVoiceList;
    private UIStandFigureR svtFigure;
    private int svtId;
    public Coroutine Talkdata;
    public float vcdelay;
    private string vcName;
    public string vctext;
    private List<ServantVoiceData[]> voiceList;
    private float volume = 1f;

    private void EndPlay()
    {
        if (this.playCnt < this.maxPlayCnt)
        {
            float delay = this.randomVoiceList[this.playCnt].delay;
            base.Invoke("svtVoicePlay", delay);
        }
        else
        {
            SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(this.Talkdata);
            this.Talkdata = null;
            this.vctext = string.Empty;
            if (this.player != null)
            {
                this.player = null;
            }
            this.playCnt = 0;
            this.fsm.SendEvent("END_PLAY");
        }
    }

    public string playVoice(System.Action callback = null)
    {
        if (this.player != null)
        {
            this.stopVoice();
        }
        if (this.voiceList == null)
        {
            return null;
        }
        this.callbackStopVoice = callback;
        int num = new System.Random().Next(this.voiceList.Count);
        if (this.beforeVoiceIdx == num)
        {
            num++;
            num = num % this.voiceList.Count;
        }
        this.randomVoiceList = this.voiceList[num];
        this.beforeVoiceIdx = num;
        this.maxPlayCnt = this.randomVoiceList.Length;
        return this.svtVoicePlay();
    }

    public string playVoice(int num = 0)
    {
        if ((this.voiceList == null) || (this.voiceList.Count == 0))
        {
            return null;
        }
        int num2 = new System.Random().Next(this.voiceList.Count);
        if (this.beforeVoiceIdx == num2)
        {
            num2++;
            num2 = num2 % this.voiceList.Count;
        }
        this.randomVoiceList = this.voiceList[num2];
        this.beforeVoiceIdx = num2;
        this.maxPlayCnt = this.randomVoiceList.Length;
        return this.svtVoicePlay();
    }

    public void playVoice(string voiceID)
    {
        this.randomVoiceList = null;
        foreach (ServantVoiceData[] dataArray in this.voiceList)
        {
            foreach (ServantVoiceData data in dataArray)
            {
                if (data.id == voiceID)
                {
                    this.randomVoiceList = dataArray;
                    break;
                }
            }
            if (this.randomVoiceList != null)
            {
                break;
            }
        }
        if (this.randomVoiceList != null)
        {
            this.maxPlayCnt = this.randomVoiceList.Length;
            this.svtVoicePlay();
        }
    }

    public void setFigure(UIStandFigureR targetFigure)
    {
        this.svtFigure = targetFigure;
    }

    public void setSvtVoice(List<ServantVoiceData[]> list, string assetName)
    {
        this.playCnt = 0;
        this.asstName = assetName;
        if (list != null)
        {
            this.voiceList = list;
            this.beforeVoiceIdx = -1;
        }
    }

    public void stopVoice()
    {
        if (this.player != null)
        {
            SoundManager.stopVoice(this.asstName, this.vcName, 0f);
            this.player = null;
            this.playCnt = 0;
            this.maxPlayCnt = 0;
        }
    }

    private string svtVoicePlay()
    {
        if ((this.asstName == null) || (this.maxPlayCnt == 0))
        {
            return null;
        }
        if (this.randomVoiceList.Length <= this.playCnt)
        {
            return null;
        }
        this.vctext = string.Empty;
        this.vcName = this.randomVoiceList[this.playCnt].id;
        this.vctext = this.randomVoiceList[this.playCnt].text;
        this.vcdelay = this.randomVoiceList[this.playCnt].delay;
        for (int i = 0; i < this.randomVoiceList.Length; i++)
        {
            if ((string.Compare(this.vcName, this.randomVoiceList[i].id) != 0) && (this.vcName.Substring(0, 4) == this.randomVoiceList[i].id.Substring(0, 4)))
            {
                this.vctext = this.vctext + this.randomVoiceList[i].text;
            }
        }
        if (this.Talkdata == null)
        {
            this.Talkdata = base.StartCoroutine(SingletonMonoBehaviour<ScriptManager>.Instance.settalk((int) MyRoomControl.getState(), this.vctext, this.vcdelay));
        }
        int face = this.randomVoiceList[this.playCnt].face;
        Debug.Log("!! ** !! Get testVoiceDataList face: " + face);
        this.svtFigure.SetFace((Face.Type) face);
        Debug.Log("!! ** !! vcName: " + this.vcName);
        this.player = SoundManager.playVoice(this.asstName, this.vcName, this.volume, new System.Action(this.EndPlay));
        this.playCnt++;
        return this.vcName;
    }
}

