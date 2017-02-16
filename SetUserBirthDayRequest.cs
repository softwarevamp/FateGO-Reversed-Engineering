using System;

public class SetUserBirthDayRequest : RequestBase
{
    public void beginRequest(string normalName, int genderType, int month, int day)
    {
        SingletonMonoBehaviour<NetworkManager>.Instance.SetSignup(normalName, genderType, month, day);
        DateTime dateTime = new DateTime(0x7d0, month, day, 0, 0, 0, DateTimeKind.Utc);
        long data = NetworkManager.getTime(dateTime);
        base.addActionField("profileeditbirth");
        base.addField("birthDay", data);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.SET_BIRTHDAY, responseList);
        if ((data != null) && data.checkError())
        {
            UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
            DateTime time = NetworkManager.getDateTime(entity.birthDay);
            SingletonMonoBehaviour<NetworkManager>.Instance.SetSignup(entity.name, entity.genderType, time.Month, time.Day);
            SingletonMonoBehaviour<NetworkManager>.Instance.WriteSignup();
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

