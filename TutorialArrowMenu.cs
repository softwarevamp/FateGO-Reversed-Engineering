using System;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowMenu : BaseMonoBehaviour
{
    [SerializeField]
    protected Transform baseArrow;
    [SerializeField]
    protected UIPanel basePanel;
    [SerializeField]
    protected Transform basePeepWindow;
    protected System.Action callbackFunc;
    protected static readonly float CLOSE_TIME = 0.1f;
    protected static readonly float MASK_ALPHA = 0.7f;
    [SerializeField]
    protected UISprite maskSprite;
    protected static readonly float OPEN_TIME = 0.3f;
    protected static readonly int PeepWindowCacheCount = 5;
    protected List<TutorialArrowMark> tutorialArrowMarkList = new List<TutorialArrowMark>();
    [SerializeField]
    protected GameObject tutorialArrowMarkPrefab;
    protected List<GameObject> tutorialPeepWindowTextureList = new List<GameObject>();
    [SerializeField]
    protected GameObject tutorialPeepWindowTexturePrefab;

    public void Close(System.Action callback)
    {
        this.callbackFunc = callback;
        UIPanel panel = (this.basePanel == null) ? base.gameObject.GetComponent<UIPanel>() : this.basePanel;
        if (panel != null)
        {
            panel.alpha = 1f;
            TweenAlpha alpha = TweenAlpha.Begin(panel.gameObject, CLOSE_TIME, 0f);
            if (alpha != null)
            {
                alpha.method = UITweener.Method.EaseOutQuad;
                alpha.eventReceiver = base.gameObject;
                alpha.callWhenFinished = "EndCloseTutorialArrowMenu";
                return;
            }
        }
        base.Invoke("EndCloseTutorialArrowMenu", 0.1f);
    }

    protected GameObject CreatePrefab(GameObject prefab, Transform parentObject)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        transform.parent = parentObject;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        return obj2;
    }

    public void EndCloseTutorialArrowMenu()
    {
        this.Init();
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    protected void EndOpenBaseDialog()
    {
        if (this.callbackFunc != null)
        {
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            callbackFunc();
        }
    }

    public void Init()
    {
        this.ObjectInitialize();
        base.gameObject.SetActive(false);
    }

    protected void ObjectInitialize()
    {
        if (this.tutorialArrowMarkList.Count > 0)
        {
            for (int i = 0; i < this.tutorialArrowMarkList.Count; i++)
            {
                UnityEngine.Object.Destroy(this.tutorialArrowMarkList[i].gameObject);
            }
            this.tutorialArrowMarkList.Clear();
        }
        if (this.tutorialPeepWindowTextureList.Count > 0)
        {
            foreach (GameObject obj2 in this.tutorialPeepWindowTextureList)
            {
                UnityEngine.Object.Destroy(obj2);
            }
            this.tutorialPeepWindowTextureList.Clear();
        }
    }

    public void Open(Vector2 pos, float way, Rect rect, System.Action func)
    {
        Vector2[] posList = new Vector2[] { pos };
        this.Open(posList, way, rect, func);
    }

    public void Open(Vector2 pos, float way, Rect[] rects, System.Action func)
    {
        Vector2[] posList = new Vector2[] { pos };
        this.Open(posList, way, rects, func);
    }

    public void Open(Vector2[] posList, float way, Rect[] rects, System.Action func)
    {
        float[] ways = new float[] { way };
        this.Open(posList, ways, rects, func);
    }

    public void Open(Vector2[] posList, float[] ways, Rect[] rects, System.Action func)
    {
        this.callbackFunc = func;
        base.gameObject.SetActive(true);
        this.ObjectInitialize();
        for (int i = 0; i < rects.Length; i++)
        {
            Rect rect = rects[i];
            GameObject item = this.CreatePrefab(this.tutorialPeepWindowTexturePrefab, this.basePeepWindow);
            UITexture component = item.GetComponent<UITexture>();
            component.transform.localPosition = new Vector3(rect.center.x, rect.center.y, 1f);
            component.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            component.width = 0x800;
            component.height = 0x480;
            TweenWidth width = TweenWidth.Begin(component, OPEN_TIME, ((int) rect.width) * 2);
            TweenHeight height = TweenHeight.Begin(component, OPEN_TIME, ((int) rect.height) * 2);
            width.method = UITweener.Method.EaseOutQuad;
            height.method = UITweener.Method.EaseOutQuad;
            this.tutorialPeepWindowTextureList.Add(item);
        }
        for (int j = 0; j < posList.Length; j++)
        {
            TutorialArrowMark mark = this.CreatePrefab(this.tutorialArrowMarkPrefab, this.baseArrow).GetComponent<TutorialArrowMark>();
            if ((1 < ways.Length) && (ways.Length <= posList.Length))
            {
                mark.Init(posList[j], ways[j]);
            }
            else
            {
                mark.Init(posList[j], ways[0]);
            }
            this.tutorialArrowMarkList.Add(mark);
        }
        UIPanel panel = (this.basePanel == null) ? base.gameObject.GetComponent<UIPanel>() : this.basePanel;
        if (panel != null)
        {
            panel.alpha = 1f;
        }
        this.maskSprite.alpha = 0f;
        TweenAlpha alpha = TweenAlpha.Begin(this.maskSprite.gameObject, OPEN_TIME, MASK_ALPHA);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseOutQuad;
            alpha.eventReceiver = base.gameObject;
            alpha.callWhenFinished = "EndOpenBaseDialog";
        }
        else
        {
            this.maskSprite.alpha = MASK_ALPHA;
            this.EndOpenBaseDialog();
        }
    }

    public void Open(Vector2[] posList, float way, Rect rect, System.Action func)
    {
        Rect[] rects = new Rect[] { rect };
        this.Open(posList, way, rects, func);
    }

    public bool IsBusy =>
        base.gameObject.activeSelf;
}

