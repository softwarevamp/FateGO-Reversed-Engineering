public class DeckSetupResponseCommand : ResponseCommandBase
{
    public override ResponseCommandBase.Result ExecuteResponse(ResponseData data)
    {
        if (data.success != null)
        {
            return ResponseCommandBase.Result.SUCCESS;
        }
        return ResponseCommandBase.Result.ERROR;
    }

    public override ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.DECK_SETUP;
}

