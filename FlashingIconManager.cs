using System;
using System.Collections.Generic;

public class FlashingIconManager : SingletonMonoBehaviour<FlashingIconManager>
{
    protected static readonly float CYCLE_TIME = 1f;
    protected float dispTime;
    protected static float iconAlpha;
    protected List<FlashingIconComponent> iconList = new List<FlashingIconComponent>();
    protected bool isAddAlpha = true;

    public static void AddIcon(FlashingIconComponent icon)
    {
        List<FlashingIconComponent> iconList = SingletonMonoBehaviour<FlashingIconManager>.Instance.iconList;
        if (iconList.IndexOf(icon) < 0)
        {
            iconList.Add(icon);
        }
    }

    public static void Initialize()
    {
        FlashingIconManager instance = SingletonMonoBehaviour<FlashingIconManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
            ShiningIconManager.Initialize();
            NewIconManager.Initialize();
        }
    }

    protected void InitializeLocal()
    {
    }

    public static void Reboot()
    {
        FlashingIconManager instance = SingletonMonoBehaviour<FlashingIconManager>.Instance;
        if (instance != null)
        {
            instance.RebootLocal();
            ShiningIconManager.Reboot();
            NewIconManager.Reboot();
        }
    }

    protected void RebootLocal()
    {
        SingletonMonoBehaviour<FlashingIconManager>.Instance.iconList.Clear();
    }

    public static void RemoveIcon(FlashingIconComponent icon)
    {
        SingletonMonoBehaviour<FlashingIconManager>.Instance.iconList.Remove(icon);
    }

    protected void Update()
    {
        this.dispTime += RealTime.deltaTime;
        if (this.isAddAlpha)
        {
            if (this.dispTime >= CYCLE_TIME)
            {
                this.dispTime = 0f;
                this.isAddAlpha = false;
                iconAlpha = 1f;
            }
            else
            {
                iconAlpha = this.dispTime / CYCLE_TIME;
            }
        }
        else if (this.dispTime >= CYCLE_TIME)
        {
            this.dispTime = 0f;
            this.isAddAlpha = true;
            iconAlpha = 0f;
        }
        else
        {
            iconAlpha = 1f - (this.dispTime / CYCLE_TIME);
        }
        List<FlashingIconComponent> iconList = SingletonMonoBehaviour<FlashingIconManager>.Instance.iconList;
        for (int i = iconList.Count - 1; i >= 0; i--)
        {
            if (!iconList[i].UpdateIcon(iconAlpha))
            {
                iconList.RemoveAt(i);
            }
        }
        ShiningIconManager instance = SingletonMonoBehaviour<ShiningIconManager>.Instance;
        if (instance != null)
        {
            instance.UpdateAlpha(iconAlpha);
        }
        NewIconManager manager2 = SingletonMonoBehaviour<NewIconManager>.Instance;
        if (manager2 != null)
        {
            manager2.UpdateAlpha(iconAlpha);
        }
    }
}

