using System;
using UnityEngine;

public class BattleFieldEffectComponent : BaseMonoBehaviour
{
    public Color[] ambientlist;
    public int[] bglist;
    public GameObject[] fieldeffect;
    public Transform root2D;
    private GameObject viewobject;

    public int getAmbientColors() => 
        this.ambientlist.Length;

    public string[] getFieldEffects()
    {
        string[] strArray = new string[this.fieldeffect.Length];
        for (int i = 0; i < strArray.Length; i++)
        {
            strArray[i] = this.fieldeffect[i].name;
        }
        return strArray;
    }

    public void setAmbientColor(int index)
    {
        RenderSettings.ambientLight = this.ambientlist[index];
    }

    public void setFieldEffect(int index)
    {
        if (this.viewobject != null)
        {
            UnityEngine.Object.Destroy(this.viewobject);
        }
        if ((0 <= index) && (index < this.fieldeffect.Length))
        {
            this.viewobject = base.createObject(this.fieldeffect[index], this.root2D, null);
        }
    }
}

