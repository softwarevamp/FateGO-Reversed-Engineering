namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Sets the value of the preference identified by key."), ActionCategory("PlayerPrefs")]
    public class PlayerPrefsSetInt : FsmStateAction
    {
        [CompoundArray("Count", "Key", "Value"), HutongGames.PlayMaker.Tooltip("Case sensitive key.")]
        public FsmString[] keys;
        public FsmInt[] values;

        public override void OnEnter()
        {
            for (int i = 0; i < this.keys.Length; i++)
            {
                if (!this.keys[i].IsNone || !this.keys[i].Value.Equals(string.Empty))
                {
                    PlayerPrefs.SetInt(this.keys[i].Value, !this.values[i].IsNone ? this.values[i].Value : 0);
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.keys = new FsmString[1];
            this.values = new FsmInt[1];
        }
    }
}

