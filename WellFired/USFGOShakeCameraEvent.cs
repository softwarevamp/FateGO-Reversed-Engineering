namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("FGO Shake Camera"), USequencerEvent("FGO/Camera/Shake Camera")]
    public class USFGOShakeCameraEvent : USEventBase
    {
        private int frameCount;
        public float horizontalShakeSize = 1f;
        public bool isRelative;
        private Transform target;
        private Vector3 targetPosition;
        public float verticalShakeSize = 1f;

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                this.frameCount = 0;
                this.target = base.AffectedObject.transform.parent;
                this.targetPosition = this.target.localPosition;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if ((this.target != null) && (Application.isPlaying || base.Sequence.IsPlaying))
            {
                float x;
                float y;
                float num4 = UnityEngine.Random.Range(0f, this.horizontalShakeSize) * ((Mathf.Cos((3.141593f * this.frameCount) / 1.4f) >= 0f) ? ((float) 1) : ((float) (-1)));
                float num5 = UnityEngine.Random.Range(0f, this.verticalShakeSize) * ((Mathf.Cos((3.141593f * this.frameCount) / 1f) >= 0f) ? ((float) 1) : ((float) (-1)));
                this.frameCount++;
                float z = this.target.localPosition.z;
                if (this.isRelative)
                {
                    x = this.targetPosition.x;
                    y = this.targetPosition.y;
                }
                else
                {
                    x = 0f;
                    y = 0f;
                }
                x += num4;
                y += num5;
                this.target.transform.localPosition = new Vector3(x, y, z);
            }
        }

        public override void StopEvent()
        {
            this.UndoEvent();
        }

        public override void UndoEvent()
        {
            if (base.AffectedObject != null)
            {
                if (this.isRelative)
                {
                    this.target.localPosition = this.targetPosition;
                }
                else
                {
                    this.target.localPosition = Vector3.zero;
                }
            }
        }

        public void Update()
        {
        }
    }
}

