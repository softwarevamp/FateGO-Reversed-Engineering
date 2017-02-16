using System;

public class TopMyRoomRequest : RequestBase
{
    public void beginRequest(int[][] voicePlayedList)
    {
        string str = string.Empty;
        for (int i = 0; i < voicePlayedList.Length; i++)
        {
            int[] numArray = voicePlayedList[i];
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
        Debug.Log("TopMyRoomRequest " + str);
        base.addActionField("cardvoice");
        base.addField("voicePlayedList", "[" + str + "]");
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.HOME, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed("ok");
        }
        else
        {
            base.completed("ng");
        }
    }
}

