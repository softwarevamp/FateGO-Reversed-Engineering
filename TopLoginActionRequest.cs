using System;
using System.Collections.Generic;
using UnityEngine;

public class TopLoginActionRequest : RequestBase
{
    public void beginRequest(string action)
    {
        base.addActionField("toplogin");
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
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.LOGIN, responseList);
        if (data != null)
        {
            if (data.checkError())
            {
                UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                Dictionary<string, object> success = data.success;
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
                    base.completed(JsonManager.toJson(data.success));
                }
            }
            else
            {
                base.completed(data.resCode);
            }
        }
        else
        {
            base.completed("ng");
        }
    }
}

