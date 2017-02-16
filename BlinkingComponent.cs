using System;
using UnityEngine;

public class BlinkingComponent : MonoBehaviour
{
    public const float ITVL_TIME = 0.75f;
    private bool mIsDisp;
    private bool mIsPlay;
    private float mOldTime;
    private Vector3 mOrgScl;

    private void Awake()
    {
        this.mOrgScl = base.gameObject.GetLocalScale();
    }

    private void OnEnable()
    {
        this.Play();
    }

    public void Play()
    {
        this.mOldTime = 0f;
        this.mIsPlay = true;
        this.PlayExec();
    }

    private void PlayExec()
    {
        if (this.mIsPlay)
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            float num2 = realtimeSinceStartup - this.mOldTime;
            if (num2 >= 0.75f)
            {
                this.mOldTime = realtimeSinceStartup;
                this.SetDisp(!this.mIsDisp);
            }
        }
    }

    private void SetDisp(bool is_disp)
    {
        this.mIsDisp = is_disp;
        Vector3 v = !this.mIsDisp ? Vector3.zero : this.mOrgScl;
        base.gameObject.SetLocalScale(v);
    }

    public void Stop()
    {
        this.mIsPlay = false;
        this.SetDisp(false);
    }

    private void Update()
    {
        this.PlayExec();
    }
}

