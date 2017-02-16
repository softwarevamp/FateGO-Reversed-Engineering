namespace WellFired
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using WellFired.Shared;

    [USequencerFriendlyName("Dissolve Transition"), USequencerEvent("Camera/Transition/Dissolve"), ExecuteInEditMode]
    public class USCameraDissolveTransition : USEventBase
    {
        [SerializeField]
        private Camera destinationCamera;
        [SerializeField]
        private Camera sourceCamera;
        private BaseTransition transition;

        public override void EndEvent()
        {
            if (((this.sourceCamera != null) && (this.destinationCamera != null)) && (this.transition != null))
            {
                this.transition.TransitionComplete();
            }
        }

        public override void FireEvent()
        {
            if (this.transition == null)
            {
                this.transition = new BaseTransition();
            }
            if (((this.sourceCamera == null) || (this.destinationCamera == null)) || (this.transition == null))
            {
                Debug.LogError("Can't continue this transition with null cameras.");
            }
            else
            {
                this.transition.InitializeTransition(this.sourceCamera, this.destinationCamera, new List<Camera>(), new List<Camera>(), TypeOfTransition.Dissolve);
            }
        }

        private void OnGUI()
        {
            if (((this.sourceCamera != null) && (this.destinationCamera != null)) && (this.transition != null))
            {
                this.transition.ProcessTransitionFromOnGUI();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            if (((this.sourceCamera != null) && (this.destinationCamera != null)) && (this.transition != null))
            {
                this.transition.ProcessEventFromNoneOnGUI(deltaTime, base.Duration);
            }
        }

        public override void StopEvent()
        {
            if (((this.sourceCamera != null) && (this.destinationCamera != null)) && (this.transition != null))
            {
                this.UndoEvent();
            }
        }

        public override void UndoEvent()
        {
            if (((this.sourceCamera != null) && (this.destinationCamera != null)) && (this.transition != null))
            {
                this.transition.RevertTransition();
            }
        }
    }
}

