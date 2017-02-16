namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("FGOAction")]
    public class FGOActorHitDamageEffect : FsmStateAction
    {
        [RequiredField, Tooltip("ActionActor"), CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmInt countValue;
        [Tooltip("GameObject name to create. Usually a Prefab.")]
        public FileName.HIT_EFFECT effecttype;
        public FsmBool isParent;
        public FsmString nodename;
        public FsmVector3 position;
        [Tooltip("Folder for load")]
        public ResourceFolder resourceFolder;
        public FsmInt startValue;
        public FsmGameObject storeObject;
        [CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            Debug.Log(" Do not Use ");
        }

        public override void Reset()
        {
            this.effecttype = FileName.HIT_EFFECT.SLASH_VERTICAL;
        }
    }
}

