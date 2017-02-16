using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StandFigureRenderWaitStatus
{
    protected Face.Type faceType;
    protected int limitCount;
    protected RenderTexture renderTex;
    protected int svtId;
    protected Texture2D[] textureList;

    protected event EndHandler callbackFunc;

    public StandFigureRenderWaitStatus(int svtId, int limitCount, Face.Type faceType, Texture2D[] textureList, EndHandler callback)
    {
        this.svtId = svtId;
        this.limitCount = limitCount;
        this.faceType = faceType;
        this.textureList = textureList;
        this.renderTex = null;
        if (callback != null)
        {
            this.callbackFunc = callback;
        }
    }

    public StandFigureRenderWaitStatus(RenderTexture renderTex, int svtId, int limitCount, Face.Type faceType, Texture2D[] textureList, EndHandler callback)
    {
        this.svtId = svtId;
        this.limitCount = limitCount;
        this.faceType = faceType;
        this.textureList = textureList;
        this.renderTex = renderTex;
        if (callback != null)
        {
            this.callbackFunc = callback;
        }
    }

    public void Callback(RenderTexture renderTex)
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(renderTex);
        }
        else
        {
            renderTex.Release();
        }
    }

    public RenderTexture GetRenderTexture() => 
        this.renderTex;

    public void SetCharacter(UIStandFigureRender standFigureRender)
    {
        standFigureRender.SetCharacter(this.svtId, this.limitCount, this.faceType, this.textureList);
    }

    public delegate void EndHandler(RenderTexture texture);
}

