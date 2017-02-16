using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using UnityEngine;

public class UICharaGraphRender : UITweenRenderer
{
    protected int adjustAtk;
    protected int adjustHp;
    protected int atk;
    [SerializeField]
    protected Kind baseKind;
    protected static readonly string[] bodyTextureNameTable;
    protected int classId;
    protected static string[] frameSpriteNameTable;
    protected int hp;
    protected int imageLimitCount;
    protected int limitCount;
    protected static readonly int MAIN_SIZE_X = 0x400;
    protected static readonly int MAIN_SIZE_Y = 0x400;
    protected static int OTHER_IMAGE_LIMIT_COUNT = 4;
    protected int rarity;
    protected static string[] raritySpriteNameTable;
    protected static readonly float SERVANT_BODY_H = (((float) (SERVANT_BODY_SIZE_Y - 1)) / ((float) MAIN_SIZE_Y));
    protected static readonly int SERVANT_BODY_SIZE_X = 510;
    protected static readonly int SERVANT_BODY_SIZE_Y = 0x2d2;
    protected static readonly float SERVANT_BODY_U = (1.5f / ((float) MAIN_SIZE_X));
    protected static readonly float SERVANT_BODY_V = (((float) ((MAIN_SIZE_Y - SERVANT_BODY_SIZE_Y) - 1.5)) / ((float) MAIN_SIZE_Y));
    protected static readonly float SERVANT_BODY_W = (((float) (SERVANT_BODY_SIZE_X - 1)) / ((float) MAIN_SIZE_X));
    protected static readonly float SERVANT_EQUIP_BODY_H = (((float) (SERVANT_EQUIP_BODY_SIZE_Y - 1)) / ((float) MAIN_SIZE_Y));
    protected static readonly int SERVANT_EQUIP_BODY_SIZE_X = 510;
    protected static readonly int SERVANT_EQUIP_BODY_SIZE_Y = 0x368;
    protected static readonly float SERVANT_EQUIP_BODY_U = (1.5f / ((float) MAIN_SIZE_X));
    protected static readonly float SERVANT_EQUIP_BODY_V = (((float) ((MAIN_SIZE_Y - SERVANT_EQUIP_BODY_SIZE_Y) - 1.5)) / ((float) MAIN_SIZE_Y));
    protected static readonly float SERVANT_EQUIP_BODY_W = (((float) (SERVANT_EQUIP_BODY_SIZE_X - 1)) / ((float) MAIN_SIZE_X));
    protected static readonly ReadOnlyCollection<Rect> servantBodyRectTable;
    protected static readonly Rect servantEquipBodyRect;
    protected int svtId;
    protected SvtType.Type svtType;
    protected int textureImageLimitCount;

    static UICharaGraphRender()
    {
        Rect[] array = new Rect[] { new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H), new Rect(SERVANT_BODY_U + 0.5f, SERVANT_BODY_V, SERVANT_BODY_W, SERVANT_BODY_H) };
        servantBodyRectTable = Array.AsReadOnly<Rect>(array);
        servantEquipBodyRect = new Rect(SERVANT_EQUIP_BODY_U, SERVANT_EQUIP_BODY_V, SERVANT_EQUIP_BODY_W, SERVANT_EQUIP_BODY_H);
        bodyTextureNameTable = new string[] { "a", "a", "b", "b", "c", "c" };
        raritySpriteNameTable = new string[] { "rarity_c", "rarity_uc", "rarity_r", "rarity_sr", "rarity_ssr" };
        frameSpriteNameTable = new string[] { "frame1", "frame1", "frame2", "frame3", "frame3" };
    }

    public static string[] GetAssetNameList(int svtId, int imageLimitCount)
    {
        string name = "CharaGraph/" + svtId;
        if (!AssetManager.isExistAssetStorage(name))
        {
            name = "CharaGraph/100000";
        }
        return new string[] { name };
    }

    public Vector2 GetBodySize()
    {
        if (this.baseKind == Kind.SERVANT)
        {
            return new Vector2((float) SERVANT_BODY_SIZE_X, (float) SERVANT_BODY_SIZE_Y);
        }
        return new Vector2((float) SERVANT_EQUIP_BODY_SIZE_X, (float) SERVANT_EQUIP_BODY_SIZE_Y);
    }

    public Texture2D GetBodyTexture(AssetData data)
    {
        if (this.baseKind == Kind.SERVANT)
        {
            Texture2D textured = data.GetObject<Texture2D>(data.LastName + bodyTextureNameTable[this.imageLimitCount]);
            if (textured != null)
            {
                this.textureImageLimitCount = this.imageLimitCount;
                return textured;
            }
            this.textureImageLimitCount = 0;
            return data.GetObject<Texture2D>(data.LastName + bodyTextureNameTable[this.textureImageLimitCount]);
        }
        if (this.baseKind == Kind.ENEMY_COLLECTION_DETAIL)
        {
            if (this.imageLimitCount == OTHER_IMAGE_LIMIT_COUNT)
            {
                Texture2D textured2 = data.GetObject<Texture2D>(data.LastName + "b");
                if (textured2 != null)
                {
                    this.textureImageLimitCount = this.imageLimitCount;
                    return textured2;
                }
            }
            this.textureImageLimitCount = 0;
            return data.GetObject<Texture2D>(data.LastName + "a");
        }
        this.textureImageLimitCount = 0;
        return data.GetObject<Texture2D>(data.LastName + "a");
    }

    public Rect GetBodyUvRect()
    {
        if (this.baseKind == Kind.SERVANT)
        {
            return servantBodyRectTable[this.textureImageLimitCount];
        }
        return servantEquipBodyRect;
    }

    public string GetClassSprite()
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(this.classId) != null)
        {
            return $"class{this.classId:D3}_{this.GetFrameSprite()}";
        }
        return null;
    }

    public string GetFrameSprite() => 
        frameSpriteNameTable[this.rarity - 1];

    public Texture2D GetNameTexture(AssetData data) => 
        data.GetObject<Texture2D>(data.LastName + "a");

    public string GetRaritySprite() => 
        raritySpriteNameTable[this.rarity - 1];

    public void MoveAlpha(float duration, float alpha)
    {
        if (duration > 0f)
        {
            Color color = base.color;
            color.a = alpha;
            TweenRendererColor.Begin(base.gameObject, duration, color);
        }
        else
        {
            this.SetAlpha(alpha);
        }
    }

    public void MoveAlpha(float duration, float alpha, GameObject callbackObject, string callbackFunc)
    {
        if (duration > 0f)
        {
            Color color = base.color;
            color.a = alpha;
            TweenRendererColor color2 = TweenRendererColor.Begin(base.gameObject, duration, color);
            if ((callbackObject != null) && (color2 != null))
            {
                color2.eventReceiver = callbackObject;
                color2.callWhenFinished = callbackFunc;
            }
        }
        else
        {
            this.SetAlpha(alpha);
            if (callbackObject != null)
            {
                callbackObject.SendMessage(callbackFunc);
            }
        }
    }

    public void SetAlpha(float alpha)
    {
        Color c = base.color;
        c.a = alpha;
        this.SetTweenColor(c);
    }

    protected void SetAtk(int atk, int adjustAtk = 0)
    {
        if (this.atk >= 0)
        {
            this.atk = atk;
            this.adjustAtk = adjustAtk;
        }
    }

    protected void SetCharacter()
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.svtId);
        this.svtType = (SvtType.Type) entity.type;
        this.classId = entity.classId;
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.svtId, this.limitCount);
        this.rarity = entity2.rarity;
        if (entity.type == 3)
        {
            this.atk = -1;
            this.hp = -1;
        }
        else
        {
            this.atk = 0;
            this.hp = 0;
        }
        this.adjustAtk = 0;
        this.adjustHp = 0;
    }

    public void SetCharacter(int svtId, int limitCount, int imageLimitCount)
    {
        this.svtId = svtId;
        this.limitCount = (limitCount <= BalanceConfig.ServantLimitMax) ? limitCount : BalanceConfig.ServantLimitMax;
        if (imageLimitCount == BalanceConfig.OtherImageLimitCount)
        {
            this.imageLimitCount = OTHER_IMAGE_LIMIT_COUNT;
        }
        else
        {
            this.imageLimitCount = imageLimitCount;
        }
        this.SetCharacter();
    }

    public void SetCharacter(int svtId, int limitCount, bool isOwn, bool isReal)
    {
        this.svtId = svtId;
        this.limitCount = (limitCount <= BalanceConfig.ServantLimitMax) ? limitCount : BalanceConfig.ServantLimitMax;
        this.imageLimitCount = ImageLimitCount.GetCardImageLimitCount(svtId, limitCount, isOwn, isReal);
        this.SetCharacter();
    }

    protected void SetHp(int hp, int adjustHp = 0)
    {
        if (this.hp >= 0)
        {
            this.hp = hp;
            this.adjustHp = adjustHp;
        }
    }

    public void SetLayer(int layer)
    {
        if (base.gameObject.layer != layer)
        {
            this.SetLayer(base.transform, layer);
        }
    }

    protected void SetLayer(Transform tf, int layer)
    {
        tf.gameObject.layer = layer;
        IEnumerator enumerator = tf.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                this.SetLayer(current, layer);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public enum Kind
    {
        SERVANT,
        SERVANT_EQUIP,
        ENEMY_COLLECTION_DETAIL
    }
}

