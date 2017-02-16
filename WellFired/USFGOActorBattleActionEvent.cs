namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Character/Battle Action"), USequencerFriendlyName("FGO Actor Battle Action")]
    public class USFGOActorBattleActionEvent : USEventBase
    {
        public ActionType actionType;
        public string attachNodeName = "en_waist";
        public int countValue = 1;
        public int criticalEffectId = -1;
        public Vector3 damageNumberOffset = new Vector3(0f, 0f, 0f);
        public int effectId = -1;
        public int functionIndex = -1;
        public bool isNoDamageMotion;
        public bool isRandomDamagePosition = true;
        public int startValue;

        public override void FireEvent()
        {
            if (base.AffectedObject != null)
            {
                BattleActorControl component = base.AffectedObject.GetComponent<BattleActorControl>();
                if (((component == null) && (SingletonMonoBehaviour<BattleSequenceManager>.Instance != null)) && (SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor != null))
                {
                    component = SingletonMonoBehaviour<BattleSequenceManager>.Instance.actor.GetComponent<BattleActorControl>();
                }
                if (component != null)
                {
                    Debug.Log(string.Concat(new object[] { "    >>>>> actor is not null : ", this.startValue, " - ", this.countValue }));
                    if (SingletonMonoBehaviour<BattleSequenceManager>.Instance != null)
                    {
                        BattlePerformance performance = SingletonMonoBehaviour<BattleSequenceManager>.Instance.Performance;
                        switch (this.actionType)
                        {
                            case ActionType.Damage:
                                performance.ShowDamage(component.gameObject, this.effectId, this.criticalEffectId, this.attachNodeName, this.functionIndex, this.startValue, this.countValue, this.isRandomDamagePosition, this.damageNumberOffset, this.isNoDamageMotion);
                                break;

                            case ActionType.BuffDebuff:
                                performance.ShowBuff(component.gameObject, this.functionIndex);
                                break;

                            case ActionType.Heal:
                                performance.showHeal(component.gameObject, this.functionIndex);
                                break;

                            case ActionType.DamageVoice:
                                performance.callNpDamageVoice();
                                break;
                        }
                    }
                }
                else
                {
                    Debug.Log("    >>>>> actor is null");
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
            if (base.AffectedObject == null)
            {
            }
        }

        public enum ActionType
        {
            Damage,
            BuffDebuff,
            Heal,
            DamageVoice
        }
    }
}

