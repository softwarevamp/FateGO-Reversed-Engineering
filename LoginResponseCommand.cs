public class LoginResponseCommand : ResponseCommandBase
{
    public override ResponseCommandBase.Result ExecuteResponse(ResponseData data)
    {
        if (data.checkError())
        {
            return ResponseCommandBase.Result.SUCCESS;
        }
        return ResponseCommandBase.Result.ERROR;
    }

    public override ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.LOGIN;
}

