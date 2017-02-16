using System;
using System.Collections.Generic;

public class BattleResultRequest : RequestBase
{
    public void beginRequest(long battleId, int battleResult, string scores, string action, int[][] voicePlayedArray, int battleResultType)
    {
        base.addField("battleId", battleId);
        base.addField("battleResult", battleResult);
        base.addField("scores", scores);
        base.addField("action", action);
        base.addField("battleMode", battleResultType);
        if (voicePlayedArray != null)
        {
            string str = string.Empty;
            for (int i = 0; i < voicePlayedArray.Length; i++)
            {
                int[] numArray = voicePlayedArray[i];
                if ((numArray != null) && (numArray.Length == 2))
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        object[] objArray1 = new object[] { "[", numArray[0], ",", numArray[1], "]" };
                        str = string.Concat(objArray1);
                    }
                    else
                    {
                        string str2 = str;
                        object[] objArray2 = new object[] { str2, ",[", numArray[0], ",", numArray[1], "]" };
                        str = string.Concat(objArray2);
                    }
                }
            }
            base.addField("voicePlayedList", "[" + str + "]");
        }
        base.addActionField("battleresult");
        base.beginRequest();
    }

    public void debugPrint()
    {
        foreach (KeyValuePair<string, string> pair in base.paramString)
        {
            Debug.Log($"Key : {pair.Key} / Value : {pair.Value}");
        }
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockBattleResultRequest");

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        Debug.Log(">>>>>>>>>>>>>>>>>>BattleResultRequest::requestCompleted >:" + responseList.Length);
        TopHomeRequest.clearExpirationDate();
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.BATTLE_RESULT, responseList);
        if ((data != null) && data.checkError())
        {
            Debug.Log("responseData ok");
            Dictionary<string, object> success = data.success;
            if (success != null)
            {
                Debug.Log("successList ok");
                base.completed(JsonManager.toJson(success));
                return;
            }
        }
        base.completed("ng");
    }
}

