using System;

public class SingletonTemplate<CLASS> where CLASS: class, new()
{
    protected static CLASS mInstance;

    protected SingletonTemplate()
    {
    }

    public static void Delete()
    {
        if (SingletonTemplate<CLASS>.mInstance != null)
        {
            SingletonTemplate<CLASS>.mInstance = null;
        }
    }

    public virtual void Destroy()
    {
    }

    public static CLASS Instance
    {
        get
        {
            if (SingletonTemplate<CLASS>.mInstance == null)
            {
                SingletonTemplate<CLASS>.mInstance = Activator.CreateInstance<CLASS>();
            }
            return SingletonTemplate<CLASS>.mInstance;
        }
    }
}

