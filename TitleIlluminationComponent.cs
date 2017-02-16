using System;
using UnityEngine;

public class TitleIlluminationComponent : MonoBehaviour
{
    public int IlluminationX;
    public int IlluminationY;
    public float lifetime;
    public float lifetimeMax;
    protected TitleIlluminationManager manager;

    public void Setup(int x, int y, int sz, float lifetime, TitleIlluminationManager manager)
    {
        this.IlluminationX = x;
        this.IlluminationY = y;
        this.lifetimeMax = this.lifetime = lifetime;
        this.manager = manager;
        float num = x * sz;
        float num2 = y * (sz / 2);
        if ((y % 2) == 0)
        {
            num -= sz / 2;
        }
        base.transform.localPosition = new Vector3(num, num2, 0f);
        UISprite component = base.GetComponent<UISprite>();
        Color color = component.color;
        component.color = new Color(color.r, color.g, color.b, 1f);
        component.depth = 1;
        component.MarkAsChanged();
    }

    private void Start()
    {
    }

    private void Update()
    {
        this.lifetime -= Time.deltaTime;
        UISprite component = base.GetComponent<UISprite>();
        if (component != null)
        {
            if (this.lifetime <= 0f)
            {
                this.manager.ReturnIllumination(this);
                component.alpha = 0f;
            }
            else
            {
                float num = this.lifetime / this.lifetimeMax;
                component.alpha = num;
            }
            component.MarkAsChanged();
        }
    }
}

