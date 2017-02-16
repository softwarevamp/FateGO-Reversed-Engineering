using System;
using UnityEngine;

public class MaterialCollectionComponent : MonoBehaviour
{
    [SerializeField]
    protected MaterialCollectionMenu materialCollectionMenu;
    [SerializeField]
    protected PlayMakerFSM myRoomFsm;

    public bool closeMenu()
    {
        this.materialCollectionMenu.Close(new System.Action(this.onClose));
        return true;
    }

    public void hideMenu()
    {
        this.materialCollectionMenu.Init();
        base.gameObject.SetActive(false);
    }

    protected void onClose()
    {
        this.myRoomFsm.SendEvent("CLOSE_MENU");
    }

    protected void onCloseWebView()
    {
        this.myRoomFsm.SendEvent("CLOSE_MATERIAL");
    }

    protected void onEndMenu()
    {
        this.myRoomFsm.SendEvent("CLOSE_MATERIAL");
    }

    public bool openMenu()
    {
        base.gameObject.SetActive(true);
        this.materialCollectionMenu.Open(delegate {
            this.myRoomFsm.SendEvent("GO_NEXT");
        }, new MaterialCollectionMenu.CallbackFunc(this.onEndMenu));
        return true;
    }

    public void showMenu()
    {
        base.gameObject.SetActive(true);
        this.materialCollectionMenu.Init();
    }
}

