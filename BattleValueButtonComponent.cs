using System;
using UnityEngine;

public class BattleValueButtonComponent : MonoBehaviour
{
    public string sendmessage;
    public GameObject target;
    public int val;

    public void OnClickTarget()
    {
        Debug.Log("OnClickTarget");
        if (this.target != null)
        {
            this.target.SendMessage(this.sendmessage, this.val);
        }
    }
}

