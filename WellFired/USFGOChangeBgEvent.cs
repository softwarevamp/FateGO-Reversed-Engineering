namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Change BG"), USequencerFriendlyName("FGO Change BG")]
    public class USFGOChangeBgEvent : USEventBase
    {
        public string bgName = string.Empty;
        public string bgType = string.Empty;
        public Vector3 localEulerAng = Vector3.zero;
        public Vector3 localPos = Vector3.zero;
        public string oldBgName = "999999";
        public string oldBgType = string.Empty;
        public bool parentCamera;

        public override void FireEvent()
        {
            base.Duration = 1f;
            base.Sequence.Pause();
            SingletonMonoBehaviour<FGOSequenceManager>.Instance.ChangeBg(this.bgName, this.bgType, this.parentCamera, this.localPos, this.localEulerAng, new System.Action(this.OnBgChanged));
        }

        private void OnBgChanged()
        {
            base.Sequence.Play();
            Debug.Log("L:BG Change Done.");
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
        }

        public void Update()
        {
        }
    }
}

