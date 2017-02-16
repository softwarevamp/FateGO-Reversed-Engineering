using System;
using System.Collections.Generic;

public class FriendOfferRequest : RequestBase
{
    public void beginRequest(long targetUserId)
    {
        Debug.Log("FriendOfferRequest:beginRequest [" + targetUserId + "] ");
        base.addActionField("friendoffer");
        base.addField("targetUserId", targetUserId);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.FRIEND_OFFER, responseList);
        if ((data != null) && data.checkError())
        {
            Dictionary<string, object> success = data.success;
            string str = !success.ContainsKey("status") ? "0" : ((string) success["status"]);
            string title = !success.ContainsKey("title") ? string.Empty : ((string) success["title"]);
            string message = !success.ContainsKey("message") ? string.Empty : ((string) success["message"]);
            if (str.Equals("0"))
            {
                base.completed(JsonManager.toJson(data.success));
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, new NotificationDialog.ClickDelegate(this.requestErrorDialog), -1);
            }
        }
        else
        {
            base.completed("ng");
        }
    }

    public void requestErrorDialog(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        base.completed("ng");
    }
}

