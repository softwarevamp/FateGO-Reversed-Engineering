namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorVoice : FsmStateAction
    {
        public BattlePerformance.ActorGroup actorGroup;
        [CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorObject;
        public FsmInt callRate;
        public FsmEvent FinishedEvent;
        public Voice.BATTLE voice1;
        public Voice.BATTLE voice2;
        public Voice.BATTLE voice3;
        public Voice.BATTLE voice4;
        public Voice.BATTLE voice5;
        public FsmFloat volume = 1f;
        public int weight1;
        public int weight2;
        public int weight3;
        public int weight4;
        public int weight5;

        protected Voice.BATTLE GetVoice(int idx)
        {
            if (idx == 0)
            {
                return this.voice1;
            }
            if (idx == 1)
            {
                return this.voice2;
            }
            if (idx == 2)
            {
                return this.voice3;
            }
            if (idx == 3)
            {
                return this.voice4;
            }
            if (idx == 4)
            {
                return this.voice5;
            }
            return Voice.BATTLE.NONE;
        }

        protected int GetWeight(int idx)
        {
            if (idx == 0)
            {
                return this.weight1;
            }
            if (idx == 1)
            {
                return this.weight2;
            }
            if (idx == 2)
            {
                return this.weight3;
            }
            if (idx == 3)
            {
                return this.weight4;
            }
            if (idx == 4)
            {
                return this.weight5;
            }
            return 0;
        }

        public override void OnEnter()
        {
            GameObject obj2 = this.actorObject.Value;
            int num = this.callRate.Value;
            if (obj2 == null)
            {
                base.Finish();
            }
            else
            {
                int voiceCount = this.VoiceCount;
                Voice.BATTLE[] voices = new Voice.BATTLE[voiceCount];
                int[] weightlist = new int[voiceCount];
                for (int i = 0; i < voiceCount; i++)
                {
                    voices[i] = this.GetVoice(i);
                    weightlist[i] = this.GetWeight(i);
                }
                BattleActorControl component = obj2.GetComponent<BattleActorControl>();
                component.performance.PlayActorsVoice((float) num, component, this.actorGroup, voices, weightlist, this.volume.Value, delegate {
                    if (this.FinishedEvent != null)
                    {
                        base.Fsm.Event(this.FinishedEvent);
                    }
                });
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.volume.Value = 1f;
        }

        public void SetVoice(int idx, Voice.BATTLE voice)
        {
            if (idx == 0)
            {
                this.voice1 = voice;
            }
            if (idx == 1)
            {
                this.voice2 = voice;
            }
            if (idx == 2)
            {
                this.voice3 = voice;
            }
            if (idx == 3)
            {
                this.voice4 = voice;
            }
            if (idx == 4)
            {
                this.voice5 = voice;
            }
        }

        public void SetWeight(int idx, int weight)
        {
            if (idx == 0)
            {
                this.weight1 = weight;
            }
            if (idx == 1)
            {
                this.weight2 = weight;
            }
            if (idx == 2)
            {
                this.weight3 = weight;
            }
            if (idx == 3)
            {
                this.weight4 = weight;
            }
            if (idx == 4)
            {
                this.weight5 = weight;
            }
        }

        protected int VoiceCount
        {
            get
            {
                if (this.voice1 == Voice.BATTLE.NONE)
                {
                    return 0;
                }
                if (this.voice2 == Voice.BATTLE.NONE)
                {
                    return 1;
                }
                if (this.voice3 == Voice.BATTLE.NONE)
                {
                    return 2;
                }
                if (this.voice4 == Voice.BATTLE.NONE)
                {
                    return 3;
                }
                if (this.voice5 == Voice.BATTLE.NONE)
                {
                    return 4;
                }
                return 5;
            }
        }
    }
}

