﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Network), HutongGames.PlayMaker.Tooltip("Set the level prefix which will then be prefixed to all network ViewID numbers.\n\nThis prevents old network updates from straying into a new level from the previous level.\n\nThis can be set to any number and then incremented with each new level load. This doesn't add overhead to network traffic but just diminishes the pool of network ViewID numbers a little bit.")]
    public class NetworkSetLevelPrefix : FsmStateAction
    {
        [RequiredField, UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("The level prefix which will then be prefixed to all network ViewID numbers.")]
        public FsmInt levelPrefix;

        public override void OnEnter()
        {
            if (this.levelPrefix.IsNone)
            {
                this.LogError("Network LevelPrefix not set");
            }
            else
            {
                Network.SetLevelPrefix(this.levelPrefix.Value);
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.levelPrefix = null;
        }
    }
}

