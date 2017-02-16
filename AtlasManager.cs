using System;
using UnityEngine;

public class AtlasManager : SingletonMonoBehaviour<AtlasManager>
{
    protected System.Action AtlasLoadCallback;
    public const string BANNER_ATLAS_PATH = "Banner/DownloadBanner";
    public const string BANNER_ICON_SPNAME_PREFIX = "banner_icon_";
    public const string BANNER_NOTICE_SPNAME_PREFIX = "banner_notice_";
    public const string BANNER_SPNAME_PREFIX = "banner_";
    protected AssetData bannerAssetData;
    [SerializeField]
    protected UIAtlas bannerAtlas;
    protected UIAtlas[] bannerDownloadAtlasList;
    protected static string[] bannerFileList = new string[] { "Banner/DownloadBanner", "ShopBanners/DownloadShopBanner" };
    protected System.Action bannerLoadCallbackFunc;
    private static readonly string[] baseFileList = new string[] { "listframes00_bg", "listframes1_bg", "listframes2_bg", "listframes3_bg", string.Empty, "listframes00_bg" };
    [SerializeField]
    protected UIAtlas buffIconAtlas;
    protected UIAtlas[] buffIconDownloadAtlasList;
    protected UIAtlas charaGraphOptionDownloadAtlas;
    [SerializeField]
    protected UIAtlas classIconsAtlas;
    protected UIAtlas classIconsDownloadAtlas;
    [SerializeField]
    protected UIAtlas commonAtlas;
    [SerializeField]
    protected UIAtlas equipFaceAtlas;
    protected UIAtlas[] equipFaceAtlasList;
    [SerializeField]
    protected UIAtlas faceAtlas;
    protected UIAtlas[] faceDecolorizationAtlasList;
    protected UIAtlas[] faceNormalAtlasList;
    private static readonly string[] formationBaseFileList = new string[] { "formation_framebg_0", "formation_framebg_1", "formation_framebg_2", "formation_framebg_3", "formation_framebg_0", "formation_blank_01", "formation_support" };
    private static readonly string[] formationFrameFileList = new string[] { "formation_frame_0", "formation_frame_1", "formation_frame_2", "formation_frame_3", "formation_frame_0", string.Empty, string.Empty };
    private static readonly string[] formationFrameForSupportSelectFileList = new string[] { "formation_frame_support0", "formation_frame_support1", "formation_frame_support2", "formation_frame_support3", "formation_blank_01", string.Empty, string.Empty };
    protected bool isBannerLoad;
    protected bool isBusy;
    [SerializeField]
    protected UIAtlas itemAtlas;
    protected UIAtlas[] itemDownloadAtlasList;
    [SerializeField]
    protected UIAtlas markAtlas;
    protected UIAtlas markDownloadAtlas;
    public const string SHOP_BANNER_ATLAS_PATH = "ShopBanners/DownloadShopBanner";
    protected AssetData shopBannerAssetData;
    protected UIAtlas[] shopBannerDownloadAtlasList;
    [SerializeField]
    protected UIAtlas skillIconAtlas;
    protected UIAtlas[] skillIconDownloadAtlasList;

    public static void Initialize()
    {
        AtlasManager instance = SingletonMonoBehaviour<AtlasManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    protected void InitializeLocal()
    {
        this.isBusy = true;
        this.itemDownloadAtlasList = null;
        this.classIconsDownloadAtlas = null;
        this.skillIconDownloadAtlasList = null;
        this.buffIconDownloadAtlasList = null;
        this.markDownloadAtlas = null;
        this.equipFaceAtlasList = null;
        this.faceNormalAtlasList = null;
        this.faceDecolorizationAtlasList = null;
        this.charaGraphOptionDownloadAtlas = null;
        this.isBannerLoad = false;
        this.bannerLoadCallbackFunc = null;
        this.bannerAssetData = null;
        this.bannerDownloadAtlasList = null;
        this.shopBannerAssetData = null;
        this.shopBannerDownloadAtlasList = null;
        if (!AssetManager.loadAssetStorage("Items/DownloadItem", new AssetLoader.LoadEndDataHandler(this.LoadItemEnd)))
        {
            this.LoadItemEnd(null);
        }
    }

    public static bool IsBusyInitialize()
    {
        AtlasManager instance = SingletonMonoBehaviour<AtlasManager>.Instance;
        if (instance == null)
        {
            return false;
        }
        return instance.isBusy;
    }

    public static bool IsExistShopBanner(int bannerId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.IsExistShopBannerLocal("shop_event_menu_" + bannerId);

    protected bool IsExistShopBannerLocal(string bannerName)
    {
        if (string.IsNullOrEmpty(bannerName))
        {
            return true;
        }
        if (this.shopBannerDownloadAtlasList != null)
        {
            int length = this.shopBannerDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.shopBannerDownloadAtlasList[i];
                if (atlas.GetSprite(bannerName) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void LoadBanner(System.Action callback)
    {
        SingletonMonoBehaviour<AtlasManager>.Instance.LoadBannerLocal(callback);
    }

    protected void LoadBannerEndLocal()
    {
        if (this.isBannerLoad)
        {
            this.isBannerLoad = false;
            this.bannerAssetData = AssetManager.getAssetStorage("Banner/DownloadBanner");
            GameObject[] objectList = this.bannerAssetData.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = this.bannerAssetData.GetObject<GameObject>("DownloadBannerAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.bannerDownloadAtlasList = atlasArray;
            }
            this.shopBannerAssetData = AssetManager.getAssetStorage("ShopBanners/DownloadShopBanner");
            GameObject[] objArray2 = this.shopBannerAssetData.GetObjectList<GameObject>();
            if ((objArray2 != null) && (objArray2.Length > 0))
            {
                int num3 = objArray2.Length;
                UIAtlas[] atlasArray2 = new UIAtlas[num3];
                for (int j = 0; j < num3; j++)
                {
                    atlasArray2[j] = this.shopBannerAssetData.GetObject<GameObject>("DownloadShopBannerAtlas" + (j + 1)).GetComponent<UIAtlas>();
                }
                this.shopBannerDownloadAtlasList = atlasArray2;
            }
        }
        System.Action bannerLoadCallbackFunc = this.bannerLoadCallbackFunc;
        if (bannerLoadCallbackFunc != null)
        {
            this.bannerLoadCallbackFunc = null;
            bannerLoadCallbackFunc();
        }
    }

    protected void LoadBannerLocal(System.Action callback)
    {
        this.bannerLoadCallbackFunc = (System.Action) Delegate.Combine(this.bannerLoadCallbackFunc, callback);
        if (this.bannerAssetData == null)
        {
            if (this.isBannerLoad)
            {
                return;
            }
            this.isBannerLoad = true;
            if (AssetManager.loadAssetStorage(bannerFileList, new System.Action(this.LoadBannerEndLocal)))
            {
                return;
            }
            this.isBannerLoad = false;
        }
        this.LoadBannerEndLocal();
    }

    public void LoadBuffIconAtlas(System.Action callback)
    {
        this.AtlasLoadCallback = callback;
        if (!AssetManager.loadAssetStorage("BuffIcons/DownloadBuffIcon", new AssetLoader.LoadEndDataHandler(this.LoadBuffIconEnd)))
        {
            this.LoadBuffIconEnd(null);
        }
    }

    protected void LoadBuffIconEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = data.GetObject<GameObject>("DownloadBuffIconAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.buffIconDownloadAtlasList = atlasArray;
            }
        }
        if (this.AtlasLoadCallback != null)
        {
            this.AtlasLoadCallback();
            this.AtlasLoadCallback = null;
        }
    }

    protected void LoadCharaGraphEnd(AssetData data)
    {
        if (data != null)
        {
            this.charaGraphOptionDownloadAtlas = data.GetObject<GameObject>("CharaGraphOptionAtlas").GetComponent<UIAtlas>();
        }
        this.isBusy = false;
    }

    protected void LoadClassIconsEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                UIAtlas component = new UIAtlas();
                component = data.GetObject<GameObject>("DownloadClassIconsAtlas").GetComponent<UIAtlas>();
                this.classIconsDownloadAtlas = component;
            }
        }
        if (!AssetManager.loadAssetStorage("SkillIcons/DownloadSkillIcon", new AssetLoader.LoadEndDataHandler(this.LoadSkillIconEnd)))
        {
            this.LoadSkillIconEnd(null);
        }
    }

    protected void LoadEquipFaceEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = data.GetObject<GameObject>("DownloadEquipFaceAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.equipFaceAtlasList = atlasArray;
            }
        }
        if (!AssetManager.loadAssetStorage("Faces/DownloadFace", new AssetLoader.LoadEndDataHandler(this.LoadFaceEnd)))
        {
            this.LoadFaceEnd(null);
        }
    }

    protected void LoadFaceEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                UIAtlas[] atlasArray2 = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = data.GetObject<GameObject>("DownloadFaceAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.faceNormalAtlasList = atlasArray;
            }
        }
        if (!AssetManager.loadAssetStorage("CharaGraphOption/CharaGraphOption", new AssetLoader.LoadEndDataHandler(this.LoadCharaGraphEnd)))
        {
            this.LoadCharaGraphEnd(null);
        }
    }

    protected void LoadItemEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = data.GetObject<GameObject>("DownloadItemAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.itemDownloadAtlasList = atlasArray;
            }
        }
        if (!AssetManager.loadAssetStorage("ClassIcons/DownloadClassIcons", new AssetLoader.LoadEndDataHandler(this.LoadClassIconsEnd)))
        {
            this.LoadClassIconsEnd(null);
        }
    }

    protected void LoadMarkEnd(AssetData data)
    {
        if (data != null)
        {
            this.markDownloadAtlas = data.GetObject<GameObject>().GetComponent<UIAtlas>();
        }
        if (!AssetManager.loadAssetStorage("EquipFaces/DownloadEquipFace", new AssetLoader.LoadEndDataHandler(this.LoadEquipFaceEnd)))
        {
            this.LoadEquipFaceEnd(null);
        }
    }

    protected void LoadSkillIconEnd(AssetData data)
    {
        if (data != null)
        {
            GameObject[] objectList = data.GetObjectList<GameObject>();
            if ((objectList != null) && (objectList.Length > 0))
            {
                int length = objectList.Length;
                UIAtlas[] atlasArray = new UIAtlas[length];
                for (int i = 0; i < length; i++)
                {
                    atlasArray[i] = data.GetObject<GameObject>("DownloadSkillIconAtlas" + (i + 1)).GetComponent<UIAtlas>();
                }
                this.skillIconDownloadAtlasList = atlasArray;
            }
        }
        if (!AssetManager.loadAssetStorage("Marks/DownloadMark", new AssetLoader.LoadEndDataHandler(this.LoadMarkEnd)))
        {
            this.LoadMarkEnd(null);
        }
    }

    public static void Reboot()
    {
        AtlasManager instance = SingletonMonoBehaviour<AtlasManager>.Instance;
        if (instance != null)
        {
            instance.RebootLocal();
        }
    }

    protected void RebootLocal()
    {
    }

    public static void ReleaseBanner()
    {
        SingletonMonoBehaviour<AtlasManager>.Instance.ReleaseBannerLocal();
    }

    protected void ReleaseBannerLocal()
    {
        if (this.isBannerLoad)
        {
            this.isBannerLoad = false;
            AssetManager.releaseAssetStorage(bannerFileList);
        }
        else
        {
            if (this.bannerAssetData != null)
            {
                AssetManager.releaseAsset(this.bannerAssetData);
            }
            if (this.shopBannerAssetData != null)
            {
                AssetManager.releaseAsset(this.shopBannerAssetData);
            }
        }
        this.bannerDownloadAtlasList = null;
        this.bannerAssetData = null;
        this.shopBannerDownloadAtlasList = null;
        this.shopBannerAssetData = null;
    }

    public void ReleaseNoneResidentAtlas()
    {
        this.ReleaseBannerLocal();
    }

    public static bool SetBanner(UISprite sprite, EventEntity eventEntity)
    {
        string bannerName = "banner_" + eventEntity.getEventBannerID();
        if (NetworkManager.getTime() < eventEntity.getEventStartedAt())
        {
            bannerName = "banner_notice_" + eventEntity.noticeBannerId;
        }
        return SetBanner(sprite, bannerName);
    }

    public static bool SetBanner(UISprite sprite, string bannerName) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetBannerLocal(sprite, bannerName);

    public static bool SetBannerIcon(UISprite sprite, EventEntity eventEntity)
    {
        string bannerName = "banner_icon_" + eventEntity.getEventIconID();
        return SetBanner(sprite, bannerName);
    }

    protected bool SetBannerLocal(UISprite sprite, string bannerName)
    {
        if (string.IsNullOrEmpty(bannerName))
        {
            sprite.spriteName = null;
            return true;
        }
        if (this.bannerDownloadAtlasList != null)
        {
            int length = this.bannerDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.bannerDownloadAtlasList[i];
                if (atlas.GetSprite(bannerName) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = bannerName;
                    return true;
                }
            }
        }
        sprite.spriteName = null;
        return false;
    }

    protected bool SetBuffIconByIconIdLocal(UISprite sprite, int buffIconId)
    {
        string name = $"bufficon_{buffIconId:000}";
        if (this.buffIconDownloadAtlasList != null)
        {
            foreach (UIAtlas atlas in this.buffIconDownloadAtlasList)
            {
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
            sprite.atlas = this.buffIconAtlas;
        }
        else
        {
            sprite.atlas = this.buffIconAtlas;
            if (sprite.atlas.GetSprite(name) != null)
            {
                sprite.spriteName = name;
                return true;
            }
        }
        sprite.spriteName = "bufficon_300";
        return false;
    }

    protected bool SetBuffIconLocal(UISprite sprite, int buffId)
    {
        BuffEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.BUFF).getEntityFromId<BuffEntity>(buffId);
        int buffIconId = (entity == null) ? 300 : entity.iconId;
        if (buffIconId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        return this.SetBuffIconByIconIdLocal(sprite, buffIconId);
    }

    public static void SetCharaGraphaOption(UISprite sprite, string optionName)
    {
        SingletonMonoBehaviour<AtlasManager>.Instance.SetCharaGraphaOptionLocal(sprite, optionName);
    }

    protected void SetCharaGraphaOptionLocal(UISprite sprite, string optionName)
    {
        if (this.charaGraphOptionDownloadAtlas != null)
        {
            sprite.atlas = this.charaGraphOptionDownloadAtlas;
            sprite.spriteName = optionName;
        }
    }

    public static void SetClass(UISprite sprite, int classId, int frameType)
    {
        ServantClassEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(classId);
        if (entity != null)
        {
            object[] objArray1 = new object[] { "class", frameType, "_", entity.iconImageId };
            SingletonMonoBehaviour<AtlasManager>.Instance.SetClassIconLocal(sprite, string.Concat(objArray1));
        }
        else
        {
            sprite.spriteName = null;
        }
    }

    public static bool SetClassIcon(UISprite sprite, int iconImageId, int frameType) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetClassIconLocal(sprite, "class" + frameType.ToString() + "_" + iconImageId.ToString());

    protected bool SetClassIconLocal(UISprite sprite, string classIconName)
    {
        if (string.IsNullOrEmpty(classIconName))
        {
            sprite.spriteName = null;
            return true;
        }
        if (this.classIconsDownloadAtlas != null)
        {
            UIAtlas classIconsDownloadAtlas = this.classIconsDownloadAtlas;
            if (classIconsDownloadAtlas.GetSprite(classIconName) != null)
            {
                sprite.atlas = classIconsDownloadAtlas;
                sprite.spriteName = classIconName;
                return true;
            }
            sprite.atlas = this.classIconsAtlas;
        }
        else
        {
            sprite.atlas = this.classIconsAtlas;
            if (sprite.atlas.GetSprite(classIconName) != null)
            {
                sprite.spriteName = classIconName;
                return true;
            }
        }
        sprite.spriteName = "class_097";
        return false;
    }

    public static bool SetClassTextIcon(UISprite sprite, int iconImageId, int frameType) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetClassTextIconLocal(sprite, "class" + frameType.ToString() + "_txt_" + iconImageId.ToString());

    protected bool SetClassTextIconLocal(UISprite sprite, string classIconName)
    {
        if (string.IsNullOrEmpty(classIconName))
        {
            sprite.spriteName = null;
            return true;
        }
        if (this.classIconsDownloadAtlas != null)
        {
            UIAtlas classIconsDownloadAtlas = this.classIconsDownloadAtlas;
            if (classIconsDownloadAtlas.GetSprite(classIconName) != null)
            {
                sprite.atlas = classIconsDownloadAtlas;
                sprite.spriteName = classIconName;
                return true;
            }
            sprite.atlas = this.classIconsAtlas;
        }
        else
        {
            sprite.atlas = this.classIconsAtlas;
            if (sprite.atlas.GetSprite(classIconName) != null)
            {
                sprite.spriteName = classIconName;
                return true;
            }
        }
        sprite.spriteName = "class1_txt_12";
        return false;
    }

    public static void SetCommon(UISprite sprite)
    {
        SingletonMonoBehaviour<AtlasManager>.Instance.SetCommonLocal(sprite);
    }

    public void SetCommonLocal(UISprite sprite)
    {
        sprite.atlas = this.commonAtlas;
    }

    public static bool SetEquipFace(UISprite sprite, int svtId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetEquipFaceLocal(sprite, svtId);

    protected bool SetEquipFaceLocal(UISprite sprite, int svtId)
    {
        if (svtId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        string name = "f_" + svtId.ToString() + "0";
        if (this.equipFaceAtlasList != null)
        {
            int length = this.equipFaceAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.equipFaceAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
        }
        sprite.atlas = this.equipFaceAtlas;
        if (sprite.atlas.GetSprite(name) != null)
        {
            sprite.spriteName = name;
            return true;
        }
        sprite.spriteName = "f_1000000";
        return false;
    }

    public static bool SetEquipItem(UISprite sprite, int equipItemImageId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetEquipItemLocal(sprite, equipItemImageId);

    protected bool SetEquipItemLocal(UISprite sprite, int equipItemImageId)
    {
        if (equipItemImageId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        string name = "masterequip" + $"{equipItemImageId:D5}";
        if (this.itemDownloadAtlasList != null)
        {
            int length = this.itemDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.itemDownloadAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
            sprite.atlas = this.itemAtlas;
        }
        else
        {
            sprite.atlas = this.itemAtlas;
            if (sprite.atlas.GetSprite(name) != null)
            {
                sprite.spriteName = name;
                return true;
            }
        }
        sprite.spriteName = "masterequip00001";
        return false;
    }

    public static bool SetFace(UISprite sprite, int svtId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetFaceLocal(sprite, svtId, 0);

    public static bool SetFace(UISprite sprite, int svtId, int limitCount)
    {
        int imageLimitCount = (svtId <= 0) ? 0 : ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        return SingletonMonoBehaviour<AtlasManager>.Instance.SetFaceLocal(sprite, svtId, imageLimitCount);
    }

    public static void SetFaceBaseIcon(UISprite sprite, int frameType)
    {
        sprite.spriteName = baseFileList[frameType];
    }

    public static void SetFaceFrameIcon(UISprite sprite)
    {
        sprite.spriteName = "img_common_frame01";
    }

    public static bool SetFaceImage(UISprite sprite, int svtId, int imageLimitCount) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetFaceLocal(sprite, svtId, imageLimitCount);

    protected bool SetFaceLocal(UISprite sprite, int svtId, int imageLimitCount)
    {
        if (svtId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        string name = "f_" + svtId.ToString() + imageLimitCount.ToString();
        if (this.faceNormalAtlasList != null)
        {
            int length = this.faceNormalAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.faceNormalAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
        }
        sprite.atlas = this.faceAtlas;
        if (sprite.atlas.GetSprite(name) != null)
        {
            sprite.spriteName = name;
            return true;
        }
        sprite.spriteName = "f_1000000";
        return false;
    }

    public static void SetFormationBase(UISprite sprite, int frameType)
    {
        sprite.spriteName = formationBaseFileList[frameType];
    }

    public static void SetFormationFrame(UISprite sprite, int frameType)
    {
        sprite.spriteName = formationFrameFileList[frameType];
    }

    public static void SetFormationFrameForSupportSelect(UISprite sprite, int frameType)
    {
        sprite.spriteName = formationFrameForSupportSelectFileList[frameType];
    }

    public static bool SetHideFace(UISprite sprite) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetHideFaceLocal(sprite);

    protected bool SetHideFaceLocal(UISprite sprite)
    {
        string name = "f_1000010";
        if (this.faceNormalAtlasList != null)
        {
            int length = this.faceNormalAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.faceNormalAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
        }
        sprite.atlas = this.faceAtlas;
        if (sprite.atlas.GetSprite(name) != null)
        {
            sprite.spriteName = name;
            return true;
        }
        sprite.spriteName = "f_1000000";
        return false;
    }

    public static bool SetHideSkillIcon(UISprite sprite) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetSkillIconLocal(sprite, 1);

    public static bool SetItem(UISprite sprite, int itemImageId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetItemLocal(sprite, itemImageId);

    protected bool SetItemLocal(UISprite sprite, int itemImageId)
    {
        if (itemImageId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        string name = itemImageId.ToString();
        if (this.itemDownloadAtlasList != null)
        {
            int length = this.itemDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.itemDownloadAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
            sprite.atlas = this.itemAtlas;
        }
        else
        {
            sprite.atlas = this.itemAtlas;
            if (sprite.atlas.GetSprite(name) != null)
            {
                sprite.spriteName = name;
                return true;
            }
        }
        sprite.spriteName = "0";
        return false;
    }

    public static bool SetMark(UISprite sprite, string markName) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetMarkLocal(sprite, markName);

    protected bool SetMarkLocal(UISprite sprite, string markName)
    {
        if (string.IsNullOrEmpty(markName))
        {
            sprite.spriteName = null;
            return true;
        }
        sprite.atlas = (this.markDownloadAtlas == null) ? this.markAtlas : this.markDownloadAtlas;
        if (sprite.atlas.GetSprite(markName) != null)
        {
            sprite.spriteName = markName;
            return true;
        }
        sprite.spriteName = null;
        return false;
    }

    public static bool SetNoMountFace(UISprite sprite) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetNoMountFaceLocal(sprite);

    protected bool SetNoMountFaceLocal(UISprite sprite)
    {
        sprite.atlas = this.faceAtlas;
        sprite.spriteName = "f_1000000";
        return true;
    }

    public static bool SetRarityIcon(UISprite sprite, int rarity) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetRarityIconLocal(sprite, "rarity" + rarity.ToString() + "_0");

    public static bool SetRarityIcon(UISprite sprite, int rarity, int exceedCount)
    {
        if (exceedCount > 0)
        {
            ServantExceedEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantExceedMaster>(DataNameKind.Kind.SERVANT_EXCEED).getEntityFromId(rarity, exceedCount + 1);
            return SingletonMonoBehaviour<AtlasManager>.Instance.SetRarityIconLocal(sprite, "rarity" + rarity.ToString() + "_" + ((entity == null) ? "2" : "1"));
        }
        return SingletonMonoBehaviour<AtlasManager>.Instance.SetRarityIconLocal(sprite, "rarity" + rarity.ToString() + "_0");
    }

    protected bool SetRarityIconLocal(UISprite sprite, string rarityIconName)
    {
        if (string.IsNullOrEmpty(rarityIconName))
        {
            sprite.spriteName = null;
            return true;
        }
        if (this.classIconsDownloadAtlas != null)
        {
            UIAtlas classIconsDownloadAtlas = this.classIconsDownloadAtlas;
            if (classIconsDownloadAtlas.GetSprite(rarityIconName) != null)
            {
                sprite.atlas = classIconsDownloadAtlas;
                sprite.spriteName = rarityIconName;
                sprite.MakePixelPerfect();
                return true;
            }
            sprite.atlas = this.classIconsAtlas;
        }
        else
        {
            sprite.atlas = this.classIconsAtlas;
            if (sprite.atlas.GetSprite(rarityIconName) != null)
            {
                sprite.spriteName = rarityIconName;
                sprite.MakePixelPerfect();
                return true;
            }
        }
        sprite.spriteName = null;
        return false;
    }

    public static bool SetSBuffIcon(UISprite sprite, int buffId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetBuffIconLocal(sprite, buffId);

    public static bool SetSBuffIconByIconId(UISprite sprite, int buffIconId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetBuffIconByIconIdLocal(sprite, buffIconId);

    public static void SetServantType(UISprite sprite, int type, int frameType)
    {
        SvtType.Type type2 = (SvtType.Type) type;
        if (SvtType.IsServant(type2))
        {
            sprite.spriteName = $"listframes{Rarity.getServantClassIcon(frameType)}_txt_servant";
        }
        else if (SvtType.IsServantEquip(type2))
        {
            sprite.spriteName = $"listframes{Rarity.getServantTypeImg(frameType)}_txt_craftessence";
        }
        else if (type2 == SvtType.Type.COMBINE_MATERIAL)
        {
            sprite.spriteName = $"listframes{Rarity.getServantTypeImg(frameType)}_txt_expup";
        }
        else if (type2 == SvtType.Type.STATUS_UP)
        {
            sprite.spriteName = $"listframes{Rarity.getServantTypeImg(frameType)}_txt_statusup";
        }
        else if (SvtType.IsServant(type2))
        {
            sprite.spriteName = $"listframes{Rarity.getServantTypeImg(frameType)}_txt_servant";
        }
        else
        {
            sprite.spriteName = null;
        }
    }

    public static bool SetShopBanner(UISprite sprite, int bannerId) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetShopBannerLocal(sprite, "shop_event_menu_" + bannerId);

    public static bool SetShopBanner(UISprite sprite, string bannerName) => 
        SingletonMonoBehaviour<AtlasManager>.Instance.SetShopBannerLocal(sprite, bannerName);

    protected bool SetShopBannerLocal(UISprite sprite, string bannerName)
    {
        if (sprite == null)
        {
            return true;
        }
        if (string.IsNullOrEmpty(bannerName))
        {
            sprite.spriteName = null;
            return true;
        }
        if (this.shopBannerDownloadAtlasList != null)
        {
            int length = this.shopBannerDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.shopBannerDownloadAtlasList[i];
                if (atlas.GetSprite(bannerName) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = bannerName;
                    return true;
                }
            }
        }
        sprite.spriteName = null;
        return false;
    }

    public static bool SetSkillIcon(UISprite sprite, int skillId)
    {
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillId);
        int skillImageId = (entity == null) ? 0 : entity.iconId;
        return SingletonMonoBehaviour<AtlasManager>.Instance.SetSkillIconLocal(sprite, skillImageId);
    }

    protected bool SetSkillIconLocal(UISprite sprite, int skillImageId)
    {
        if (skillImageId <= 0)
        {
            sprite.spriteName = null;
            return true;
        }
        string name = $"skill_{skillImageId:D5}";
        if (this.skillIconDownloadAtlasList != null)
        {
            int length = this.skillIconDownloadAtlasList.Length;
            for (int i = 0; i < length; i++)
            {
                UIAtlas atlas = this.skillIconDownloadAtlasList[i];
                if (atlas.GetSprite(name) != null)
                {
                    sprite.atlas = atlas;
                    sprite.spriteName = name;
                    return true;
                }
            }
            sprite.atlas = this.skillIconAtlas;
        }
        else
        {
            sprite.atlas = this.skillIconAtlas;
            if (sprite.atlas.GetSprite(name) != null)
            {
                sprite.spriteName = name;
                return true;
            }
        }
        sprite.spriteName = "skill_00000";
        return false;
    }

    public void UnloadBuffIconAtlas()
    {
        AssetManager.releaseAssetStorage("BuffIcons/DownloadBuffIcon");
        this.buffIconDownloadAtlasList = null;
    }

    public enum FrameType
    {
        BLACK = 0,
        BLANK = 5,
        BRONZE = 1,
        DEFAULT = 2,
        GOLD = 3,
        HIDE = 4,
        SILVER = 2,
        SUPPORT = 6
    }
}

