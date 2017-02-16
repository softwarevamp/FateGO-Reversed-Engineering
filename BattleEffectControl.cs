using System;
using UnityEngine;

public class BattleEffectControl : BaseMonoBehaviour
{
    [SerializeField]
    private GameObject avoidanceObject;
    [SerializeField]
    private GameObject buffIconObject;
    [SerializeField]
    private GameObject[] buffTextObjectlist;
    [SerializeField]
    private GameObject[] damageObjectlist;
    [SerializeField]
    private GameObject[] EffectList;
    [SerializeField]
    private GameObject invincibleObject;
    private Spawner spawner;

    public void destroyInstantiate(GameObject obj)
    {
        this.spawner.Despawn(obj, true);
    }

    public GameObject getavoidanceObject() => 
        this.avoidanceObject;

    public GameObject getBuffTextObject(int color)
    {
        int index = color - 1;
        if (index < 0)
        {
            index = 0;
        }
        if ((this.buffTextObjectlist != null) && (index < this.buffTextObjectlist.Length))
        {
            return this.buffTextObjectlist[index];
        }
        index = 0;
        if ((this.buffTextObjectlist != null) && (index < this.buffTextObjectlist.Length))
        {
            return this.buffTextObjectlist[index];
        }
        return null;
    }

    public GameObject getDamageObject(bool critical, bool week, bool resist)
    {
        if (critical)
        {
            return this.spawner.Spawn(this.damageObjectlist[1]);
        }
        if (week)
        {
            return this.spawner.Spawn(this.damageObjectlist[2]);
        }
        if (resist)
        {
            return this.spawner.Spawn(this.damageObjectlist[3]);
        }
        return this.spawner.Spawn(this.damageObjectlist[0]);
    }

    public GameObject getEffectInstantiate(ID id)
    {
        int index = (int) id;
        if (index < this.EffectList.Length)
        {
            return this.spawner.Spawn(this.EffectList[index]);
        }
        return null;
    }

    public GameObject getinvincibleObject() => 
        this.invincibleObject;

    public GameObject getListEffect(ID id)
    {
        int index = (int) id;
        if (index < this.EffectList.Length)
        {
            return this.EffectList[index];
        }
        return null;
    }

    public GameObject getTreasureObject(int rarity, Transform rootTr)
    {
        GameObject obj2 = null;
        Transform root = null;
        if (0x3e8 <= rarity)
        {
            obj2 = base.createObject("effect/obj_treasure_item01", rootTr, null);
            root = obj2.transform.getNodeFromName("treasure", false);
            if (rarity == 0x3e9)
            {
                base.createObject("effect/obj_treasure_p", root, null);
                return obj2;
            }
            if (rarity == 0x3ea)
            {
                base.createObject("effect/obj_treasure_pp", root, null);
            }
            return obj2;
        }
        obj2 = base.createObject($"effect/obj_treasure{(rarity % 3) + 1:D2}", rootTr, null);
        root = obj2.transform.getNodeFromName("treasure", false);
        switch ((rarity / 3))
        {
            case 1:
                base.createObject("effect/obj_treasure_p", root, null);
                return obj2;

            case 2:
                base.createObject("effect/obj_treasure_pp", root, null);
                return obj2;
        }
        return obj2;
    }

    public GameObject setBuffIconObject(int[] bufflist)
    {
        if ((bufflist == null) || (bufflist.Length <= 0))
        {
            return null;
        }
        GameObject obj2 = new GameObject();
        Vector3 vector = new Vector3();
        foreach (int num in bufflist)
        {
            GameObject obj3 = base.createObject(this.buffIconObject, obj2.transform, null);
            obj3.GetComponent<BattleServantBuffIconComponent>().setIcon(num);
            obj3.transform.localPosition = vector;
            vector.x -= 44f;
        }
        return obj2;
    }

    private void Start()
    {
        this.spawner = SingletonMonoBehaviour<Spawner>.Instance;
        if (this.spawner != null)
        {
            this.spawner.Precache(this.EffectList[2], 5);
            this.spawner.Precache(this.EffectList[3], 5);
            this.spawner.Precache(this.EffectList[4], 50);
            this.spawner.Precache(this.EffectList[6], 3);
            this.spawner.Precache(this.EffectList[7], 5);
            this.spawner.Precache(this.damageObjectlist[0], 5);
            this.spawner.Precache(this.damageObjectlist[1], 5);
            this.spawner.Precache(this.damageObjectlist[2], 5);
            this.spawner.Precache(this.damageObjectlist[3], 5);
        }
    }

    public enum ID
    {
        DAMAGE_NO,
        CRITICAL_DAMAGE__NO,
        TITLE_CRITICAL,
        TITLE_WEEK,
        STAR,
        SERVANT,
        HEAL_NO,
        REGIST
    }
}

