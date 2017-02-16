using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class OptionManager : SingletonMonoBehaviour<OptionManager>
{
    protected static bool isModify;

    public static float GetBgmVolume() => 
        PlayerPrefs.GetFloat("OptionBgmVolume", 0.4f);

    public static bool GetLocalNotiffication() => 
        (PlayerPrefs.GetInt("OptionLocalNotiffication", 1) != 0);

    public static bool GetMessageShowfication() => 
        (PlayerPrefs.GetInt("MessageShowfication", 1) != 0);

    public static bool GetNotiffication() => 
        (PlayerPrefs.GetInt("OptionNotiffication", 1) != 0);

    public static float GetSeVolume() => 
        PlayerPrefs.GetFloat("OptionSeVolume", 0.9f);

    public static float GetVoiceVolume() => 
        PlayerPrefs.GetFloat("OptionVoiceVolume", 1f);

    public static void Initialize()
    {
        Recover();
    }

    public static void Recover()
    {
        SoundManager.SetBgmMasterVolume(GetBgmVolume());
        SoundManager.SetSeMasterVolume(GetSeVolume());
        SoundManager.SetVoiceMasterVolume(GetVoiceVolume());
    }

    public static bool SaveData()
    {
        if (isModify)
        {
            isModify = false;
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static void SetBgmVolume(float v)
    {
        float bgmVolume = GetBgmVolume();
        if (v != bgmVolume)
        {
            PlayerPrefs.SetFloat("OptionBgmVolume", v);
            isModify = true;
            SoundManager.SetBgmMasterVolume(v);
        }
    }

    public static void SetLocalNotiffication(bool f)
    {
        bool localNotiffication = GetLocalNotiffication();
        if (f != localNotiffication)
        {
            PlayerPrefs.SetInt("OptionLocalNotiffication", !f ? 0 : 1);
            isModify = true;
        }
    }

    public static void SetMessageShowfication(bool f)
    {
        bool messageShowfication = GetMessageShowfication();
        if (f != messageShowfication)
        {
            PlayerPrefs.SetInt("MessageShowfication", !f ? 0 : 1);
            isModify = true;
        }
    }

    public static void SetNotiffication(bool f, bool forceSend = false)
    {
        bool notiffication = GetNotiffication();
        if ((f != notiffication) || forceSend)
        {
            PlayerPrefs.SetInt("OptionNotiffication", !f ? 0 : 1);
            isModify = true;
            if (SingletonMonoBehaviour<NotificationManager>.Instance != null)
            {
                SingletonMonoBehaviour<NotificationManager>.Instance.SetRemotePushState(f);
            }
        }
    }

    public static void SetSeVolume(float v)
    {
        float seVolume = GetSeVolume();
        if (v != seVolume)
        {
            PlayerPrefs.SetFloat("OptionSeVolume", v);
            isModify = true;
            SoundManager.SetSeMasterVolume(v);
        }
    }

    public static void SetVoiceVolume(float v)
    {
        float voiceVolume = GetVoiceVolume();
        if (v != voiceVolume)
        {
            PlayerPrefs.SetFloat("OptionVoiceVolume", v);
            isModify = true;
            SoundManager.SetVoiceMasterVolume(v);
        }
    }

    public static void TestBgmVolume(float v)
    {
        SoundManager.SetBgmMasterVolume(v);
    }

    public static void TestSeVolume(float v)
    {
        SoundManager.SetSeMasterVolume(v);
    }

    public static void TestVoiceVolume(float v)
    {
        SoundManager.SetVoiceMasterVolume(v);
    }
}

