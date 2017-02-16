namespace WellFired
{
    using System;
    using UnityEngine;

    [USequencerEvent("FGO/Play Audio MissFunction"), USequencerEventHideDuration, USequencerFriendlyName("FGO Play Audio Miss Function")]
    public class USFGOPlayAudioEventMissFunction : USFGOPlayAudioEvent
    {
        public string missFuncList;

        public override void FireEvent()
        {
            string audioAssetPath = base.GetAudioAssetPath();
            string audioAssetFileName = base.GetAudioAssetFileName();
            if (base.audioClip == null)
            {
                Debug.Log("PATH:" + audioAssetPath + " FILE:" + audioAssetFileName);
                if (0 < this.missFuncList.Length)
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = this.missFuncList.Split(separator);
                    int[] numArray = new int[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (!int.TryParse(strArray[i], out numArray[i]))
                        {
                            Debug.Log("Play Audio Miss Function : 数値の指定が間違っています");
                            return;
                        }
                    }
                    BattleSequenceManager instance = SingletonMonoBehaviour<BattleSequenceManager>.Instance;
                    if (instance != null)
                    {
                        GameObject actor = instance.actor;
                        if (actor != null)
                        {
                            BattleActorControl component = actor.GetComponent<BattleActorControl>();
                            if (component != null)
                            {
                                BattleActionData.BuffData[] dataArray = component.ActionData.buffdatalist.ToArray();
                                int num2 = 0;
                                foreach (BattleActionData.BuffData data in dataArray)
                                {
                                    if (data.isMiss)
                                    {
                                        foreach (int num4 in numArray)
                                        {
                                            if (data.functionIndex == num4)
                                            {
                                                num2++;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (num2 == numArray.Length)
                                {
                                    base.audioPlay(audioAssetPath, audioAssetFileName);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

