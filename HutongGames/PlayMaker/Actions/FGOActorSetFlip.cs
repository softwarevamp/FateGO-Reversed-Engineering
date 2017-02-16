namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorSetFlip : FsmStateAction
    {
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public FsmBool isDirLeft;

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            if (obj2 != null)
            {
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    if (this.isDirLeft.Value)
                    {
                        component.setDirLeft();
                    }
                    else
                    {
                        component.setDirRight();
                    }
                }
            }
            base.Finish();
        }
    }
}

