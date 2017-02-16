namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Toggle Component"), USequencerEvent("Object/Toggle Component"), USequencerEventHideDuration]
    public class USEnableComponentEvent : USEventBase
    {
        [SerializeField, HideInInspector]
        private string componentName;
        public bool enableComponent;
        private bool prevEnable;

        public override void FireEvent()
        {
            Behaviour component = base.AffectedObject.GetComponent(this.ComponentName) as Behaviour;
            if (component != null)
            {
                this.prevEnable = component.enabled;
                component.enabled = this.enableComponent;
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
                Behaviour component = base.AffectedObject.GetComponent(this.ComponentName) as Behaviour;
                if (component != null)
                {
                    component.enabled = this.prevEnable;
                }
            }
        }

        public string ComponentName
        {
            get => 
                this.componentName;
            set
            {
                this.componentName = value;
            }
        }
    }
}

