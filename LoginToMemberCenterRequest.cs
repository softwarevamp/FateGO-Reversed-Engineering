using System;
using System.Collections.Generic;
using UnityEngine;

public class LoginToMemberCenterRequest : RequestBase
{
    public override string getMockData() => 
        base.getMockData();

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "rgfate/60_member/logintomembercenter.php");

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.LOGIN_TO_MEMBERCENTER, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                if (success.ContainsKey("dataVer"))
                {
                    string s = success["dataVer"].ToString();
                    string str2 = "0";
                    if (success.ContainsKey("dateVer"))
                    {
                        str2 = success["dateVer"].ToString();
                    }
                    SingletonMonoBehaviour<DataManager>.Instance.setMasterData(int.Parse(s), long.Parse(str2));
                }
                if ((PlayerPrefs.GetString("rguid", "0") == success["rguid"].ToString()) || (PlayerPrefs.GetString("rguid", "0") == "0"))
                {
                    PlayerPrefs.SetString("rguid", success["rguid"].ToString());
                }
                else
                {
                    TerminalPramsManager.PhaseCnt = 0;
                    string str3 = PlayerPrefs.GetString("Asset", "offline");
                    PlayerPrefs.DeleteAll();
                    BattleData.deleteSaveData();
                    PlayerPrefs.SetString("Asset", str3);
                    PlayerPrefs.Save();
                    NetworkManager.DeleteSaveData();
                    UserServantLockManager.DeleteSaveData();
                    UserServantNewManager.DeleteSaveData();
                    UserServantCollectionManager.DeleteSaveData();
                    ServantCommentManager.DeleteSaveData();
                    UserEquipNewManager.DeleteSaveData();
                    OtherUserNewManager.DeleteSaveData();
                    PlayerPrefs.SetString("rguid", success["rguid"].ToString());
                    PlayerPrefs.SetString("usk", data.usk);
                    PlayerPrefs.SetString("rgusk", success["rgusk"].ToString());
                }
                base.completed("ok");
                GameObject.Find("TitleScene").GetComponent<TitleRootComponent>().SetBoxCLient(true);
                return;
            }
        }
        base.completed("ng");
    }
}

