using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TutorialBigDialog : BaseDialog
{
    protected int _loadIndex;
    protected string[] assetsImageLoadList;
    [SerializeField]
    protected GameObject CloseButton;
    protected System.Action closeCallbackFunc;
    protected int CurrentIndex;
    protected static readonly float FADE_TIME = 0.3f;
    protected UITexture FadeInTarget;
    protected UITexture FadeOutTarget;
    protected TutorialFlag.Id flagId;
    protected List<GameObject> ImagePageList;
    [SerializeField]
    protected GameObject ImageRoot;
    protected bool isButtonEnable;
    protected bool IsFading;
    protected bool IsInitialized;
    [SerializeField]
    protected GameObject NextButton;
    [SerializeField]
    protected GameObject PrevButton;
    protected TutorialFlag.ImageId[] TutorialImageLoadList;
    [SerializeField]
    protected GameObject TutorialImagePrefab;

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.isButtonEnable = false;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        foreach (GameObject obj2 in this.ImagePageList)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        if (this.TutorialImageLoadList != null)
        {
            foreach (TutorialFlag.ImageId id in this.TutorialImageLoadList)
            {
                AssetManager.releaseAssetStorage(this.GetTuorialImagePath((int) id));
            }
            this.TutorialImageLoadList = null;
        }
        if (this.assetsImageLoadList != null)
        {
            for (int i = 0; i < this.assetsImageLoadList.Length; i += 2)
            {
                AssetManager.releaseAssetStorage(this.assetsImageLoadList[i]);
            }
            this.assetsImageLoadList = null;
        }
        this.Init();
        base.gameObject.SetActive(false);
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    protected void EndTurorialRequest(string result)
    {
        this.Close(this.closeCallbackFunc);
    }

    protected string GetTuorialImageName(int imgId) => 
        ("tutorial_" + $"{imgId:D2}");

    protected string GetTuorialImagePath(int imgId) => 
        ("Tutorial/" + this.GetTuorialImageName(imgId));

    public void Init()
    {
        this.isButtonEnable = false;
        base.gameObject.SetActive(false);
        base.Init();
    }

    protected void LoadImages(string[] images)
    {
        this._loadIndex = 0;
        this.ImagePageList = new List<GameObject>();
        this.LoadStartAssets();
    }

    protected void LoadImages(TutorialFlag.ImageId[] images)
    {
        this._loadIndex = 0;
        this.ImagePageList = new List<GameObject>();
        this.LoadStart();
    }

    protected void LoadStart()
    {
        AssetManager.loadAssetStorage(this.GetTuorialImagePath((int) this.TutorialImageLoadList[this._loadIndex]), delegate (AssetData data) {
            string name = this.GetTuorialImageName((int) this.TutorialImageLoadList[this._loadIndex]);
            Texture2D textured = data.GetObject<Texture2D>(name);
            Texture2D texture = data.GetObject<Texture2D>(name + "_alpha");
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.TutorialImagePrefab);
            go.name = "Image" + this._loadIndex;
            UITexture component = go.GetComponent<UITexture>();
            UIDragScrollView view = go.GetComponent<UIDragScrollView>();
            NGUITools.SetLayer(go, base.gameObject.layer);
            Material material = new Material(Shader.Find("Custom/SpriteWithMask"));
            component.material = material;
            material.mainTexture = textured;
            material.SetTexture("_MaskTex", texture);
            go.SetActive(false);
            go.transform.parent = this.ImageRoot.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            this.ImagePageList.Add(go);
            this._loadIndex++;
            if (this._loadIndex >= this.TutorialImageLoadList.Length)
            {
                this.CurrentIndex = 0;
                this.UpdatePage(this.CurrentIndex);
                base.Invoke("OpenWindow", 0.3f);
            }
            else
            {
                this.LoadStart();
            }
        });
    }

    protected void LoadStartAssets()
    {
        AssetManager.loadAssetStorage(this.assetsImageLoadList[this._loadIndex], delegate (AssetData data) {
            Texture2D textured = data.GetObject<Texture2D>(this.assetsImageLoadList[this._loadIndex + 1]);
            Texture2D texture = data.GetObject<Texture2D>(this.assetsImageLoadList[this._loadIndex + 1] + "_alpha");
            GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.TutorialImagePrefab);
            go.name = "Image" + this._loadIndex;
            UITexture component = go.GetComponent<UITexture>();
            UIDragScrollView view = go.GetComponent<UIDragScrollView>();
            NGUITools.SetLayer(go, base.gameObject.layer);
            Material material = new Material(Shader.Find("Custom/SpriteWithMask"));
            component.material = material;
            material.mainTexture = textured;
            material.SetTexture("_MaskTex", texture);
            go.SetActive(false);
            go.transform.parent = this.ImageRoot.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            this.ImagePageList.Add(go);
            this._loadIndex += 2;
            if (this._loadIndex >= this.assetsImageLoadList.Length)
            {
                this.CurrentIndex = 0;
                this.UpdatePage(this.CurrentIndex);
                base.Invoke("OpenWindow", 0.3f);
            }
            else
            {
                this.LoadStartAssets();
            }
        });
    }

    public void OnBottomButton()
    {
        if (!this.IsFading && (this.CurrentIndex == (this.ImagePageList.Count - 1)))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.OnClickClose();
        }
    }

    public void OnClickClose()
    {
        if (!this.IsFading && this.isButtonEnable)
        {
            this.isButtonEnable = false;
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            if (this.flagId == TutorialFlag.Id.NULL)
            {
                this.Close(this.closeCallbackFunc);
            }
            else
            {
                NetworkManager.getRequest<TutorialSetRequest>(new NetworkManager.ResultCallbackFunc(this.EndTurorialRequest)).beginRequest(this.flagId);
            }
        }
    }

    public void OnNextButton()
    {
        if (!this.IsFading && (this.CurrentIndex != (this.ImagePageList.Count - 1)))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.UpdatePage(this.CurrentIndex + 1);
        }
    }

    protected void OnPageChange(int idx)
    {
        if ((idx != this.CurrentIndex) && ((idx >= 0) && (idx < this.ImagePageList.Count)))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
    }

    public void OnPrevButton()
    {
        if (!this.IsFading && (this.CurrentIndex > 0))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.UpdatePage(this.CurrentIndex - 1);
        }
    }

    public void Open(TutorialFlag.ImageId[] images, TutorialFlag.Id flagId = -1, System.Action func = null)
    {
        if (!this.IsInitialized)
        {
            this.Init();
            this.IsInitialized = true;
        }
        this.closeCallbackFunc = func;
        this.flagId = flagId;
        this.TutorialImageLoadList = images;
        this.isButtonEnable = false;
        base.SetMask(false);
        this.LoadImages(images);
        base.gameObject.SetActive(true);
        this.UpdateButtons(true);
    }

    protected void OpenWindow()
    {
        base.Open(new System.Action(this.EndOpen), false);
    }

    public void OpenWithAssets(string[] images, System.Action func = null)
    {
        if (!this.IsInitialized)
        {
            this.Init();
            this.IsInitialized = true;
        }
        this.closeCallbackFunc = func;
        this.flagId = TutorialFlag.Id.NULL;
        this.assetsImageLoadList = images;
        this.isButtonEnable = false;
        base.SetMask(false);
        this.LoadImages(images);
        base.gameObject.SetActive(true);
        this.UpdateButtons(true);
    }

    protected void UpdateButtons(bool isDisp = true)
    {
        if (!isDisp)
        {
            this.CloseButton.SetActive(false);
            this.NextButton.SetActive(false);
            this.PrevButton.SetActive(false);
        }
        else
        {
            if (this.CurrentIndex == (this.ImagePageList.Count - 1))
            {
                this.CloseButton.SetActive(true);
                this.NextButton.SetActive(false);
            }
            else
            {
                this.CloseButton.SetActive(false);
                this.NextButton.SetActive(true);
                this.NextButton.transform.GetComponentsInChildren<UILabel>(true)[0].text = LocalizationManager.Get("TUTORIAL_IMAGE_DIALOG_NEXT");
            }
            if (this.CurrentIndex > 0)
            {
                this.PrevButton.SetActive(true);
                this.PrevButton.transform.GetComponentsInChildren<UILabel>(true)[0].text = LocalizationManager.Get("TUTORIAL_IMAGE_DIALOG_PREV");
            }
            else
            {
                this.PrevButton.SetActive(false);
            }
        }
    }

    protected void UpdatePage(int dispIndex)
    {
        bool flag = dispIndex != this.CurrentIndex;
        this.FadeInTarget = null;
        this.FadeOutTarget = null;
        int count = this.ImagePageList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject obj2 = this.ImagePageList[i];
            UITexture component = obj2.GetComponent<UITexture>();
            if (dispIndex == i)
            {
                obj2.SetActive(true);
                if (flag)
                {
                    this.FadeInTarget = component;
                    component.alpha = 0f;
                }
                else
                {
                    component.alpha = 1f;
                }
            }
            else if (i == this.CurrentIndex)
            {
                if (flag)
                {
                    this.FadeOutTarget = component;
                    obj2.SetActive(true);
                }
                else
                {
                    obj2.SetActive(false);
                }
            }
            else
            {
                obj2.SetActive(false);
            }
        }
        this.CurrentIndex = dispIndex;
        if (!flag)
        {
            this.UpdateButtons(true);
        }
        else
        {
            this.IsFading = true;
            TweenAlpha alpha = TweenAlpha.Begin(this.FadeOutTarget.gameObject, FADE_TIME, 0f);
            TweenAlpha.Begin(this.FadeInTarget.gameObject, FADE_TIME, 1f).SetOnFinished(delegate {
                this.FadeOutTarget.gameObject.SetActive(false);
                this.IsFading = false;
                this.UpdateButtons(true);
            });
        }
    }
}

