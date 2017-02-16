using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleIlluminationManager : SingletonMonoBehaviour<TitleIlluminationManager>
{
    private static int[,,] DirTable;
    [SerializeField]
    protected float FadeSpeed = 1f;
    protected int IlluminationCount;
    [SerializeField]
    protected int IlluminationCountMax = 2;
    [SerializeField]
    protected List<GameObject> IlluminationDisp;
    protected static readonly int IlluminationHeight = 0x22;
    protected IlluminationInfo[] IlluminationObjects;
    [SerializeField]
    protected List<GameObject> IlluminationPool;
    [SerializeField]
    protected static readonly int IlluminationPoolCount = 30;
    [SerializeField]
    protected int[] IlluminationPopFrames;
    [SerializeField]
    protected int[] IlluminationPopProb;
    [SerializeField]
    protected GameObject IlluminationPrefab;
    protected static readonly int IlluminationSize = 0x23;
    protected static readonly int IlluminationWidth = 30;
    [SerializeField]
    protected float MoveSpeed = 0.03f;
    protected float PopTime;
    protected bool Spawn;
    [SerializeField]
    protected UIPanel TargetPanel;

    protected void CreateIllumination()
    {
        foreach (IlluminationInfo info in this.IlluminationObjects)
        {
            if (!info.exists)
            {
                if (this.IlluminationPool.Count != 0)
                {
                    int x = UnityEngine.Random.Range((int) (-IlluminationWidth / 2), (int) (IlluminationWidth / 2));
                    int y = UnityEngine.Random.Range((int) (-IlluminationHeight / 2), (int) (IlluminationHeight / 2));
                    info.x = x;
                    info.y = y;
                    info.restCount = UnityEngine.Random.Range(6, 9);
                    info.moveTime = 0f;
                    info.exists = true;
                    this.SpawnIllumination(x, y);
                    this.IlluminationCount++;
                }
                return;
            }
        }
    }

    public void Initialize()
    {
        this.IlluminationObjects = new IlluminationInfo[this.IlluminationCountMax];
        for (int i = 0; i < this.IlluminationCountMax; i++)
        {
            this.IlluminationObjects[i] = new IlluminationInfo();
        }
        this.IlluminationPool = new List<GameObject>();
        for (int j = 0; j < IlluminationPoolCount; j++)
        {
            GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.IlluminationPrefab);
            item.transform.parent = this.TargetPanel.transform;
            item.transform.localPosition = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.SetActive(false);
            this.IlluminationPool.Add(item);
        }
        this.IlluminationDisp = new List<GameObject>();
        this.PopTime = 0f;
        DirTable = new int[,,] { { { -1, -1 }, { 0, -1 }, { -1, 1 }, { 0, 1 } }, { { 0, -1 }, { 1, -1 }, { 0, 1 }, { 1, 1 } } };
    }

    public void ReturnIllumination(TitleIlluminationComponent obj)
    {
        obj.gameObject.SetActive(false);
        this.IlluminationDisp.Remove(obj.gameObject);
        this.IlluminationPool.Add(obj.gameObject);
    }

    protected void SpawnIllumination(int x, int y)
    {
        if (this.IlluminationPool.Count != 0)
        {
            GameObject obj2 = this.IlluminationPool[0];
            this.IlluminationPool.RemoveAt(0);
            TitleIlluminationComponent component = obj2.GetComponent<TitleIlluminationComponent>();
            component.Setup(x, y, IlluminationSize, this.FadeSpeed, this);
            component.gameObject.SetActive(true);
            this.IlluminationDisp.Add(component.gameObject);
        }
    }

    private void Start()
    {
        this.Initialize();
        this.StartEffect();
    }

    public void StartEffect()
    {
        this.Spawn = true;
    }

    private void Update()
    {
        this.UpdateIllumination(Time.deltaTime);
        float popTime = this.PopTime;
        this.PopTime += Time.deltaTime;
        if (this.Spawn && (this.IlluminationCount < this.IlluminationCountMax))
        {
            for (int i = 0; i < this.IlluminationPopFrames.Length; i++)
            {
                int num3 = this.IlluminationPopFrames[i];
                int num4 = (int) (popTime / (((float) num3) / 60f));
                int num5 = (int) (this.PopTime / (((float) num3) / 60f));
                if (num4 != num5)
                {
                    int num6 = this.IlluminationPopProb[i];
                    if (UnityEngine.Random.Range(0, 100) < num6)
                    {
                        this.CreateIllumination();
                    }
                }
            }
        }
    }

    protected void UpdateIllumination(float delta)
    {
        foreach (IlluminationInfo info in this.IlluminationObjects)
        {
            if (!info.exists)
            {
                continue;
            }
            info.moveTime += delta;
            if (info.moveTime >= this.MoveSpeed)
            {
                info.moveTime = 0f;
                int num2 = 10;
                int x = info.x;
                int y = info.y;
                while (num2 > 0)
                {
                    num2--;
                    int num5 = UnityEngine.Random.Range(0, 3);
                    int num6 = DirTable[info.y & 1, num5, 0];
                    int num7 = DirTable[info.y & 1, num5, 1];
                    x = info.x + num6;
                    y = info.y + num7;
                    if ((((((x != info.prevX) || (y != info.prevY)) && (x >= (-IlluminationWidth / 2))) && (x <= (IlluminationWidth / 2))) && (y >= (-IlluminationHeight / 2))) && (y <= (IlluminationHeight / 2)))
                    {
                        break;
                    }
                }
                info.prevX = info.x;
                info.prevY = info.y;
                info.x = x;
                info.y = y;
                this.SpawnIllumination(info.x, info.y);
                info.restCount--;
                if (info.restCount <= 0)
                {
                    this.IlluminationCount--;
                    info.exists = false;
                }
            }
        }
    }

    protected class IlluminationInfo
    {
        public bool exists;
        public float moveTime;
        public int prevX;
        public int prevY;
        public int restCount;
        public int x;
        public int y;
    }
}

