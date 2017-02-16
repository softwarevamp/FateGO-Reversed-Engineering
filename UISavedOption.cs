using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
    public string keyName;
    private UIToggle mCheck;
    private UIPopupList mList;
    private UIProgressBar mSlider;

    private void Awake()
    {
        this.mList = base.GetComponent<UIPopupList>();
        this.mCheck = base.GetComponent<UIToggle>();
        this.mSlider = base.GetComponent<UIProgressBar>();
    }

    private void OnDisable()
    {
        if (this.mCheck != null)
        {
            EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
        }
        else if (this.mList != null)
        {
            EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
        }
        else if (this.mSlider != null)
        {
            EventDelegate.Remove(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
        }
        else
        {
            UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UIToggle toggle = componentsInChildren[index];
                if (toggle.value)
                {
                    PlayerPrefs.SetString(this.key, toggle.name);
                    break;
                }
                index++;
            }
        }
    }

    private void OnEnable()
    {
        if (this.mList != null)
        {
            EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
            string str = PlayerPrefs.GetString(this.key);
            if (!string.IsNullOrEmpty(str))
            {
                this.mList.value = str;
            }
        }
        else if (this.mCheck != null)
        {
            EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
            this.mCheck.value = PlayerPrefs.GetInt(this.key, !this.mCheck.startsActive ? 0 : 1) != 0;
        }
        else if (this.mSlider != null)
        {
            EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.SaveProgress));
            this.mSlider.value = PlayerPrefs.GetFloat(this.key, this.mSlider.value);
        }
        else
        {
            string str2 = PlayerPrefs.GetString(this.key);
            UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UIToggle toggle = componentsInChildren[index];
                toggle.value = toggle.name == str2;
                index++;
            }
        }
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetFloat(this.key, UIProgressBar.current.value);
    }

    public void SaveSelection()
    {
        PlayerPrefs.SetString(this.key, UIPopupList.current.value);
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt(this.key, !UIToggle.current.value ? 0 : 1);
    }

    private string key =>
        (!string.IsNullOrEmpty(this.keyName) ? this.keyName : ("NGUI State: " + base.name));
}

