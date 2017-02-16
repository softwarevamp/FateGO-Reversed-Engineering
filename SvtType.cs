using System;

public static class SvtType
{
    public static bool IsCombineMaterial(Type type) => 
        (type == Type.COMBINE_MATERIAL);

    public static bool IsEnemy(Type type) => 
        (type == Type.ENEMY);

    public static bool IsEnemy(int type) => 
        (type == 4);

    public static bool IsEnemyCollectionDetail(Type type) => 
        (type == Type.ENEMY_COLLECTION_DETAIL);

    public static bool IsExpUp(Type type) => 
        ((type == Type.COMBINE_MATERIAL) || (type == Type.SVT_EQUIP_MATERIAL));

    public static bool IsKeepServant(Type type) => 
        ((((type == Type.NORMAL) || (type == Type.HEROINE)) || (type == Type.COMBINE_MATERIAL)) || (type == Type.STATUS_UP));

    public static bool IsKeepServantEquip(Type type) => 
        ((type == Type.SERVANT_EQUIP) || (type == Type.SVT_EQUIP_MATERIAL));

    public static bool IsLock(Type type) => 
        (((((type == Type.NORMAL) || (type == Type.HEROINE)) || ((type == Type.COMBINE_MATERIAL) || (type == Type.SERVANT_EQUIP))) || (type == Type.STATUS_UP)) || (type == Type.SVT_EQUIP_MATERIAL));

    public static bool IsOrganization(Type type) => 
        ((type == Type.NORMAL) || (type == Type.HEROINE));

    public static bool IsServant(Type type) => 
        ((((type == Type.NORMAL) || (type == Type.HEROINE)) || (type == Type.ENEMY)) || (type == Type.ENEMY_COLLECTION));

    public static bool IsServantCollection(Type type) => 
        ((((type == Type.NORMAL) || (type == Type.HEROINE)) || (type == Type.ENEMY_COLLECTION)) || (type == Type.ENEMY_COLLECTION_DETAIL));

    public static bool IsServantEquip(Type type) => 
        (type == Type.SERVANT_EQUIP);

    public static bool IsStatusUp(Type type) => 
        (type == Type.STATUS_UP);

    public static bool IsSvtEqMaterial(Type type) => 
        (type == Type.SVT_EQUIP_MATERIAL);

    public enum Type
    {
        COMBINE_MATERIAL = 3,
        ENEMY = 4,
        ENEMY_COLLECTION = 5,
        ENEMY_COLLECTION_DETAIL = 9,
        HEROINE = 2,
        NORMAL = 1,
        SERVANT_EQUIP = 6,
        STATUS_UP = 7,
        SVT_EQUIP_MATERIAL = 8
    }
}

