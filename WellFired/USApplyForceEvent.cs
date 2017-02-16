namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Apply Force"), USequencerEvent("Physics/Apply Force"), USequencerEventHideDuration]
    public class USApplyForceEvent : USEventBase
    {
        public Vector3 direction = Vector3.up;
        private Transform previousTransform;
        public float strength = 1f;
        public ForceMode type = ForceMode.Impulse;

        public override void FireEvent()
        {
            Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
            if (component == null)
            {
                Debug.Log("Attempting to apply an impulse to an object, but it has no rigid body from USequencerApplyImpulseEvent::FireEvent");
            }
            else
            {
                component.AddForceAtPosition((Vector3) (this.direction * this.strength), base.transform.position, this.type);
                this.previousTransform = component.transform;
            }
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
            if (base.AffectedObject != null)
            {
                Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
                if (component != null)
                {
                    component.Sleep();
                    if (this.previousTransform != null)
                    {
                        base.AffectedObject.transform.position = this.previousTransform.position;
                        base.AffectedObject.transform.rotation = this.previousTransform.rotation;
                    }
                }
            }
        }
    }
}

