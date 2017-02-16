namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerFriendlyName("Toggle Mesh Renderer"), USequencerEventHideDuration, USequencerEvent("Render/Toggle Mesh Renderer")]
    public class USMeshRenderDisable : USEventBase
    {
        public bool enable;
        private bool previousEnable;

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                MeshRenderer component = base.AffectedObject.GetComponent<MeshRenderer>();
                if (component == null)
                {
                    Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
                }
                else
                {
                    this.previousEnable = component.enabled;
                    component.enabled = this.enable;
                }
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
                MeshRenderer component = base.AffectedObject.GetComponent<MeshRenderer>();
                if (component == null)
                {
                    Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
                }
                else
                {
                    component.enabled = this.previousEnable;
                }
            }
        }
    }
}

