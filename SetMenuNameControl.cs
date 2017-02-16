using System;
using UnityEngine;

public class SetMenuNameControl : MonoBehaviour
{
    public UILabel menuNameLb;

    public void setMenuName(string menuName)
    {
        this.menuNameLb.text = menuName;
    }
}

