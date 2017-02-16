using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Slider Colors")]
public class UISliderColors : MonoBehaviour
{
    public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };
    private UIProgressBar mBar;
    private UIBasicSprite mSprite;
    public UISprite sprite;

    private void Start()
    {
        this.mBar = base.GetComponent<UIProgressBar>();
        this.mSprite = base.GetComponent<UIBasicSprite>();
        this.Update();
    }

    private void Update()
    {
        if ((this.sprite != null) && (this.colors.Length != 0))
        {
            float f = (this.mBar == null) ? this.mSprite.fillAmount : this.mBar.value;
            f *= this.colors.Length - 1;
            int index = Mathf.FloorToInt(f);
            Color color = this.colors[0];
            if (index >= 0)
            {
                if ((index + 1) < this.colors.Length)
                {
                    float t = f - index;
                    color = Color.Lerp(this.colors[index], this.colors[index + 1], t);
                }
                else if (index < this.colors.Length)
                {
                    color = this.colors[index];
                }
                else
                {
                    color = this.colors[this.colors.Length - 1];
                }
            }
            color.a = this.sprite.color.a;
            this.sprite.color = color;
        }
    }
}

