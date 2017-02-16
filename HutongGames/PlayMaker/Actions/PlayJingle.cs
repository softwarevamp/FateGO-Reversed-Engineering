namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Plays the Game System Jingle data."), ActionCategory(ActionCategory.Audio)]
    public class PlayJingle : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Event to send when the se finishes playing.")]
        public FsmEvent finishedEvent;
        [HutongGames.PlayMaker.Tooltip("Set the jingle name string."), ObjectType(typeof(AudioClip))]
        public FsmString jingleName;
        [HasFloatSlider(0f, 1f), HutongGames.PlayMaker.Tooltip("Set the volume.")]
        public FsmFloat volume = 1f;

        protected void EndPlayJingle()
        {
            if (this.finishedEvent != null)
            {
                base.Fsm.Event(this.finishedEvent);
            }
        }

        public override void OnEnter()
        {
            SoundManager.playJingle(this.jingleName.Value, this.volume.Value, new System.Action(this.EndPlayJingle));
            base.Finish();
        }

        public override void Reset()
        {
            this.finishedEvent = null;
        }
    }
}

