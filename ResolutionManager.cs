using System;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public bool enableFlip;
    private bool is3DCamera;
    public int logicalHeight = 0x240;
    public int logicalWidth = 0x400;

    private void LateUpdate()
    {
    }

    private void OnPostRender()
    {
        if (this.enableFlip && this.is3DCamera)
        {
            GL.SetRevertBackfacing(false);
        }
    }

    private void OnPreCull()
    {
        if (this.enableFlip && this.is3DCamera)
        {
            base.GetComponent<Camera>().ResetWorldToCameraMatrix();
            base.GetComponent<Camera>().ResetProjectionMatrix();
            base.GetComponent<Camera>().projectionMatrix *= Matrix4x4.Scale(new Vector3(!BattlePerformance.CameraFlip ? 1f : -1f, 1f, 1f));
        }
    }

    private void OnPreRender()
    {
        if (this.enableFlip && this.is3DCamera)
        {
            GL.SetRevertBackfacing(BattlePerformance.CameraFlip);
        }
    }

    private void Start()
    {
        float width = Screen.width;
        float height = Screen.height;
        float logicalWidth = this.logicalWidth;
        float logicalHeight = this.logicalHeight;
        UIRoot component = base.GetComponent<UIRoot>();
        if (component != null)
        {
            int num5 = (int) Mathf.Round((((float) this.logicalWidth) / width) * height);
            int num6 = (int) Mathf.Round((((float) this.logicalHeight) / height) * width);
            float num7 = (Screen.height * logicalWidth) / (Screen.width * logicalHeight);
            int num8 = (num7 <= 1f) ? ((int) logicalHeight) : ((int) (logicalHeight * num7));
            int num9 = (num7 <= 1f) ? ((int) (logicalWidth * num7)) : ((int) logicalWidth);
            if (num8 == ((int) logicalHeight))
            {
                component.manualWidth = num6;
            }
            else
            {
                component.manualHeight = num5;
                component.minimumHeight = num5;
                component.maximumHeight = num5;
            }
        }
        else
        {
            this.is3DCamera = true;
        }
        Camera camera = base.GetComponent<Camera>();
        if (camera != null)
        {
            float num10 = 1f;
            if (((width / logicalHeight) * logicalWidth) >= Screen.width)
            {
                num10 = (width * logicalHeight) / (height * logicalWidth);
            }
            camera.rect = new Rect(0f, (1f - num10) / 2f, 1f, num10);
        }
    }
}

