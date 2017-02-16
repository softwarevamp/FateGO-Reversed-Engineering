namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Input), HutongGames.PlayMaker.Tooltip("Resets all Input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame")]
    public class ResetInputAxes : FsmStateAction
    {
        public override void OnEnter()
        {
            Input.ResetInputAxes();
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

