namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Preloads the Game System BGM data."), ActionCategory(ActionCategory.Audio)]
    public class PreloadBgm : FsmStateAction
    {
        [ObjectType(typeof(AudioClip)), HutongGames.PlayMaker.Tooltip("Set the bgm name string.")]
        public FsmString bgmName;
        [HutongGames.PlayMaker.Tooltip("Event to send when the bgm finishes preloading.")]
        public FsmEvent finishedEvent;

        protected void EndLoadBgm()
        {
            base.Finish();
        }

        public override void OnEnter()
        {
            SoundManager.loadBgm(this.bgmName.Value, new System.Action(this.EndLoadBgm));
        }

        public override void OnExit()
        {
            if (this.finishedEvent != null)
            {
                base.Fsm.Event(this.finishedEvent);
            }
        }

        public override void Reset()
        {
        }
    }
}

