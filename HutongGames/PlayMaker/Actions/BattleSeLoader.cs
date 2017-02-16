namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("BattleSeLoader - load or download Battle Se."), ActionCategory(ActionCategory.Audio)]
    public class BattleSeLoader : FsmStateAction
    {
        [Tooltip("Store the result in an Object variable of type AssetData.")]
        public FsmObject assetData;
        [Tooltip("Set the Battle Se name string(Common,CommonWeapon,Weapon01,etc...)")]
        public FsmString categoryName;
        [Tooltip("Optionally send an Event when the load finishes.")]
        public FsmEvent finishEvent;
        [Tooltip("If checked, only download. ( usually, download and load to memory.")]
        public FsmBool isDownloadOnly;

        public override void OnEnter()
        {
            Debug.Log("L:AssetManagerLoader onEnter");
            if (!this.isDownloadOnly.Value)
            {
                Debug.Log("L:AssetManagerLoader load");
            }
            if (this.finishEvent != null)
            {
                base.Fsm.Event(this.finishEvent);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.categoryName = string.Empty;
            this.isDownloadOnly = 0;
            this.finishEvent = null;
        }
    }
}

