using System;
using UnityEngine;

public class AutoBattleButtonComponent : MonoBehaviour
{
    public UISprite buttonSprite;
    public BattleLogic logic;
    public Collider overPanelCollider;

    public void InitButton()
    {
        if (this.logic.data.systemflg_showautobutton)
        {
            base.gameObject.SetActive(true);
            if (this.logic.data.systemflg_autobattle)
            {
                this.buttonSprite.color = Color.white;
            }
            else
            {
                this.buttonSprite.color = Color.gray;
            }
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        this.logic.data.systemflg_autobattle = !this.logic.data.systemflg_autobattle;
        if (this.logic.data.systemflg_autobattle)
        {
            this.buttonSprite.color = Color.white;
        }
        else
        {
            this.buttonSprite.color = Color.gray;
        }
    }
}

