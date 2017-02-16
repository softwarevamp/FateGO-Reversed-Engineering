using System;

public interface IState<T> where T: class
{
    void begin(T that);
    void end(T that);
    void update(T that);
}

