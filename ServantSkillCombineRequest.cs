using System;

public class ServantSkillCombineRequest : RequestBase
{
    public void beginRequest(long baseUsrSvtId, int selectSkillIndex, int selectSkillId)
    {
        base.addActionField("cardcombineskill");
        base.addField("baseUserSvtId", baseUsrSvtId);
        base.addField("num", selectSkillIndex);
        base.addField("skillId", selectSkillId);
        base.beginRequest();
    }

    public override string getURL() => 
        NetworkManager.getActionUrl(false);

    public override void requestCompleted(ResponseData[] responseList)
    {
        ResponseData data = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.CARD_COMBINE_SKILL, responseList);
        if ((data != null) && data.checkError())
        {
            base.completed(JsonManager.toJson(data.success));
        }
        else
        {
            base.completed("ng");
        }
    }
}

