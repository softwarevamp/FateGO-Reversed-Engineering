using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

public class UIMasterFullFigureRender : UITweenRenderer
{
    protected static readonly int BODY_SIZE_X = 0x3fe;
    protected static readonly int BODY_SIZE_Y = 0x3fe;
    protected static readonly ReadOnlyCollection<Vector2> bodySizeTable;
    protected static readonly ReadOnlyCollection<Vector2> bodyTextureSizeTable;
    protected static readonly ReadOnlyCollection<Vector2> bodyTopTable;
    protected Vector2 dispOffset;
    protected DispType dispType;
    protected int equipId;
    protected int genderType;
    protected static readonly int MAIN_SIZE_X = 0x400;
    protected static readonly int MAIN_SIZE_Y = 0x400;

    static UIMasterFullFigureRender()
    {
        Vector2[] array = new Vector2[] { new Vector2(0f, 0f) };
        bodyTopTable = Array.AsReadOnly<Vector2>(array);
        Vector2[] vectorArray2 = new Vector2[] { new Vector2(1022f, 1022f) };
        bodyTextureSizeTable = Array.AsReadOnly<Vector2>(vectorArray2);
        Vector2[] vectorArray3 = new Vector2[] { new Vector2(1022f, 1022f) };
        bodySizeTable = Array.AsReadOnly<Vector2>(vectorArray3);
    }

    public static string[] GetAssetNameList(int genderType, int equipId)
    {
        int num;
        string[] strArray = new string[1];
        if (equipId > 0)
        {
            EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
            if (entity != null)
            {
                num = (genderType != 2) ? entity.maleImageId : entity.femaleImageId;
                if (num > 0)
                {
                    strArray[0] = $"MasterFullFigure/equip{num:D5}";
                    if (AssetManager.isExistAssetStorage(strArray[0]))
                    {
                        return strArray;
                    }
                }
            }
        }
        num = (genderType != 2) ? 1 : 2;
        strArray[0] = $"MasterFullFigure/equip{num:D5}";
        return strArray;
    }

    public Texture2D GetBodyAlphaTexture(AssetData[] dataList)
    {
        string lastName = dataList[0].LastName;
        return dataList[0].GetObject<Texture2D>(lastName + "a");
    }

    public Vector2 GetBodySize() => 
        bodySizeTable[(int) this.dispType];

    public Texture2D GetBodyTexture(AssetData[] dataList)
    {
        string lastName = dataList[0].LastName;
        return dataList[0].GetObject<Texture2D>(lastName);
    }

    public Rect GetBodyUvRect()
    {
        Vector2 vector = bodyTopTable[(int) this.dispType];
        Vector2 vector2 = bodyTextureSizeTable[(int) this.dispType];
        vector.x += ((BODY_SIZE_X - vector2.x) / 2f) + 1.5f;
        vector.y += (BODY_SIZE_Y - vector2.y) + 1.5f;
        return new Rect(vector.x / ((float) MAIN_SIZE_X), vector.y / ((float) MAIN_SIZE_Y), vector2.x / ((float) MAIN_SIZE_X), vector2.y / ((float) MAIN_SIZE_Y));
    }

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

    public void SetCharacter(DispType dispType, int genderType, int equipId)
    {
        this.dispType = dispType;
        this.genderType = genderType;
        this.equipId = equipId;
        int num = 0;
        if (equipId > 0)
        {
            EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
            if (entity != null)
            {
                num = (genderType != 2) ? entity.maleImageId : entity.femaleImageId;
                if ((num > 0) && !AssetManager.isExistAssetStorage($"MasterFullFigure/equip{num:D5}"))
                {
                    num = 0;
                }
            }
        }
        if (num <= 0)
        {
            num = (genderType != 2) ? 1 : 2;
        }
        EquipImageEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP_IMAGE).getEntityFromId<EquipImageEntity>(num);
        DispType type = this.dispType;
        this.dispOffset = new Vector2((float) entity2.offsetX, (float) entity2.offsetY);
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

    public enum DispType
    {
        USER_SELECT
    }
}

