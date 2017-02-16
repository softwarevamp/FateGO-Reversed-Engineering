using System;
using System.Collections.Generic;
using UnityEngine;

public class NewIconManager : SingletonMonoBehaviour<NewIconManager>
{
    protected List<NewIconComponent> iconList = new List<NewIconComponent>();
    protected static Material newIconMaterial;
    [SerializeField]
    protected Texture2D newIconTexture;

    public static void AddIcon(NewIconComponent icon)
    {
        List<NewIconComponent> iconList = SingletonMonoBehaviour<NewIconManager>.Instance.iconList;
        if (iconList.IndexOf(icon) < 0)
        {
            iconList.Add(icon);
        }
    }

    public static void Initialize()
    {
        NewIconManager instance = SingletonMonoBehaviour<NewIconManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    protected void InitializeLocal()
    {
        if (newIconMaterial == null)
        {
            newIconMaterial = new Material(Shader.Find("Custom/NewIconRender"));
            newIconMaterial.mainTexture = this.newIconTexture;
        }
    }

    public static void Reboot()
    {
        NewIconManager instance = SingletonMonoBehaviour<NewIconManager>.Instance;
        if (instance != null)
        {
            instance.RebootLocal();
        }
    }

    protected void RebootLocal()
    {
        SingletonMonoBehaviour<NewIconManager>.Instance.iconList.Clear();
    }

    public static void RemoveIcon(NewIconComponent icon)
    {
        SingletonMonoBehaviour<NewIconManager>.Instance.iconList.Remove(icon);
    }

    public static void SetIcon(UITexture texture)
    {
        texture.material = newIconMaterial;
    }

    public void UpdateAlpha(float alpha)
    {
        if (newIconMaterial != null)
        {
            newIconMaterial.SetFloat("_Add", alpha);
            List<NewIconComponent> iconList = SingletonMonoBehaviour<NewIconManager>.Instance.iconList;
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

