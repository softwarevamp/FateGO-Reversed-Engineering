namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOFade : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject cameraObject;
        [RequiredField]
        public float duration;
        [RequiredField]
        public Color fadeColor;
        public FsmEvent finishEvent;
        [RequiredField]
        public bool isFadeIn;
        public bool isNotWait;

        public override void OnEnter()
        {
            this.cameraObject = base.Fsm.Variables.GetFsmGameObject("CameraFsm");
            GameObject obj2 = this.cameraObject.Value;
            if (obj2 != null)
            {
                obj2.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmGameObject("FadeObject").Value.GetComponent<NGUIFader>().FadeStart(this.fadeColor, this.duration, this.isFadeIn, new NGUIFader.OnFinished(this.OnFinished), false);
            }
            if (this.isNotWait)
            {
                base.Finish();
                if (this.finishEvent != null)
                {
                    base.Fsm.Event(this.finishEvent);
                }
            }
        }

        private void OnFinished()
        {
            Debug.Log("L:FINISHED!");
            if (!this.isNotWait)
            {
                base.Finish();
                if (this.finishEvent != null)
                {
                    base.Fsm.Event(this.finishEvent);
                }
            }
        }

        public override void Reset()
        {
            this.fadeColor = Color.black;
            this.duration = 1f;
            this.isFadeIn = false;
            this.finishEvent = null;
            this.isNotWait = false;
        }
    }
}

