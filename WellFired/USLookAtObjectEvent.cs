namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Look At Object"), USequencerEvent("Transform/Look At Object")]
    public class USLookAtObjectEvent : USEventBase
    {
        public AnimationCurve inCurve;
        public float lookAtTime;
        public GameObject objectToLookAt;
        public AnimationCurve outCurve;
        private Quaternion previousRotation;
        private Quaternion sourceOrientation;

        public USLookAtObjectEvent()
        {
            Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) };
            this.inCurve = new AnimationCurve(keys);
            Keyframe[] keyframeArray2 = new Keyframe[] { new Keyframe(0f, 1f), new Keyframe(1f, 0f) };
            this.outCurve = new AnimationCurve(keyframeArray2);
            this.lookAtTime = 2f;
            this.sourceOrientation = Quaternion.identity;
            this.previousRotation = Quaternion.identity;
        }

        public override void FireEvent()
        {
            if (this.objectToLookAt == null)
            {
                Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
            }
            else
            {
                this.previousRotation = base.AffectedObject.transform.rotation;
                this.sourceOrientation = base.AffectedObject.transform.rotation;
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (this.objectToLookAt == null)
            {
                Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
            }
            else
            {
                Keyframe keyframe = this.inCurve[this.inCurve.length - 1];
                float time = keyframe.time;
                float num2 = this.lookAtTime + time;
                float t = 1f;
                if (deltaTime <= time)
                {
                    t = Mathf.Clamp(this.inCurve.Evaluate(deltaTime), 0f, 1f);
                }
                else if (deltaTime >= num2)
                {
                    t = Mathf.Clamp(this.outCurve.Evaluate(deltaTime - num2), 0f, 1f);
                }
                Vector3 position = base.AffectedObject.transform.position;
                Vector3 forward = this.objectToLookAt.transform.position - position;
                Quaternion b = Quaternion.LookRotation(forward);
                base.AffectedObject.transform.rotation = Quaternion.Slerp(this.sourceOrientation, b, t);
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
                base.AffectedObject.transform.rotation = this.previousRotation;
            }
        }
    }
}

