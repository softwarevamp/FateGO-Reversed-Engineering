using System;
using UnityEngine;

public class FGOActionUtil
{
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
}

