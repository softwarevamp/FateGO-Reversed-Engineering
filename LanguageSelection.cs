using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Language Selection"), RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
    [CompilerGenerated]
    private static EventDelegate.Callback <>f__am$cache1;
    private UIPopupList mList;

    private void Awake()
    {
        this.mList = base.GetComponent<UIPopupList>();
        this.Refresh();
    }

    public void Refresh()
    {
        if ((this.mList != null) && (Localization.knownLanguages != null))
        {
            this.mList.items.Clear();
            int index = 0;
            int length = Localization.knownLanguages.Length;
            while (index < length)
            {
                this.mList.items.Add(Localization.knownLanguages[index]);
                index++;
            }
            this.mList.value = Localization.language;
        }
    }

    private void Start()
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = (EventDelegate.Callback) (() => (Localization.language = UIPopupList.current.value));
        }
        EventDelegate.Add(this.mList.onChange, <>f__am$cache1);
    }
}

