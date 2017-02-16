using System;

public class ResponseCommandBase : ResponseData
{
    public virtual Result ExecuteResponse(ResponseData data) => 
        Result.SUCCESS;

    public string GetCommandName() => 
        ResponseCommandKind.GetName(this.GetKind());

    public virtual ResponseCommandKind.Kind GetKind() => 
        ResponseCommandKind.Kind.NONE;

    public enum Result
    {
        SUCCESS,
        ERROR,
        RETRY
    }
}

