using System;
using UnityEngine;

public class SetGameOptionComponent : MonoBehaviour
{
    public FullDownloadControl fullDlCtr;
    public PlayMakerFSM myRoomFsm;
    public UIScrollView optionScroll;
    public SetNoticeOptionControl setNoticeCtr;
    public SetVolumeControl setVolCtr;

    public void hideGameOption()
    {
        base.gameObject.SetActive(false);
        this.optionScroll.SetDragAmount(0f, 0f, false);
    }

    public void reflectionGameOption()
    {
        this.setVolCtr.reflectionVolume();
        this.setNoticeCtr.reflectionNotice();
        SoundManager.stopSe(0f);
        OptionManager.SaveData();
    }

    public void showGameOption()
    {
        this.setVolCtr.initSetVolume();
        this.setNoticeCtr.initSetNotice();
        this.fullDlCtr.Init();
        base.gameObject.SetActive(true);
    }

    private void Start()
    {
    }
}

