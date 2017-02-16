using System;
using UnityEngine;

public class SampleAssetBundleScene : MonoBehaviour
{
    protected long downloadSize;
    protected State state;
    [SerializeField]
    protected UISprite testAtlasSprite;
    [SerializeField]
    protected GameObject testParentObject;

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.state = State.LOAD_ASSET_BUNDLE;
            string[] nameList = new string[] { "Test/Card001d", "Test/card002d.png", "Test/TestCube" };
            AssetManager.loadAssetStorage(nameList, new System.Action(this.LoadPackEnd));
            AssetManager.loadAssetStorage("Items/ItemAtlas", new AssetLoader.LoadEndDataHandler(this.LoadMatEnd));
            this.downloadSize = AssetManager.getDownloadSize();
            AssetManager.loadAssetStorage("Test/card003", new AssetLoader.LoadEndDataHandler(this.LoadPackEnd4));
            AssetManager.loadAssetStorage("Test/DependCommon", new AssetLoader.LoadEndDataHandler(this.LoadDependEnd));
        }
    }

    private void LoadDependEnd(AssetData data)
    {
        string[] nameList = new string[] { "Test/DependPrefab1", "Test/DependPrefab2", "Test/DependPrefab3" };
        AssetManager.loadAssetStorage(nameList, new System.Action(this.LoadDependEnd2));
    }

    private void LoadDependEnd2()
    {
        this.SetAssetPrefab("Test/DependPrefab1", 100f, 0f);
        this.SetAssetPrefab("Test/DependPrefab2", 200f, 0f);
        this.SetAssetPrefab("Test/DependPrefab3", 300f, 0f);
    }

    private void LoadMatEnd(AssetData data)
    {
        Debug.Log("LoadMatEnd " + data.Name);
        UIAtlas component = data.GetObject<GameObject>().GetComponent<UIAtlas>();
        this.testAtlasSprite.atlas = component;
        this.testAtlasSprite.spriteName = "100";
    }

    private void LoadPackEnd()
    {
        this.LoadPackEnd1(AssetManager.getAssetStorage("Test/Card001"));
        this.LoadPackEnd2(AssetManager.getAssetStorage("Test/card002p.png"));
        this.LoadPackEnd3(AssetManager.getAssetStorage("Test/TestCube"));
    }

    private void LoadPackEnd1(AssetData data)
    {
        Debug.Log("LoadPackEnd1 " + data.Name);
        GameObject obj2 = NGUITools.AddChild(this.testParentObject);
        obj2.name = "Test1";
        obj2.transform.localPosition = new Vector3(100f, -100f, 0f);
        UITexture texture = obj2.AddComponent<UITexture>();
        Texture2D textured = data.GetObject<Texture2D>("card001");
        texture.mainTexture = textured;
        texture.MarkAsChanged();
        texture.depth = 10;
    }

    private void LoadPackEnd2(AssetData data)
    {
        Debug.Log("LoadPackEnd2 " + data.Name);
        GameObject obj2 = NGUITools.AddChild(this.testParentObject);
        obj2.name = "Test2";
        obj2.transform.localPosition = new Vector3(200f, -100f, 0f);
        UITexture texture = obj2.AddComponent<UITexture>();
        Texture2D textured = data.GetObject<Texture2D>();
        texture.mainTexture = textured;
        texture.MarkAsChanged();
        texture.depth = 10;
    }

    private void LoadPackEnd3(AssetData data)
    {
        Debug.Log("LoadPackEnd3 " + data.Name);
        GameObject obj2 = UnityEngine.Object.Instantiate(data.GetObject()) as GameObject;
        obj2.transform.parent = this.testParentObject.transform;
        obj2.transform.localScale = new Vector3(10f, 10f, 10f);
        obj2.layer = this.testParentObject.layer;
    }

    private void LoadPackEnd4(AssetData data)
    {
        Debug.Log("LoadPackEnd3 " + data.Name);
        GameObject obj2 = NGUITools.AddChild(this.testParentObject);
        obj2.name = "Test4";
        obj2.transform.localPosition = new Vector3(300f, -100f, 0f);
        UITexture texture = obj2.AddComponent<UITexture>();
        Texture2D textured = data.GetObject() as Texture2D;
        texture.mainTexture = textured;
        texture.MarkAsChanged();
        texture.depth = 10;
    }

    private void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.LOAD_ASSET_BUNDLE:
                this.state = State.LOAD_MEMORY;
                break;

            case State.LOAD_MEMORY:
                this.state = State.IDLE;
                break;
        }
    }

    private void SetAssetPrefab(string name, float x, float y)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(AssetManager.getAssetStorage(name).GetObject()) as GameObject;
        obj2.transform.parent = this.testParentObject.transform;
        obj2.transform.localPosition = new Vector3(x, y, 0f);
        obj2.transform.localScale = Vector3.one;
        obj2.layer = this.testParentObject.layer;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if ((this.state != State.INIT) && (this.state == State.LOAD_ASSET_BUNDLE))
        {
            long num = AssetManager.getDownloadSize();
            Debug.Log($"download {((float) num) / ((float) this.downloadSize):p} [{num}/{this.downloadSize}]");
            if (num <= 0L)
            {
                this.state = State.LOAD_MEMORY;
            }
        }
    }

    protected enum State
    {
        INIT,
        LOAD_ASSET_BUNDLE,
        LOAD_MEMORY,
        IDLE
    }
}

