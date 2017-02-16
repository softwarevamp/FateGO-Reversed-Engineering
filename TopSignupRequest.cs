using System;
using UnityEngine;

public class TopSignupRequest : RequestBase
{
    public void beginRequest(string action)
    {
        base.addActionField("signuptop");
        base.addField("nickname", PlayerPrefs.GetString("nickname"));
        base.addField("sgtype", PlayerPrefs.GetString("sgtype"));
        base.addField("sgtag", PlayerPrefs.GetString("sgtag"));
        base.beginRequest();
    }

    public override string getMockData() => 
        string.Empty;

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SIGNUP, responseList);
        if ((data != null) && data.checkError())
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            if (entity != null)
            {
                if (entity.tutorialProgress != -1)
                {
                    PlayerPrefs.SetInt(TutorialFlag.SAVE_KEY1, entity.tutorialProgress);
                }
                else
                {
                    PlayerPrefs.DeleteKey(TutorialFlag.SAVE_KEY1);
                    PlayerPrefs.Save();
                }
                UserServantNewManager.LoginProcess();
                UserServantCollectionManager.LoginProcess();
                ServantCommentManager.LoginProcess();
                UserEquipNewManager.LoginProcess();
                base.completed("ok");
                return;
            }
        }
        base.completed("ng");
    }
}

