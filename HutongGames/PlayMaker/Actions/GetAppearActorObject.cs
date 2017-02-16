namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class GetAppearActorObject : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject PerformanceObject;
        public SIDE side;
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.PerformanceObject.Value;
            if (obj2 != null)
            {
                BattlePerformance component = obj2.GetComponent<BattlePerformance>();
                if (component != null)
                {
                    BattleData data = component.data;
                    int uniqueId = -1;
                    BattleServantData[] dataArray = null;
                    if (this.side == SIDE.PLAYER)
                    {
                        dataArray = data.getFieldPlayerServantList();
                    }
                    else if (this.side == SIDE.ENEMY)
                    {
                        dataArray = data.getFieldEnemyServantList();
                    }
                    foreach (BattleServantData data2 in dataArray)
                    {
                        if (data2.isAppear)
                        {
                            uniqueId = data2.getUniqueID();
                            break;
                        }
                    }
                    if ((uniqueId == -1) && (0 < dataArray.Length))
                    {
                        uniqueId = dataArray[0].getUniqueID();
                    }
                    if ((uniqueId != -1) && !this.storeObject.IsNone)
                    {
                        this.storeObject.Value = component.getServantGameObject(uniqueId);
                    }
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.storeObject = null;
        }

        public enum SIDE
        {
            PLAYER,
            ENEMY
        }
    }
}

