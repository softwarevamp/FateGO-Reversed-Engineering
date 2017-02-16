namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Sleep Rigid Body"), USequencerEvent("Physics/Sleep Rigid Body"), USequencerEventHideDuration]
    public class USSleepRigidBody : USEventBase
    {
        public override void FireEvent()
        {
            Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
            if (component == null)
            {
                Debug.Log("Attempting to Nullify a force on an object, but it has no rigid body from USSleepRigidBody::FireEvent");
            }
            else
            {
                component.Sleep();
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
                    component.WakeUp();
                }
            }
        }
    }
}

