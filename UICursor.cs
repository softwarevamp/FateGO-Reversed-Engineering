using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Cursor"), RequireComponent(typeof(UISprite))]
public class UICursor : MonoBehaviour
{
    public static UICursor instance;
    private UIAtlas mAtlas;
    private UISprite mSprite;
    private string mSpriteName;
    private Transform mTrans;
    public Camera uiCamera;

    private void Awake()
    {
        instance = this;
    }

    public static void Clear()
    {
        if ((instance != null) && (instance.mSprite != null))
        {
            Set(instance.mAtlas, instance.mSpriteName);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public static void Set(UIAtlas atlas, string sprite)
    {
        if ((instance != null) && (instance.mSprite != null))
        {
            instance.mSprite.atlas = atlas;
            instance.mSprite.spriteName = sprite;
            instance.mSprite.MakePixelPerfect();
            instance.Update();
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mSprite = base.GetComponentInChildren<UISprite>();
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        if (this.mSprite != null)
        {
            this.mAtlas = this.mSprite.atlas;
            this.mSpriteName = this.mSprite.spriteName;
            if (this.mSprite.depth < 100)
            {
                this.mSprite.depth = 100;
            }
        }
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (this.uiCamera != null)
        {
            mousePosition.x = Mathf.Clamp01(mousePosition.x / ((float) Screen.width));
            mousePosition.y = Mathf.Clamp01(mousePosition.y / ((float) Screen.height));
            this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
            if (this.uiCamera.orthographic)
            {
                Vector3 localPosition = this.mTrans.localPosition;
                localPosition.x = Mathf.Round(localPosition.x);
                localPosition.y = Mathf.Round(localPosition.y);
                this.mTrans.localPosition = localPosition;
            }
        }
        else
        {
            mousePosition.x -= Screen.width * 0.5f;
            mousePosition.y -= Screen.height * 0.5f;
            mousePosition.x = Mathf.Round(mousePosition.x);
            mousePosition.y = Mathf.Round(mousePosition.y);
            this.mTrans.localPosition = mousePosition;
        }
    }
}

