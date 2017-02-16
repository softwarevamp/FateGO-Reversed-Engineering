namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Loads the Game System BGM data."), ActionCategory(ActionCategory.Audio)]
    public class LoadBgm : FsmStateAction
    {
        [ObjectType(typeof(AudioClip)), HutongGames.PlayMaker.Tooltip("Set the bgm name string.")]
        public FsmString bgmName;
        [HutongGames.PlayMaker.Tooltip("Event to send when the bgm finishes loading.")]
        public FsmEvent finishedEvent;

        protected void EndLoadBgm()
        {
            if (this.finishedEvent != null)
            {
                base.Fsm.Event(this.finishedEvent);
            }
        }

        public override void OnEnter()
        {
            SoundManager.loadBgm(this.bgmName.Value, new System.Action(this.EndLoadBgm));
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

