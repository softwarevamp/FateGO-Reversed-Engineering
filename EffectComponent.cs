using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class EffectComponent : BaseMonoBehaviour
{
    protected static int _RenderQueue = 0xfa0;
    [SerializeField]
    public float endtime = 5f;
    private System.Action FigureLoadCallback;
    private string filename;
    public UILabel label;
    public bool loop;
    public float losttime = 5f;
    public GameObject mstobject;
    private UIStandFigureR myStandFigure;
    public ParticleSystem[] particlelist;
    private Spawner spawner;
    public UITexture texture;
    private float totaltime;
    public bool uieffect;

    public void EndLoadAsset(AssetData loadData)
    {
        Debug.Log("EndLoadAsset:" + this.filename);
        foreach (string str in loadData.GetObjectNameList())
        {
            Debug.Log("name:" + str);
        }
        this.texture.mainTexture = loadData.GetObject<Texture2D>(this.filename);
    }

    public void Init()
    {
        if (this.uieffect)
        {
            Transform transform = base.transform;
            UISprite[] componentsInChildren = base.gameObject.GetComponentsInChildren<UISprite>();
            UILabel[] labelArray = base.gameObject.GetComponentsInChildren<UILabel>();
            if ((componentsInChildren == null) && (labelArray == null))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.GetComponent<Renderer>().material.renderQueue = _RenderQueue;
                }
            }
        }
    }

    private void OnDestroy()
    {
        Renderer component = base.GetComponent<Renderer>();
        if ((component != null) && (component.materials != null))
        {
            foreach (Material material in component.materials)
            {
                UnityEngine.Object.DestroyImmediate(material);
            }
        }
        if (this.texture != null)
        {
            Resources.UnloadAsset(this.texture.mainTexture);
            this.texture.mainTexture = null;
        }
    }

    private void onFigureAssetLoad()
    {
        if (this.FigureLoadCallback != null)
        {
            this.FigureLoadCallback();
        }
    }

    private void OnSpawn()
    {
        this.totaltime = 0f;
    }

    public void replaceFigure(GameObject obj)
    {
        Transform transform = base.transform.getNodeFromName("Texture", true);
        transform.GetComponent<UITexture>().enabled = false;
        UIStandFigureR component = obj.GetComponent<UIStandFigureR>();
        if (component != null)
        {
            component.transform.parent = transform;
            component.transform.localPosition = new Vector3(-280f, 475f, 0f);
            component.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            component.transform.localScale = Vector3.one;
            component.SetDepth(200);
            this.myStandFigure = component;
        }
    }

    public void setFigure(int svtId, int limit, System.Action callback = null)
    {
        Transform transform = base.transform.getNodeFromName("Texture", true);
        transform.GetComponent<UITexture>().enabled = false;
        this.FigureLoadCallback = callback;
        this.myStandFigure = StandFigureManager.CreateRenderPrefab(transform.gameObject, svtId, limit, Face.Type.CRY, 0, new System.Action(this.onFigureAssetLoad));
        this.myStandFigure.transform.localPosition = new Vector3(-280f, 475f, 0f);
        this.myStandFigure.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        this.myStandFigure.transform.localScale = Vector3.one;
        this.myStandFigure.SetDepth(200);
    }

    public void setLabel(string text)
    {
        if (this.label != null)
        {
            this.label.text = text;
        }
    }

    public void setTexture(string filename)
    {
        if (this.texture != null)
        {
            Texture2D textured = Resources.Load<Texture2D>(filename);
            this.texture.mainTexture = textured;
        }
    }

    private void Start()
    {
        this.spawner = SingletonMonoBehaviour<Spawner>.Instance;
        this.Init();
    }

    private void Update()
    {
        if (!this.loop)
        {
            this.totaltime += Time.deltaTime;
            if ((this.endtime + this.losttime) < this.totaltime)
            {
                if (this.spawner != null)
                {
                    this.spawner.Despawn(base.gameObject, true);
                }
                else
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }
            else if (this.endtime <= this.totaltime)
            {
                foreach (ParticleSystem system in this.particlelist)
                {
                    system.Stop();
                }
            }
        }
    }
}

