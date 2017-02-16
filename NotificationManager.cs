using System;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : SingletonMonoBehaviour<NotificationManager>
{
    protected static bool isSend;
    private AndroidJavaClass m_ANObj;
    public Text Text_Message;

    public void Initialize()
    {
    }

    private bool InitNotificator()
    {
        if (this.m_ANObj == null)
        {
            try
            {
                this.m_ANObj = new AndroidJavaClass("com.guyastudio.unityplugins.AndroidNotificator");
            }
            catch
            {
                Debug.LogError(" ");
                return false;
            }
        }
        if (this.m_ANObj == null)
        {
            return false;
        }
        return true;
    }

    protected void OnApplicationPause(bool isPause)
    {
        Debug.Log(string.Concat(new object[] { "NotificationManager::OnApplicationPasue NEW [", isPause, "] ", isSend }));
        if (isPause)
        {
            if (OptionManager.GetLocalNotiffication())
            {
                isSend = true;
                if (NetworkManager.IsLogin)
                {
                    UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
                    if (entity != null)
                    {
                        int message = (int) entity.getActAllRecoverTime();
                        if ((message > 0) && this.InitNotificator())
                        {
                            MonoBehaviour.print(message);
                            object[] args = new object[] { Application.productName, "温馨提示", LocalizationManager.Get("NOTIFICATION_AP_RECOVER_MESSAGE"), message, false };
                            this.m_ANObj.CallStatic("ShowNotification", args);
                        }
                    }
                }
            }
        }
        else if (isSend)
        {
            isSend = false;
        }
    }

    public void SetRemotePushState(bool isSend)
    {
    }
}

