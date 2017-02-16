using System;

public class MyRoomRootComponent : SceneRootComponent
{
    public MyRoomControl myRoomControl;

    public override void beginFinish()
    {
        Debug.Log("Call beginFinish");
        MyRoomControl control = (this.myRoomControl == null) ? base.gameObject.GetComponent<MyRoomControl>() : this.myRoomControl;
        control.quit();
        control.hideSerialCode();
        control.hideContinueDevice();
        control.hideMaterialCollection();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.MYROOM, 0x23);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        Debug.Log("Call beginInitialize");
    }

    public override void beginResume(object data)
    {
        this.myRoomControl.mBattleSetupInfo = data as BattleSetupInfo;
        if (this.myRoomControl.mBattleSetupInfo != null)
        {
            base.beginFinish();
            base.sendMessageStartUp();
            base.resumeMainMenuBar();
        }
        else
        {
            base.beginResume();
        }
    }

    public override void beginStartUp(object data)
    {
        MainMenuBar.setMenuActive(true, null);
        base.sendMessageStartUp();
        Debug.Log("Call beginStartUp");
    }
}

