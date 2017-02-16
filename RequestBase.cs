using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class RequestBase
{
    public NetworkManager.ResultCallbackFunc CallBack;
    protected Dictionary<string, int> paramInteger = new Dictionary<string, int>();
    protected Dictionary<string, string> paramString = new Dictionary<string, string>();

    public void addActionField(string key)
    {
        Debug.Log("AddActionFiled : Action = action;;Key = " + key);
        this.paramString.Add("ac", "action");
        this.paramString.Add("key", key);
        this.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
        this.addField("os", SystemInfo.operatingSystem);
        this.addField("ptype", SystemInfo.deviceModel);
        this.addField("userAgent", 1);
        string str2 = CryptData.EncryptMD5Usk(PlayerPrefs.GetString("usk"));
        this.paramString.Add("usk", str2);
        this.paramString.Add("umk", string.Empty);
        this.paramString.Add("rgsid", SingletonMonoBehaviour<DataManager>.Instance.serverId.ToString());
        this.paramString.Add("rkchannel", "24");
    }

    public void addBaseField()
    {
        NetworkManager.SetBaseField(this);
    }

    public void addDefaultField(string ac)
    {
        Debug.Log("AddActionFiled : Action = " + ac);
        this.paramString.Add("ac", ac);
        this.addField("deviceid", SystemInfo.deviceUniqueIdentifier);
        this.addField("os", SystemInfo.operatingSystem);
        this.addField("ptype", SystemInfo.deviceModel);
        string str2 = CryptData.EncryptMD5Usk(PlayerPrefs.GetString("usk"));
        this.paramString.Add("usk", str2);
        this.paramString.Add("umk", string.Empty);
        this.paramString.Add("rgsid", SingletonMonoBehaviour<DataManager>.Instance.serverId.ToString());
        this.paramString.Add("rkchannel", "24");
    }

    public void addField(string fieldName, double data)
    {
        this.paramString.Add(fieldName, string.Empty + data);
    }

    public void addField(string fieldName, int data)
    {
        this.paramInteger.Add(fieldName, data);
    }

    public void addField(string fieldName, long data)
    {
        this.paramString.Add(fieldName, string.Empty + data);
    }

    public void addField(string fieldName, object data)
    {
        string str = JsonManager.toJson(data);
        this.paramString.Add(fieldName, str);
    }

    public void addField(string fieldName, float data)
    {
        this.paramString.Add(fieldName, string.Empty + data);
    }

    public void addField(string fieldName, string data)
    {
        this.paramString.Add(fieldName, data);
    }

    public void beginRequest()
    {
        this.addBaseField();
        NetworkManager.RequestStart(this);
    }

    public bool beginRetryRequest(bool isRefreshTime = false)
    {
        if (this.ReadParameter(isRefreshTime))
        {
            NetworkManager.RequestStart(this);
            return true;
        }
        return false;
    }

    public virtual bool checkExpirationDate() => 
        false;

    public void ClearField()
    {
        this.paramInteger.Clear();
        this.paramString.Clear();
    }

    protected void ClearParameter()
    {
        if (!ManagerConfig.UseMock)
        {
            string path = this.getParameterFileName();
            if (path != null)
            {
                Debug.Log("ClearParameter: " + path);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }

    public void completed(string result)
    {
        if (this.CallBack != null)
        {
            this.CallBack(result);
        }
    }

    public virtual string getMockData() => 
        null;

    public virtual string getMockURL() => 
        null;

    protected virtual string getParameterFileName() => 
        null;

    public virtual string getURL() => 
        null;

    public string getValueFromPam(string key)
    {
        string str = string.Empty;
        if (this.paramString.TryGetValue(key, out str))
        {
            return this.paramString[key];
        }
        return string.Empty;
    }

    public WWWForm getWWWForm(out SortedDictionary<string, string> authParams)
    {
        Debug.Log("RequestBase:getWWWForm");
        WWWForm form = new WWWForm();
        authParams = new SortedDictionary<string, string>();
        foreach (string str in this.paramString.Keys)
        {
            Debug.Log("    [" + str + "]:" + this.paramString[str]);
            form.AddField(str, this.paramString[str]);
            authParams.Add(str, this.paramString[str]);
        }
        foreach (string str2 in this.paramInteger.Keys)
        {
            Debug.Log(string.Concat(new object[] { "    [", str2, "]:", this.paramInteger[str2] }));
            form.AddField(str2, this.paramInteger[str2]);
            authParams.Add(str2, Convert.ToString(this.paramInteger[str2]));
        }
        return form;
    }

    protected bool ReadParameter(bool isRefreshTime = false)
    {
        if (!ManagerConfig.UseMock)
        {
            string path = this.getParameterFileName();
            if (path == null)
            {
                return false;
            }
            Debug.Log("ReadParameter: " + path);
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                try
                {
                    this.ClearField();
                    int num = reader.ReadInt32();
                    int num2 = reader.ReadInt32();
                    for (int i = 0; i < num; i++)
                    {
                        string key = reader.ReadString();
                        string str3 = reader.ReadString();
                        this.paramString.Add(key, str3);
                    }
                    for (int j = 0; j < num2; j++)
                    {
                        string str4 = reader.ReadString();
                        int num5 = reader.ReadInt32();
                        this.paramInteger.Add(str4, num5);
                    }
                    NetworkManager.ReplaceBaseField(this, isRefreshTime);
                    return true;
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        ((IDisposable) reader).Dispose();
                    }
                }
            }
        }
        return false;
    }

    public void replaceField(string fieldName, double data)
    {
        this.paramString[fieldName] = string.Empty + data;
    }

    public void replaceField(string fieldName, int data)
    {
        this.paramInteger[fieldName] = data;
    }

    public void replaceField(string fieldName, long data)
    {
        this.paramString[fieldName] = string.Empty + data;
    }

    public void replaceField(string fieldName, object data)
    {
        string str = JsonManager.toJson(data);
        this.paramString[fieldName] = str;
    }

    public void replaceField(string fieldName, float data)
    {
        this.paramString[fieldName] = string.Empty + data;
    }

    public void replaceField(string fieldName, string data)
    {
        this.paramString[fieldName] = data;
    }

    public virtual void requestCompleted(ResponseData[] responseList)
    {
        this.completed(null);
    }

    protected void WriteParameter()
    {
        if (!ManagerConfig.UseMock)
        {
            string path = this.getParameterFileName();
            if (path != null)
            {
                Debug.Log("WriteParameter: " + path);
                BinaryWriter writer = new BinaryWriter(File.OpenWrite(path));
                try
                {
                    writer.Write(this.paramString.Count);
                    writer.Write(this.paramInteger.Count);
                    foreach (string str2 in this.paramString.Keys)
                    {
                        writer.Write(str2);
                        writer.Write(this.paramString[str2]);
                    }
                    foreach (string str3 in this.paramInteger.Keys)
                    {
                        writer.Write(str3);
                        writer.Write(this.paramInteger[str3]);
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                }
                finally
                {
                    if (writer != null)
                    {
                        ((IDisposable) writer).Dispose();
                    }
                }
            }
        }
    }
}

