using System;
using UnityEngine;

public class DataAsset : UnityEngine.Object
{
    protected byte[] byteData;
    protected string textData;

    public DataAsset(byte[] bytes)
    {
        this.byteData = bytes;
    }

    public DataAsset(string text)
    {
        this.textData = text;
    }

    public byte[] bytes =>
        this.byteData;

    public string text =>
        this.textData;
}

