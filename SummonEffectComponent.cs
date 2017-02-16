using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonEffectComponent : MonoBehaviour
{
    private int _counter;
    private int _counterMax;
    private List<string> _releaseAssetPath;
    private GameObject bgParentObject;
    private readonly int[] cardTextureSize = new int[] { 0x200, 0x36b };
    private int DownloadCounter;
    public bool EditMode;
    private GameObject effect;
    private Transform firstTPeelr;
    private Transform firstTr;
    private GameObject prevObject;
    private static Dictionary<Rarity.TYPE, CardColor> rarityToColor;
    public List<SummonInfo> SummonInfos;
    public Texture2D[] testCards;

    public void AddSummonInfo(int servantId, int limitCount, bool isRankup, bool isNewCard, NoticeEffect noticeRarity, Rarity.TYPE rarity, CardType cardType)
    {
        Debug.Log(string.Concat(new object[] { "--**-- SummonEffect AddSummonInfo : ", servantId, ":", rarity }));
        if (this.SummonInfos == null)
        {
            this.Initialize();
        }
        this.SummonInfos.Add(new SummonInfo(servantId, limitCount, isRankup, isNewCard, noticeRarity, rarity, cardType));
    }

    private void ChangeClassCardColor(Transform cardNode, int col)
    {
        Texture2D textured = this.testCards[col];
        UITexture component = cardNode.GetComponent<UITexture>();
        component.mainTexture = textured;
        float height = ((float) this.cardTextureSize[1]) / 1024f;
        float x = 0f;
        component.uvRect = new Rect(x, 1f - height, 0.5f, height);
    }

    private void CompleteCallback()
    {
        this._counter++;
        if (this._counter >= this._counterMax)
        {
            base.GetComponent<PlayMakerFSM>().SendEvent("SETUP_DONE");
        }
    }

    public void endAnimation()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            if (this.effect != null)
            {
                UnityEngine.Object.Destroy(this.effect);
                this.effect = null;
            }
            base.GetComponent<PlayMakerFSM>().SendEvent("END_FADE");
        });
    }

    public void FadeIn()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }

    public void Initialize()
    {
        this.SummonInfos = new List<SummonInfo>();
    }

    public void InitSummonEffect()
    {
        base.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("countMax").Value = this.SummonInfos.Count;
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
        this.DownloadCounter = this.SummonInfos.Count;
        foreach (SummonInfo info in this.SummonInfos)
        {
            CharaGraphManager.DownloadAsset(info.ServantId, info.LimitCount, 1, new System.Action(this.OnCharaGraphLoadDone));
        }
    }

    protected void OnCharaGraphLoadDone()
    {
        this.DownloadCounter--;
        if (this.DownloadCounter == 0)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
            base.GetComponent<PlayMakerFSM>().SendEvent("INIT_DONE");
        }
    }

    public void ReleasePrevAsset()
    {
        if (this._releaseAssetPath != null)
        {
            foreach (string str in this._releaseAssetPath)
            {
                AssetManager.releaseAssetStorage(str);
            }
            this._releaseAssetPath.Clear();
        }
    }

    private void Start()
    {
        if (rarityToColor == null)
        {
            Dictionary<Rarity.TYPE, CardColor> dictionary = new Dictionary<Rarity.TYPE, CardColor> {
                { 
                    Rarity.TYPE.COMMON,
                    CardColor.BRONZE
                },
                { 
                    Rarity.TYPE.UNCOMMON,
                    CardColor.BRONZE
                },
                { 
                    Rarity.TYPE.RARE,
                    CardColor.SILVER
                },
                { 
                    Rarity.TYPE.SRARE,
                    CardColor.GOLD
                },
                { 
                    Rarity.TYPE.SSRARE,
                    CardColor.GOLD
                },
                { 
                    Rarity.TYPE.ACCESSORY,
                    CardColor.BRONZE
                },
                { 
                    Rarity.TYPE.SUB_EQUIP,
                    CardColor.BRONZE
                }
            };
            rarityToColor = dictionary;
        }
    }

    private void Update()
    {
    }

    public void UpdateCardParam()
    {
        PlayMakerFSM component = base.GetComponent<PlayMakerFSM>();
        int num = component.FsmVariables.GetFsmInt("countIndex").Value;
        int servantId = this.SummonInfos[num].ServantId;
        int limitCount = this.SummonInfos[num].LimitCount;
        bool isRankUp = this.SummonInfos[num].IsRankUp;
        bool isNewCard = this.SummonInfos[num].IsNewCard;
        NoticeEffect noticeRarity = this.SummonInfos[num].NoticeRarity;
        Rarity.TYPE rarity = this.SummonInfos[num].Rarity;
        CardType cardType = this.SummonInfos[num].CardType;
        component.FsmVariables.GetFsmInt("ServantId").Value = servantId;
        component.FsmVariables.GetFsmInt("LimitCount").Value = limitCount;
        component.FsmVariables.GetFsmBool("IsRankUp").Value = isRankUp;
        component.FsmVariables.GetFsmBool("IsNewCard").Value = isNewCard;
        component.FsmVariables.GetFsmInt("NoticeRarity").Value = (int) noticeRarity;
        component.FsmVariables.GetFsmInt("CardType").Value = (int) cardType;
        int num4 = 0;
        switch (rarity)
        {
            case Rarity.TYPE.SSRARE:
            case Rarity.TYPE.SRARE:
                num4 = 2;
                break;

            default:
                if (rarity == Rarity.TYPE.RARE)
                {
                    num4 = 1;
                }
                break;
        }
        component.FsmVariables.GetFsmInt("Rarity").Value = num4;
        if ((this.firstTr != null) && (this.firstTPeelr != null))
        {
            this.firstTr.gameObject.SetActive(false);
            this.firstTPeelr.gameObject.SetActive(false);
        }
    }

    public void UpdateCardTexture()
    {
        <UpdateCardTexture>c__AnonStoreyA5 ya = new <UpdateCardTexture>c__AnonStoreyA5 {
            <>f__this = this
        };
        PlayMakerFSM component = base.GetComponent<PlayMakerFSM>();
        int num = component.FsmVariables.GetFsmInt("countIndex").Value;
        int servantId = this.SummonInfos[num].ServantId;
        int limitCount = this.SummonInfos[num].LimitCount;
        bool isRankUp = this.SummonInfos[num].IsRankUp;
        bool isNewCard = this.SummonInfos[num].IsNewCard;
        NoticeEffect noticeRarity = this.SummonInfos[num].NoticeRarity;
        Rarity.TYPE rarity = this.SummonInfos[num].Rarity;
        ya.cardType = this.SummonInfos[num].CardType;
        this.ReleasePrevAsset();
        this._releaseAssetPath = new List<string>();
        component.FsmVariables.GetFsmInt("ServantId").Value = servantId;
        component.FsmVariables.GetFsmInt("LimitCount").Value = limitCount;
        component.FsmVariables.GetFsmBool("IsRankUp").Value = isRankUp;
        component.FsmVariables.GetFsmBool("IsNewCard").Value = isNewCard;
        component.FsmVariables.GetFsmInt("NoticeRarity").Value = (int) noticeRarity;
        component.FsmVariables.GetFsmInt("CardType").Value = (int) ya.cardType;
        int num4 = 0;
        switch (rarity)
        {
            case Rarity.TYPE.SSRARE:
            case Rarity.TYPE.SRARE:
                num4 = 2;
                break;

            default:
                if (rarity == Rarity.TYPE.RARE)
                {
                    num4 = 1;
                }
                break;
        }
        component.FsmVariables.GetFsmInt("Rarity").Value = num4;
        this.effect = component.FsmVariables.GetFsmGameObject("SummonEffect").Value;
        string nodename = component.FsmVariables.GetFsmString("CardNodeName").Value;
        float x = component.FsmVariables.GetFsmFloat("CardScale").Value;
        ya.cardFirstName = component.FsmVariables.GetFsmString("CardFirstName").Value;
        ya.cardFirstPeelName = component.FsmVariables.GetFsmString("CardFirstPeelName").Value;
        ya.cardSecondName = component.FsmVariables.GetFsmString("CardSecondName").Value;
        ya.cardRotName = component.FsmVariables.GetFsmString("CardRotName").Value;
        Debug.Log(string.Concat(new object[] { "SummonEffect:UpdateCardParam: servant:", servantId, " limitCount:", limitCount, " isRankUp:", isRankUp }));
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        transform.gameObject.SetActive(false);
        transform.gameObject.transform.localScale = Vector3.one;
        CardColor color = rarityToColor[rarity];
        if (color == CardColor.BRONZE)
        {
            isRankUp = false;
        }
        this._counter = 0;
        this._counterMax = !this.EditMode ? (!isRankUp ? 3 : 4) : 0;
        UICharaGraphTexture texture = null;
        if (this.prevObject != null)
        {
            UnityEngine.Object.Destroy(this.prevObject);
            this.prevObject = null;
        }
        texture = CharaGraphManager.CreateTexturePrefab(transform.gameObject, servantId, limitCount, true, true, 0, new System.Action(ya.<>m__188));
        texture.transform.localPosition = Vector3.zero;
        texture.transform.localScale = new Vector3(x, x, x);
        transform.GetComponent<UITexture>().enabled = false;
        this.prevObject = texture.gameObject;
        if (this.EditMode)
        {
            if (isRankUp)
            {
                component.FsmVariables.GetFsmInt("Rank").Value = (color != CardColor.GOLD) ? 1 : 2;
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardFirstName, true), ((int) color) - 1);
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardFirstPeelName, true), ((int) color) - 1);
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardRotName, true), ((int) color) - 1);
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardSecondName, true), (int) color);
            }
            else
            {
                component.FsmVariables.GetFsmInt("Rank").Value = 0;
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardRotName, true), (int) color);
                this.ChangeClassCardColor(this.effect.transform.getNodeFromName(ya.cardSecondName, true), (int) color);
            }
            this.CompleteCallback();
        }
        else
        {
            <UpdateCardTexture>c__AnonStoreyA6 ya2 = new <UpdateCardTexture>c__AnonStoreyA6 {
                <>f__this = this
            };
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(servantId);
            ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(servantId, limitCount);
            ServantClassEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(entity.classId);
            ya2.classCardId = entity3.imageId;
            Debug.LogError("CardColor : " + color);
            if (isRankUp)
            {
                <UpdateCardTexture>c__AnonStoreyA7 ya3 = new <UpdateCardTexture>c__AnonStoreyA7 {
                    <>f__ref$165 = ya,
                    <>f__ref$166 = ya2,
                    <>f__this = this
                };
                Rarity.TYPE type2 = rarity;
                int num6 = Rarity.getLowerColorRarity((int) type2);
                component.FsmVariables.GetFsmInt("Rank").Value = (color != CardColor.GOLD) ? 1 : 2;
                string designCardPath = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(ya2.classCardId, (Rarity.TYPE) num6);
                this._releaseAssetPath.Add(designCardPath);
                AssetManager.loadAssetStorage(designCardPath, new AssetLoader.LoadEndDataHandler(ya3.<>m__189));
                ya3.classBackId = ConstantMaster.getValue("BACKSIDE_CLASS_IMAGE_ID");
                designCardPath = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(ya3.classBackId, (Rarity.TYPE) num6);
                this._releaseAssetPath.Add(designCardPath);
                AssetManager.loadAssetStorage(designCardPath, new AssetLoader.LoadEndDataHandler(ya3.<>m__18A));
                designCardPath = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(ya2.classCardId, type2);
                this._releaseAssetPath.Add(designCardPath);
                AssetManager.loadAssetStorage(designCardPath, new AssetLoader.LoadEndDataHandler(ya3.<>m__18B));
            }
            else
            {
                <UpdateCardTexture>c__AnonStoreyA8 ya4 = new <UpdateCardTexture>c__AnonStoreyA8 {
                    <>f__ref$165 = ya,
                    <>f__ref$166 = ya2,
                    <>f__this = this
                };
                Rarity.TYPE type3 = rarity;
                component.FsmVariables.GetFsmInt("Rank").Value = 0;
                string item = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(ya2.classCardId, type3);
                this._releaseAssetPath.Add(item);
                AssetManager.loadAssetStorage(item, new AssetLoader.LoadEndDataHandler(ya4.<>m__18C));
                switch (ya.cardType)
                {
                    case CardType.CANCER:
                        ya4.backImageId = ConstantMaster.getValue("BACKSIDE_SVT_EQUIP_IMAGE_ID");
                        break;

                    default:
                        ya4.backImageId = ConstantMaster.getValue("BACKSIDE_CLASS_IMAGE_ID");
                        break;
                }
                item = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(ya4.backImageId, type3);
                this._releaseAssetPath.Add(item);
                AssetManager.loadAssetStorage(item, new AssetLoader.LoadEndDataHandler(ya4.<>m__18D));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateCardTexture>c__AnonStoreyA5
    {
        internal SummonEffectComponent <>f__this;
        internal string cardFirstName;
        internal string cardFirstPeelName;
        internal string cardRotName;
        internal string cardSecondName;
        internal SummonEffectComponent.CardType cardType;

        internal void <>m__188()
        {
            Debug.LogError("CreateCard");
            this.<>f__this.CompleteCallback();
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateCardTexture>c__AnonStoreyA6
    {
        internal SummonEffectComponent <>f__this;
        internal int classCardId;
    }

    [CompilerGenerated]
    private sealed class <UpdateCardTexture>c__AnonStoreyA7
    {
        internal SummonEffectComponent.<UpdateCardTexture>c__AnonStoreyA5 <>f__ref$165;
        internal SummonEffectComponent.<UpdateCardTexture>c__AnonStoreyA6 <>f__ref$166;
        internal SummonEffectComponent <>f__this;
        internal int classBackId;

        internal void <>m__189(AssetData d)
        {
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardFirstName, true), this.<>f__ref$166.classCardId);
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardFirstPeelName, true), this.<>f__ref$166.classCardId);
            Debug.LogError("rank up");
            this.<>f__this.CompleteCallback();
        }

        internal void <>m__18A(AssetData d)
        {
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardRotName, true), this.classBackId);
            this.<>f__this.CompleteCallback();
        }

        internal void <>m__18B(AssetData d)
        {
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardSecondName, true), this.<>f__ref$166.classCardId);
            this.<>f__this.CompleteCallback();
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateCardTexture>c__AnonStoreyA8
    {
        internal SummonEffectComponent.<UpdateCardTexture>c__AnonStoreyA5 <>f__ref$165;
        internal SummonEffectComponent.<UpdateCardTexture>c__AnonStoreyA6 <>f__ref$166;
        internal SummonEffectComponent <>f__this;
        internal int backImageId;

        internal void <>m__18C(AssetData d)
        {
            this.<>f__this.firstTr = this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardFirstName, true);
            this.<>f__this.firstTPeelr = this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardFirstPeelName, true);
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.firstTr, this.<>f__ref$166.classCardId);
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.firstTPeelr, this.<>f__ref$166.classCardId);
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardSecondName, true), this.<>f__ref$166.classCardId);
            Debug.LogError("no rank up");
            this.<>f__this.CompleteCallback();
        }

        internal void <>m__18D(AssetData d)
        {
            Transform cardNode = this.<>f__this.effect.transform.getNodeFromName(this.<>f__ref$165.cardRotName, true);
            SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(d, cardNode, this.backImageId);
            UITexture component = cardNode.GetComponent<UITexture>();
            string name = (this.<>f__ref$165.cardType != SummonEffectComponent.CardType.CANCER) ? "Unlit/Transparent Colored" : "Custom/Sprite-MasterFigure (SoftClip)";
            Material material = new Material(Shader.Find(name));
            Texture mainTexture = component.mainTexture;
            component.material = material;
            material.mainTexture = mainTexture;
            this.<>f__this.CompleteCallback();
        }
    }

    public enum CardColor
    {
        BRONZE,
        SILVER,
        GOLD
    }

    public enum CardType
    {
        OTHER,
        CANCER,
        SERVANT
    }

    public enum NoticeEffect
    {
        NONE,
        SR,
        SSR
    }

    [Serializable]
    public class SummonInfo
    {
        public SummonEffectComponent.CardType CardType;
        public bool IsNewCard;
        public bool IsRankUp;
        public int LimitCount;
        public SummonEffectComponent.NoticeEffect NoticeRarity;
        public Rarity.TYPE Rarity;
        public int ServantId;

        public SummonInfo(int servantId, int limitCount, bool isRankup, bool isNewCard, SummonEffectComponent.NoticeEffect noticeRarity, Rarity.TYPE rarity, SummonEffectComponent.CardType cardType)
        {
            this.ServantId = servantId;
            this.LimitCount = limitCount;
            this.IsRankUp = isRankup;
            this.IsNewCard = isNewCard;
            this.NoticeRarity = noticeRarity;
            this.Rarity = rarity;
            this.CardType = cardType;
        }
    }
}

