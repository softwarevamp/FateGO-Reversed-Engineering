using System;
using UnityEngine;

public class NoticeNumberComponent : MonoBehaviour
{
    public static readonly float ALPHA_SPEED_RATE = 0.4f;
    public static readonly int DISP_NUMBER_MAX = 0x3e7;
    private bool mIsActive;
    private int mNumber;
    private float mTgtAlpha;
    private UIWidget mWidget;
    [SerializeField]
    private UILabel numberLabel;

    private void Awake()
    {
        this.mIsActive = true;
        this.mWidget = base.gameObject.GetComponent<UIWidget>();
        this.SetDisp(true);
    }

    public int GetNumber() => 
        this.mNumber;

    public UILabel GetNumberLabel() => 
        this.numberLabel;

    public void SetDisp(bool is_disp)
    {
        this.mTgtAlpha = !is_disp ? ((float) 0) : ((float) 1);
    }

    public void SetNumber(int number)
    {
        this.mNumber = number;
        base.gameObject.SetActive(this.mNumber > 0);
        this.numberLabel.text = (this.mNumber > DISP_NUMBER_MAX) ? (DISP_NUMBER_MAX.ToString() + "+") : this.mNumber.ToString();
    }

    private void Update()
    {
        if (this.mWidget.alpha != this.mTgtAlpha)
        {
            this.mWidget.alpha += (this.mTgtAlpha - this.mWidget.alpha) * ALPHA_SPEED_RATE;
        }
    }
}

