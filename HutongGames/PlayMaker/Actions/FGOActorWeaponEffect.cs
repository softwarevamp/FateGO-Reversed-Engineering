namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("FGOAction")]
    public class FGOActorWeaponEffect : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl))]
        public FsmGameObject actorObject;
        public FsmString actorside;
        public FsmString effectname;
        public FsmString nodename;
        public FsmBool sideflip;
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            Debug.Log(" Dont Use");
            base.Finish();
        }
    }
}

