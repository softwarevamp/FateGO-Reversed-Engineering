namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory(ActionCategory.Audio), Tooltip("Plays the Game SE data.")]
    public class PlaySe : FsmStateAction
    {
        [Tooltip("Event to send when the se finishes playing.")]
        public FsmEvent finishedEvent;
        protected SePlayer player;
        [Tooltip("Set the se name string.")]
        public FsmString seName;
        [Tooltip("Set the volume."), HasFloatSlider(0f, 1f)]
        public FsmFloat volume = 1f;

        protected void EndPlaySe()
        {
            if (this.player != null)
            {
                this.player = null;
                if (this.finishedEvent != null)
                {
                    base.Fsm.Event(this.finishedEvent);
                }
            }
        }

        public override void OnEnter()
        {
            this.player = SoundManager.playSe(this.seName.Value, this.volume.Value, new System.Action(this.EndPlaySe));
            base.Finish();
        }

        public override void OnExit()
        {
            if (this.player != null)
            {
                this.player = null;
            }
        }

        public override void Reset()
        {
            this.finishedEvent = null;
            this.player = null;
        }
    }
}

