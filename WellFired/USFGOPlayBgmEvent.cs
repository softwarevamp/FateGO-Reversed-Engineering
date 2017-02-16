namespace WellFired
{
    using System;

    [USequencerFriendlyName("FGO Play BGM"), USequencerEventHideDuration, USequencerEvent("FGO/Play BGM")]
    public class USFGOPlayBgmEvent : USEventBase
    {
        public string bgmName;
        public float fadeTime;
        public float volume = 1f;

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            if (SingletonMonoBehaviour<BattleSequenceManager>.Instance != null)
            {
                SingletonMonoBehaviour<BattleSequenceManager>.Instance.BackupBgmTime = SingletonMonoBehaviour<BgmManager>.Instance.PlayTime;
            }
            SoundManager.playBgm(this.bgmName, this.volume, this.fadeTime);
        }

        public override void PauseEvent()
        {
        }

        public override void ProcessEvent(float deltaTime)
        {
        }

        public override void ResumeEvent()
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

