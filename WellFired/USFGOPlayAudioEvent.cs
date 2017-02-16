namespace WellFired
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;

    [USequencerEventHideDuration, USequencerEvent("FGO/Play Audio"), USequencerFriendlyName("FGO Play Audio")]
    public class USFGOPlayAudioEvent : USEventBase
    {
        public AudioClip audioClip;
        public AudioCategory category;
        public string groupId;
        public bool loop;
        protected static SePlayer PrevSound;
        public string soundId;
        private bool wasPlaying;

        protected void audioPlay(string assetPath, string assetFile)
        {
            bool flag = false;
            Regex regex = new Regex(@"(\d{6})_(\d)_(.+)", RegexOptions.IgnoreCase);
            BattleSequenceManager instance = SingletonMonoBehaviour<BattleSequenceManager>.Instance;
            if (regex.IsMatch(assetFile))
            {
                Match match = regex.Match(assetFile);
                object[] objArray1 = new object[] { string.Empty, match.Groups[2], "_", match.Groups[3] };
                assetFile = string.Concat(objArray1);
                if (instance != null)
                {
                    GameObject actor = instance.actor;
                    if (actor != null)
                    {
                        BattleActorControl component = actor.GetComponent<BattleActorControl>();
                        if (component.getServantId().ToString().Equals(match.Groups[1].ToString()))
                        {
                            flag = true;
                            if (component.BattleSvtData.SvtLimitAddEnt != null)
                            {
                                object[] objArray2 = new object[] { string.Empty, component.BattleSvtData.SvtLimitAddEnt.voicePrefix, "_", match.Groups[3] };
                                assetFile = string.Concat(objArray2);
                            }
                            else
                            {
                                assetFile = "0_" + match.Groups[3];
                            }
                        }
                    }
                }
            }
            if (assetPath != null)
            {
                if (((PrevSound != null) && flag) && ((instance != null) && instance.IsAccelerateMode))
                {
                    PrevSound.StopSe(0f);
                }
                if (flag)
                {
                    PrevSound = SoundManager.playVoice(assetPath, assetFile, 1f, null);
                }
                else
                {
                    SoundManager.playSe(assetPath, assetFile, 1f, null);
                }
            }
            else
            {
                SoundManager.playSe(assetFile);
            }
        }

        public override void EndEvent()
        {
            this.UndoEvent();
        }

        public override void FireEvent()
        {
            <FireEvent>c__AnonStorey69 storey = new <FireEvent>c__AnonStorey69 {
                <>f__this = this,
                assetPath = this.GetAudioAssetPath(),
                assetFile = this.GetAudioAssetFileName()
            };
            if (this.audioClip == null)
            {
                Debug.Log("PATH:" + storey.assetPath + " FILE:" + storey.assetFile);
                if ((SingletonMonoBehaviour<BattleSequenceManager>.Instance == null) && (storey.assetPath != null))
                {
                    string name = storey.assetPath.Replace("/", "_");
                    SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(name, new System.Action(storey.<>m__74), SoundManager.CueType.ALL);
                }
                else
                {
                    this.audioPlay(storey.assetPath, storey.assetFile);
                }
            }
            else
            {
                AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
                if (component == null)
                {
                    component = base.AffectedObject.AddComponent<AudioSource>();
                    component.playOnAwake = false;
                }
                if (component.clip != this.audioClip)
                {
                    component.clip = this.audioClip;
                }
                component.time = 0f;
                component.loop = this.loop;
                if (base.Sequence.IsPlaying)
                {
                    component.Play();
                }
            }
        }

        protected string GetAudioAssetFileName()
        {
            string str = string.Empty;
            switch (this.category)
            {
            }
            return (str + this.soundId);
        }

        protected string GetAudioAssetPath()
        {
            string str = string.Empty;
            switch (this.category)
            {
                case AudioCategory.ServantNoblePhantasm:
                    return ("NoblePhantasm/" + this.groupId);

                case AudioCategory.ServantBattle:
                    return ("Servants/" + this.groupId);

                case AudioCategory.ServantNormal:
                    return ("ChrVoice/" + this.groupId);

                case AudioCategory.Common:
                    return null;

                case AudioCategory.BattleCommon:
                    return "Battle";

                case AudioCategory.Weapon:
                    return "Battle";
            }
            return str;
        }

        public override void ManuallySetTime(float deltaTime)
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component != null)
            {
                component.time = deltaTime;
            }
        }

        public override void PauseEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            this.wasPlaying = false;
            if ((component != null) && component.isPlaying)
            {
                this.wasPlaying = true;
            }
            if (component != null)
            {
                component.Pause();
            }
        }

        public override void ProcessEvent(float deltaTime)
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component == null)
            {
                component = base.AffectedObject.AddComponent<AudioSource>();
                component.playOnAwake = false;
            }
            if (component.clip != this.audioClip)
            {
                component.clip = this.audioClip;
            }
            if (!component.isPlaying)
            {
                component.time = deltaTime;
                if (base.Sequence.IsPlaying && !component.isPlaying)
                {
                    component.Play();
                }
            }
        }

        public override void ResumeEvent()
        {
            AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
            if (component != null)
            {
                component.time = base.Sequence.RunningTime - base.FireTime;
                if (this.wasPlaying)
                {
                    component.Play();
                }
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
                AudioSource component = base.AffectedObject.GetComponent<AudioSource>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }

        public void Update()
        {
            if (!this.loop && (this.audioClip != null))
            {
                base.Duration = this.audioClip.length;
            }
            else
            {
                base.Duration = -1f;
            }
        }

        [CompilerGenerated]
        private sealed class <FireEvent>c__AnonStorey69
        {
            internal USFGOPlayAudioEvent <>f__this;
            internal string assetFile;
            internal string assetPath;

            internal void <>m__74()
            {
                this.<>f__this.audioPlay(this.assetPath, this.assetFile);
            }
        }

        public enum AudioCategory
        {
            ServantNoblePhantasm,
            ServantBattle,
            ServantNormal,
            Common,
            BattleCommon,
            Weapon
        }
    }
}

