using System;
using System.Runtime.CompilerServices;

public class ClassButtonComponent : UICommonButton
{
    protected int classPos;

    protected event CallbackFunc callbackFunc;

    public void OnSelectButton()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(this.classPos);
        }
    }

    public void setClassPos(int classPos, CallbackFunc callback)
    {
        this.classPos = classPos;
        this.callbackFunc = callback;
    }

    public int ClassPos =>
        this.classPos;

    public delegate void CallbackFunc(int classPos);
}

