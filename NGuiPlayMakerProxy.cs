using System;
using UnityEngine;

public class NGuiPlayMakerProxy : MonoBehaviour
{
    public static string GetFsmEventEnumValue(Enum value)
    {
        string str = null;
        PlayMakerUtils_FsmEvent[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(PlayMakerUtils_FsmEvent), false) as PlayMakerUtils_FsmEvent[];
        if (customAttributes.Length > 0)
        {
            str = customAttributes[0].Value;
        }
        return str;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

