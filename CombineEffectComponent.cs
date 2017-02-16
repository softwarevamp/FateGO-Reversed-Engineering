using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CombineEffectComponent : MonoBehaviour
{
    private List<string> _releaseAssetPath;
    protected int baseClassCardId;
    private UserServantEntity baseSvtEntity;
    private int baseSvtId;
    private int baseSvtlimitCnt;
    private GameObject bgParentObject;
    private readonly int[] cardTextureSize = new int[] { 0x200, 0x36b };
    private int cardType;
    private int cntIndex;
    private GameObject effect;
    protected int feedClassCardId;
    private List<string> feedItemNameList;
    private List<string> feedNameList;
    private PlayMakerFSM fsm;
    private readonly string[] itemAniName = new string[] { "combine_fodder01", "combine_fodder012", "combine_fodder013", "combine_fodder014", "combine_fodder015" };
    private Transform itemBackNode;
    private int itemCntIdex;
    private List<int> itemList = new List<int>();
    private int itemMaterialCnt;
    private Transform itemNode;
    private Kind kind;
    private int materialCnt;
    private List<UserServantEntity> materialList = new List<UserServantEntity>();
    private readonly string[] startAniName = new string[] { "combine_01", "combine_02", "combine_03", "combine_04", "combine_05" };

    public void endAnimation()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            if (this.effect != null)
            {
                UnityEngine.Object.Destroy(this.effect);
                this.effect = null;
            }
            this.fsm.SendEvent("END_FADE");
        });
    }

    private void EndLoadBg(AssetData data)
    {
        <EndLoadBg>c__AnonStorey83 storey = new <EndLoadBg>c__AnonStorey83 {
            data = data,
            <>f__this = this
        };
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(storey.<>m__B7));
    }

    public void fadeIn()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => this.fsm.SendEvent("END_FADE"));
    }

    private void getCardBackImg(out Rarity.TYPE rarity, out int backCardImgId, int svtId, int svtLimitCnt)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, svtLimitCnt);
        rarity = (Rarity.TYPE) entity2.rarity;
        this.cardType = entity.type;
        switch (entity.type)
        {
            case 6:
                backCardImgId = ConstantMaster.getValue("BACKSIDE_SVT_EQUIP_IMAGE_ID");
                return;
        }
        backCardImgId = ConstantMaster.getValue("BACKSIDE_SVT_IMAGE_ID");
    }

    private void getDispInfo(out Rarity.TYPE rarity, out int classCardId, int svtId, int svtLimitCnt)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, svtLimitCnt);
        ServantClassEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(entity.classId);
        classCardId = entity3.imageId;
        rarity = (Rarity.TYPE) entity2.rarity;
    }

    public void InitCombineEffect()
    {
        this.fsm = base.GetComponent<PlayMakerFSM>();
        this.cntIndex = 0;
        if (this.kind == Kind.SERVANT)
        {
            this.bgParentObject = base.gameObject.transform.FindChild("EffectPanel/Combine_bit_prefab(Clone)/Combine_bit/BG_root").gameObject;
        }
        else
        {
            this.bgParentObject = base.gameObject.transform.FindChild("EffectPanel/Combine_fodder01(Clone)/Combine_bit/BG_root").gameObject;
        }
        GameObject gameObject = this.bgParentObject.transform.FindChild("bg").gameObject;
        if (gameObject != null)
        {
            UnityEngine.Object.DestroyImmediate(gameObject);
        }
        string name = "Bg/10500";
        AssetManager.loadAssetStorage(name, new AssetLoader.LoadEndDataHandler(this.EndLoadBg));
        SoundManager.playSe("co1");
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
        this.materialList.Clear();
        this.itemList.Clear();
    }

    private void SetCardParam()
    {
        Rarity.TYPE type;
        int num3;
        this.effect = this.fsm.FsmVariables.GetFsmGameObject("CombineEffect").Value;
        float x = this.fsm.FsmVariables.GetFsmFloat("CardScale").Value;
        string nodename = this.fsm.FsmVariables.GetFsmString("BaseCardNodeName").Value;
        string str2 = this.fsm.FsmVariables.GetFsmString("FstFeedNodeName").Value;
        string str3 = this.fsm.FsmVariables.GetFsmString("ScndFeedNodeName").Value;
        string str4 = this.fsm.FsmVariables.GetFsmString("ThrdFeedNodeName").Value;
        string str5 = this.fsm.FsmVariables.GetFsmString("FourthFeedNodeName").Value;
        string str6 = this.fsm.FsmVariables.GetFsmString("FifthFeedNodeName").Value;
        List<string> list = new List<string> {
            str2,
            str3,
            str4,
            str5,
            str6
        };
        this.feedNameList = list;
        string str7 = this.fsm.FsmVariables.GetFsmString("SkillItemNodeName").Value;
        string str8 = this.fsm.FsmVariables.GetFsmString("SecItemNodeName").Value;
        string str9 = this.fsm.FsmVariables.GetFsmString("ThrdItemNodeName").Value;
        string str10 = this.fsm.FsmVariables.GetFsmString("FourthItemNodeName").Value;
        string str11 = this.fsm.FsmVariables.GetFsmString("FifthItemNodeName").Value;
        list = new List<string> {
            str7,
            str8,
            str9,
            str10,
            str11
        };
        this.feedItemNameList = list;
        this._releaseAssetPath = new List<string>();
        Transform transform = this.effect.transform.getNodeFromName(nodename, true);
        transform.gameObject.SetActive(false);
        int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.baseSvtEntity.svtId, this.baseSvtEntity.limitCount, true, true);
        UICharaGraphTexture texture = CharaGraphManager.CreateTexturePrefab(transform.gameObject, this.baseSvtEntity, imageLimitCount, 10, null);
        texture.transform.localPosition = Vector3.zero;
        texture.transform.localScale = new Vector3(x, x, x);
        this.getDispInfo(out type, out num3, this.baseSvtId, this.baseSvtlimitCnt);
        this.baseClassCardId = num3;
        string designCardPath = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(num3, type);
        this._releaseAssetPath.Add(designCardPath);
        AssetManager.loadAssetStorage(designCardPath, new AssetLoader.LoadEndDataHandler(this.setReverseCallback));
        string str13 = "combine_01";
        switch (this.kind)
        {
            case Kind.SERVANT:
                this.materialCnt = this.materialList.Count;
                str13 = this.startAniName[this.materialCnt - 1];
                Debug.Log("** !! ** StartAnimationName: " + str13);
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str13;
                this.setServantFeed();
                break;

            case Kind.SKILL:
                this.itemMaterialCnt = this.itemList.Count;
                str13 = this.itemAniName[this.itemMaterialCnt - 1];
                Debug.Log("** !! ** StartAnimationName: " + str13);
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str13;
                this.setItemFeed();
                break;

            case Kind.DVC:
                str13 = "combine_fodder02";
                Debug.Log("** !! ** StartAnimationName: " + str13);
                this.fsm.FsmVariables.GetFsmString("StartAnimationName").Value = str13;
                this.setItemFeed();
                break;
        }
    }

    public void SetDvcCombineInfo(UserServantEntity baseSvtData, List<int> list)
    {
        this.kind = Kind.DVC;
        this.baseSvtEntity = baseSvtData;
        this.baseSvtId = baseSvtData.svtId;
        this.baseSvtlimitCnt = baseSvtData.limitCount;
        this.itemList = list;
    }

    private void SetFeedCallback(AssetData data)
    {
        Transform cardNode = this.effect.transform.getNodeFromName(this.feedNameList[this.cntIndex], true);
        SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(data, cardNode, this.feedClassCardId);
        UITexture component = cardNode.GetComponent<UITexture>();
        string name = (this.cardType != 6) ? "Unlit/Transparent Colored" : "Custom/Sprite-MasterFigure (SoftClip)";
        Material material = new Material(Shader.Find(name));
        Texture mainTexture = component.mainTexture;
        component.material = material;
        material.mainTexture = mainTexture;
        this.cntIndex++;
        if (this.cntIndex <= (this.materialCnt - 1))
        {
            this.setServantFeed();
        }
        else
        {
            this.cntIndex = 0;
            this.fsm.SendEvent("START_ANIMATION");
        }
    }

    private void setItemFeed()
    {
        for (int i = 0; i < this.itemList.Count; i++)
        {
            int itemImageId = this.itemList[i];
            this.itemNode = this.effect.transform.getNodeFromName(this.feedItemNameList[i], true);
            AtlasManager.SetItem(this.itemNode.GetComponent<UISprite>(), itemImageId);
        }
        this.fsm.SendEvent("START_ANIMATION");
    }

    private void setReverseCallback(AssetData data)
    {
        string nodename = this.fsm.FsmVariables.GetFsmString("ReverseCardNodeName").Value;
        SingletonMonoBehaviour<DesignCardManager>.Instance.SetupCardImage(data, this.effect.transform.getNodeFromName(nodename, true), this.baseClassCardId);
    }

    public void SetServantCombineInfo(UserServantEntity baseSvtData, List<UserServantEntity> list)
    {
        this.kind = Kind.SERVANT;
        this.baseSvtEntity = baseSvtData;
        this.baseSvtId = baseSvtData.svtId;
        this.baseSvtlimitCnt = baseSvtData.limitCount;
        this.materialList = list;
    }

    private void setServantFeed()
    {
        Rarity.TYPE type;
        int num3;
        int svtId = this.materialList[this.cntIndex].svtId;
        int limitCount = this.materialList[this.cntIndex].limitCount;
        this.getCardBackImg(out type, out num3, svtId, limitCount);
        this.feedClassCardId = num3;
        string designCardPath = SingletonMonoBehaviour<DesignCardManager>.Instance.GetDesignCardPath(num3, type);
        this._releaseAssetPath.Add(designCardPath);
        AssetManager.loadAssetStorage(designCardPath, new AssetLoader.LoadEndDataHandler(this.SetFeedCallback));
    }

    public void SetSkillCombineInfo(UserServantEntity baseSvtData, List<int> list)
    {
        this.kind = Kind.SKILL;
        this.baseSvtEntity = baseSvtData;
        this.baseSvtId = baseSvtData.svtId;
        this.baseSvtlimitCnt = baseSvtData.limitCount;
        this.itemList = list;
    }

    private void Start()
    {
    }

    [CompilerGenerated]
    private sealed class <EndLoadBg>c__AnonStorey83
    {
        internal CombineEffectComponent <>f__this;
        internal AssetData data;

        internal void <>m__B7()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.data.GetObject<GameObject>("bg"));
            obj2.transform.parent = this.<>f__this.bgParentObject.transform;
            obj2.transform.localRotation = Quaternion.identity;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localScale = Vector3.one;
            this.<>f__this.fsm.SendEvent("END_FADE");
        }
    }

    private enum CardColor
    {
        BRONZE,
        SILVER,
        GOLD
    }

    public enum Kind
    {
        SERVANT,
        SKILL,
        DVC
    }
}

