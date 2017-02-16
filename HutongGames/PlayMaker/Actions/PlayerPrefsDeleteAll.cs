namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [HutongGames.PlayMaker.Tooltip("Removes all keys and values from the preferences. Use with caution."), ActionCategory("PlayerPrefs")]
    public class PlayerPrefsDeleteAll : FsmStateAction
    {
        public override void OnEnter()
        {
            PlayerPrefs.DeleteAll();
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

