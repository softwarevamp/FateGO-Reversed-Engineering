using System;
using System.Collections.Generic;

public class DataMasterBase
{
    protected static bool _never;
    public string cachename = string.Empty;
    protected List<DataEntityBase> list = new List<DataEntityBase>();
    protected Dictionary<string, DataEntityBase> lookup = new Dictionary<string, DataEntityBase>();

    public void Clear()
    {
        this.list.Clear();
        this.lookup.Clear();
    }

    protected string createKey(params long[] args)
    {
        string str = string.Empty;
        int num = 0;
        foreach (long num2 in args)
        {
            str = str + num2;
            if (num != (args.Length - 1))
            {
                str = str + ":";
            }
            num++;
        }
        return str;
    }

    public bool Deleted(object obj)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.DispLog)
        {
            Debug.Log("Deleted:" + this.cachename);
        }
        foreach (DataEntityBase base2 in this.getList(obj))
        {
            string key = base2.getPrimarykey();
            if (key == null)
            {
                Debug.LogError("deleted " + this.cachename + " primary key is null error");
                return false;
            }
            if (this.lookup.ContainsKey(key))
            {
                this.list.Remove(this.lookup[key]);
                this.lookup.Remove(key);
            }
        }
        return true;
    }

    public string getCacheName() => 
        this.cachename;

    public T getEntityFromId<T>(int id) where T: DataEntityBase
    {
        string key = string.Empty + id;
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public T getEntityFromId<T>(long id) where T: DataEntityBase
    {
        string key = string.Empty + id;
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public T getEntityFromId<T>(params long[] args) where T: DataEntityBase
    {
        string str = this.createKey(args);
        if (this.isEntityExistsFromId(args))
        {
            return (T) this.lookup[str];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + str + "]");
        return null;
    }

    public T getEntityFromId<T>(int id1, int id2) where T: DataEntityBase
    {
        object[] objArray1 = new object[] { string.Empty, id1, ":", id2 };
        string key = string.Concat(objArray1);
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public T getEntityFromId<T>(int id1, int id2, int id3) where T: DataEntityBase
    {
        object[] objArray1 = new object[] { string.Empty, id1, ":", id2, ":", id3 };
        string key = string.Concat(objArray1);
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public T getEntityFromId<T>(int id1, int id2, int id3, int id4) where T: DataEntityBase
    {
        object[] objArray1 = new object[] { string.Empty, id1, ":", id2, ":", id3, ":", id4 };
        string key = string.Concat(objArray1);
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public T getEntityFromKey<T>(string key) where T: DataEntityBase
    {
        if (this.lookup.ContainsKey(key))
        {
            return (T) this.lookup[key];
        }
        Debug.LogError("Not Found: " + this.getCacheName() + " [" + key + "]");
        return null;
    }

    public List<DataEntityBase> getEntityList() => 
        this.list;

    public DataEntityBase[] getEntitys() => 
        this.list.ToArray();

    public T[] getEntitys<T>() where T: DataEntityBase
    {
        T[] localArray = new T[this.list.Count];
        for (int i = 0; i < this.list.Count; i++)
        {
            localArray[i] = this.list[i];
        }
        return localArray;
    }

    public virtual DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<DataEntityBase>(obj);

    public T getSingleEntity<T>() where T: DataEntityBase
    {
        if (this.list.Count > 1)
        {
            Debug.LogError(this.getCacheName() + " getSingleEntity() is not single data");
        }
        foreach (DataEntityBase base2 in this.list)
        {
            return (T) base2;
        }
        Debug.LogError("Not Found: " + this.getCacheName());
        return null;
    }

    public T getUserIdEntity<T>() where T: DataEntityBase => 
        this.getEntityFromId<T>(NetworkManager.UserId);

    public bool isEntityExistsFromId(params long[] args)
    {
        string key = this.createKey(args);
        return this.lookup.ContainsKey(key);
    }

    public bool isSingleEntityExists()
    {
        if (this.list.Count > 1)
        {
            Debug.LogError(this.getCacheName() + " getSingleEntity() is not single data");
        }
        return (this.list.Count == 1);
    }

    public virtual bool preProcess() => 
        false;

    public bool Replaced(object obj)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.DispLog)
        {
            Debug.Log("Replaced:" + this.cachename);
        }
        this.list.Clear();
        this.lookup.Clear();
        this.list.AddRange(this.getList(obj));
        foreach (DataEntityBase base2 in this.list)
        {
            string key = base2.getPrimarykey();
            if (key == null)
            {
                break;
            }
            if (this.lookup.ContainsKey(key))
            {
                Debug.LogError("Replaced same key add error <" + this.cachename + "> [" + key + "]");
            }
            else
            {
                this.lookup.Add(key, base2);
            }
        }
        if (SingletonMonoBehaviour<DataManager>.Instance.DispLog)
        {
            Debug.Log("Replaced Result:" + this.cachename);
        }
        return true;
    }

    public bool Updated(object obj)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.DispLog)
        {
            Debug.Log("Updated:" + this.cachename);
        }
        foreach (DataEntityBase base2 in this.getList(obj))
        {
            string key = base2.getPrimarykey();
            if (key == null)
            {
                Debug.LogError("update " + this.cachename + " primary key is null error");
                return false;
            }
            if (this.lookup.ContainsKey(key))
            {
                this.list.Remove(this.lookup[key]);
                this.lookup.Remove(key);
            }
            this.lookup.Add(key, base2);
            this.list.Add(base2);
        }
        return true;
    }
}

