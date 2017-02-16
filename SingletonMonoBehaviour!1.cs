using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    protected void Awake()
    {
        this.CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (this == SingletonMonoBehaviour<T>.Instance)
        {
            return true;
        }
        UnityEngine.Object.Destroy(this);
        return false;
    }

    public static T getInstance() => 
        SingletonMonoBehaviour<T>.instance;

    public static T Instance
    {
        get
        {
            if (SingletonMonoBehaviour<T>.instance == null)
            {
                SingletonMonoBehaviour<T>.instance = (T) UnityEngine.Object.FindObjectOfType(typeof(T));
                if (SingletonMonoBehaviour<T>.instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
            }
            return SingletonMonoBehaviour<T>.instance;
        }
    }
}

