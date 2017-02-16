using System;

public class CStateManager<T> where T: class
{
    private CFSM<T> m_fsm;
    private int m_state;
    private IState<T>[] m_state_table;

    public CStateManager(T that, int max)
    {
        this.m_fsm = new CFSM<T>(that);
        this.m_state_table = new IState<T>[max];
    }

    public void add(int idx, IState<T> si)
    {
        this.m_state_table[idx] = si;
    }

    public void destroy()
    {
        this.m_fsm.destroy();
    }

    public int getState() => 
        this.m_state;

    public void setState(int idx)
    {
        this.m_state = idx;
        this.m_fsm.setState(this.m_state_table[idx]);
    }

    public void update()
    {
        this.m_fsm.update();
    }
}

