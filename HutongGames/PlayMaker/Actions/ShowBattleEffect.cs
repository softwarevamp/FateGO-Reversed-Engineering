namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class ShowBattleEffect : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmString attachNodename;
        public FsmInt effectId;
        private GameObject effectObject;
        public FsmBool isParent = 0;
        public FsmBool isWait = 1;
        public FsmVector3 position;
        public FsmBool sideflip;
        public FsmGameObject storeObject;
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject procObject = this.actObject.Value;
            GameObject obj3 = this.targetObject.Value;
            string nodename = null;
            if (!this.attachNodename.IsNone)
            {
                nodename = this.attachNodename.Value;
            }
            int effectId = this.effectId.Value;
            if ((procObject != null) && (obj3 != null))
            {
                BattleActorControl component = procObject.GetComponent<BattleActorControl>();
                this.effectObject = BattleEffectUtility.getEffectObject(effectId, procObject);
                if (this.effectObject == null)
                {
                    Debug.Log(" no Effect : " + effectId);
                }
                Transform transform = obj3.transform.getNodeFromLvName(nodename, -1);
                this.effectObject.transform.parent = transform;
                Vector3 zero = Vector3.zero;
                if (!this.position.IsNone)
                {
                    zero += this.position.Value;
                }
                this.effectObject.transform.localPosition = Vector3.zero;
                this.effectObject.transform.eulerAngles = Vector3.up;
                this.effectObject.transform.localScale = new Vector3(1f, 1f, 1f);
                if (((component != null) && this.sideflip.Value) && component.IsEnemy)
                {
                    this.effectObject.transform.Rotate((float) 0f, 180f, (float) 0f);
                }
                if (!this.isParent.Value)
                {
                    this.effectObject.transform.parent = component.getFieldRoot();
                }
                Transform transform1 = this.effectObject.transform;
                transform1.localPosition += zero;
                this.effectObject.transform.localScale = new Vector3(1f, 1f, 1f);
                this.effectObject.SetActive(true);
                if (!this.storeObject.IsNone)
                {
                    this.storeObject.Value = this.effectObject;
                }
            }
            base.Finish();
        }

        public override void OnUpdate()
        {
            if (this.effectObject == null)
            {
                base.Finish();
            }
        }
    }
}

