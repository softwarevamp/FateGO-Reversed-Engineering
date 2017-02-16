namespace WellFired
{
    using System;

    [USequencerEvent("FGO/old/Init Noble Phantasm"), USequencerFriendlyName("FGO Init Noble Phantasm")]
    public class USFGOInitNoblePhantasm : USEventBase
    {
        public override void FireEvent()
        {
            Debug.Log("USFGOInitNoblePhantasm");
            FGOSequenceManager component = base.AffectedObject.GetComponent<FGOSequenceManager>();
            if (component != null)
            {
                Debug.Log("USFGOInitNoblePhantasm 2");
                component.InitNoblePhantasm();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public void Update()
        {
            base.Duration = 0.5f;
        }
    }
}

