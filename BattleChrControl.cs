using System;
using UnityEngine;

public class BattleChrControl : MonoBehaviour
{
    private GameObject targetObject;

    private void OnAnimEvent(AnimationEvent ev)
    {
        this.targetObject.SendMessage("OnAnimEvent", ev);
    }

    public void setTarget(GameObject obj)
    {
        this.targetObject = obj;
    }
}

