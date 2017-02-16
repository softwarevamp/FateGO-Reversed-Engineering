using System;
using UnityEngine;

public class BattleInfoMessageComponent : BaseMonoBehaviour
{
    private GameObject battleCommand;
    public Transform commandTarget;
    public Transform objTarget;
    public UILabel[] textLabel;

    public Transform getTargetTr()
    {
        if (this.objTarget != null)
        {
            return this.objTarget;
        }
        return base.gameObject.transform;
    }

    public void setCommandObject(GameObject command, BattleServantData svtData)
    {
        if (command != null)
        {
            if (this.battleCommand != null)
            {
                UnityEngine.Object.Destroy(this.battleCommand);
            }
            this.battleCommand = base.createObject(command, this.commandTarget, null);
            BattleCommandComponent comp = command.GetComponent<BattleCommandComponent>();
            BattleCommandComponent component = this.battleCommand.GetComponent<BattleCommandComponent>();
            component.setAttackCommandData(comp);
            component.resetAddObject();
            if (svtData != null)
            {
                component.updateClassMag(svtData.getClassId());
            }
            else
            {
                component.updateClassMag(-1);
            }
        }
    }

    public void setText(string str)
    {
        this.textLabel[0].text = str;
    }

    public void setText(string str, string str2)
    {
        this.textLabel[0].text = str;
        if (this.textLabel[1] != null)
        {
            this.textLabel[1].text = str2;
        }
    }
}

