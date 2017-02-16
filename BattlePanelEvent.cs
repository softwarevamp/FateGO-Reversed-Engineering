using System;
using UnityEngine;

public class BattlePanelEvent : MonoBehaviour
{
    public GameObject Panel;

    private void OnDestroy()
    {
        this.SetPanelDisable();
    }

    private void SetPanelActive()
    {
        this.SetPanelActive(true);
    }

    private void SetPanelActive(bool flag)
    {
        if (this.Panel != null)
        {
            this.Panel.SetActive(flag);
        }
    }

    private void SetPanelDisable()
    {
        this.SetPanelActive(false);
    }
}

