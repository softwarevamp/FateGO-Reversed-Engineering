namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Audio), HutongGames.PlayMaker.Tooltip("Plays the Game System SE data.")]
    public class PlaySystemSe : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("Set the system se kind string."), ObjectType(typeof(AudioClip))]
        public SeManager.SystemSeKind kind;
        [HutongGames.PlayMaker.Tooltip("Set the volume."), HasFloatSlider(0f, 1f)]
        public FsmFloat volume = 1f;

        public override void OnEnter()
        {
            SoundManager.playSystemSe(this.kind);
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

