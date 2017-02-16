using System;
using UnityEngine;

public class SerialCodeComponent : MonoBehaviour
{
    [SerializeField]
    protected PlayMakerFSM myRoomFsm;

    public bool closeMenu()
    {
        this.myRoomFsm.SendEvent("CLOSE_MENU");
        return true;
    }

    public void hideMenu()
    {
        base.gameObject.SetActive(false);
    }

    protected void onCloseWebView()
    {
        this.myRoomFsm.SendEvent("CLOSE_SERIAL_CODE");
    }

    public bool openMenu()
    {
        base.gameObject.SetActive(true);
        WebViewManager.OpenViewDynamic(LocalizationManager.Get("WEB_VIEW_TITLE_SERIAL_CODE"), "SerialCodeTop", new System.Action(this.onCloseWebView));
        return true;
    }

    public void showMenu()
    {
        base.gameObject.SetActive(true);
    }
}

