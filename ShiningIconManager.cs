using System;
using System.Collections.Generic;
using UnityEngine;

public class ShiningIconManager : SingletonMonoBehaviour<ShiningIconManager>
{
    protected List<ShiningIconComponent> iconList = new List<ShiningIconComponent>();
    [SerializeField]
    protected UIAtlas shiningIconAtlas;
    protected static Material shiningIconMaterial;

    public static void AddIcon(ShiningIconComponent icon)
    {
        List<ShiningIconComponent> iconList = SingletonMonoBehaviour<ShiningIconManager>.Instance.iconList;
        if (iconList.IndexOf(icon) < 0)
        {
            iconList.Add(icon);
        }
    }

    public static void Initialize()
    {
        ShiningIconManager instance = SingletonMonoBehaviour<ShiningIconManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    protected void InitializeLocal()
    {
        shiningIconMaterial = this.shiningIconAtlas.spriteMaterial;
    }

    public static void Reboot()
    {
        ShiningIconManager instance = SingletonMonoBehaviour<ShiningIconManager>.Instance;
        if (instance != null)
        {
            instance.RebootLocal();
        }
    }

    protected void RebootLocal()
    {
        SingletonMonoBehaviour<ShiningIconManager>.Instance.iconList.Clear();
    }

    public static void RemoveIcon(ShiningIconComponent icon)
    {
        SingletonMonoBehaviour<ShiningIconManager>.Instance.iconList.Remove(icon);
    }

    public void UpdateAlpha(float alpha)
    {
        if (shiningIconMaterial != null)
        {
            shiningIconMaterial.SetFloat("_Add", alpha);
            List<ShiningIconComponent> iconList = SingletonMonoBehaviour<ShiningIconManager>.Instance.iconList;
            for (int i = iconList.Count - 1; i >= 0; i--)
            {
                if (!iconList[i].UpdateIcon())
                {
                    iconList.RemoveAt(i);
                }
            }
        }
    }
}

