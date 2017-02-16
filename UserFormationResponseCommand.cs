public class UserFormationResponseCommand : ResponseCommandBase
{
    public override ResponseCommandBase.Result ExecuteResponse(ResponseData data)
    {
        if (data.checkError() && (data.success != null))
        {
            return ResponseCommandBase.Result.SUCCESS;
        }
        return ResponseCommandBase.Result.ERROR;
    }

    public override ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.USER_FORMATION;
}

