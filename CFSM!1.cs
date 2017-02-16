using System;

public class CFSM<T> where T: class
{
    private IState<T> m_current;
    private T that;

    public CFSM(T arg)
    {
        this.that = arg;
    }

    public void _set(IState<T> val)
    {
        this.m_current = val;
    }

    public void destroy()
    {
        if (this.m_current != null)
        {
            this.m_current.end(this.that);
        }
    }

    public IState<T> getState() => 
        this.m_current;

    public void setState(IState<T> val)
    {
        if (this.m_current != null)
        {
            this.m_current.end(this.that);
        }
        this._set(val);
        if (this.m_current != null)
        {
            this.m_current.begin(this.that);
        }
    }

    public void update()
    {
        if (this.m_current != null)
        {
            this.m_current.update(this.that);
        }
    }
}

