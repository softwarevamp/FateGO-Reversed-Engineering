using System;
using UnityEngine;

public static class BattleEffectUtility
{
    private static string[] scalelist = new string[] { string.Empty, "_S", "_M", "_L" };

    public static GameObject getEffectObject(int effectId, GameObject procObject)
    {
        EffectEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EffectMaster>(DataNameKind.Kind.EFFECT).getEntityFromId<EffectEntity>(effectId);
        GameObject original = null;
        BattleActorControl component = null;
        component = procObject.GetComponent<BattleActorControl>();
        switch (entity.folderType)
        {
            case 1:
                original = Resources.Load("effect/" + entity.name) as GameObject;
                break;

            case 2:
                if (component != null)
                {
                    original = ServantAssetLoadManager.loadNoblePhantasmEffect(component.BattleSvtData.TreasureDvcId, entity.name);
                }
                break;

            case 3:
                if (component != null)
                {
                    int index = component.getWeaponScale();
                    int weapongroup = component.getWeaponGroup();
                    int effectFolder = component.getEffectFolder();
                    original = ServantAssetLoadManager.loadWeaponGroupEffect(entity.name + scalelist[index], weapongroup, effectFolder);
                }
                break;
        }
        if (entity.isSe())
        {
            SoundManager.playSe("Battle", entity.se);
        }
        if (original == null)
        {
            Debug.LogError(" No Effect :" + effectId);
            return null;
        }
        GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(original);
        if ((entity.folderType == 3) && (obj3 != null))
        {
            UIWidget[] componentsInChildren = obj3.GetComponentsInChildren<UIWidget>();
            Color color = component.getWeaponColor();
            foreach (UIWidget widget in componentsInChildren)
            {
                widget.color = color;
            }
        }
        obj3.SetActive(false);
        return obj3;
    }

    public static GameObject getEffectObject(ResourceFolder folder, string name, GameObject actorObject)
    {
        switch (folder)
        {
            case ResourceFolder.COMMON_EFFECT:
                return (Resources.Load("Battle/CommonEffects/" + name) as GameObject);

            case ResourceFolder.ACTOR_EFFECT:
                return actorObject?.GetComponent<BattleActorControl>().getActorEffect(name);

            case ResourceFolder.BATTLE_EFFECT:
                return (Resources.Load("effect/" + name) as GameObject);
        }
        return null;
    }

    public static string getEffectSeName(int effectId)
    {
        EffectEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EffectMaster>(DataNameKind.Kind.EFFECT).getEntityFromId<EffectEntity>(effectId);
        return (!entity.isSe() ? null : entity.se);
    }

    public static GameObject loadEffectToNode(GameObject targetObject, int effectId, GameObject procObject)
    {
        EffectEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EffectMaster>(DataNameKind.Kind.EFFECT).getEntityFromId<EffectEntity>(effectId);
        GameObject obj2 = getEffectObject(effectId, procObject);
        Transform transform = targetObject.transform.getNodeFromLvName(entity.getNodeName(), -1);
        obj2.transform.parent = transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.eulerAngles = Vector3.up;
        obj2.transform.localScale = Vector3.one;
        return obj2;
    }
}

