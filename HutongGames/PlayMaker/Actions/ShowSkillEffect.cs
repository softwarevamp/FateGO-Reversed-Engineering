namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class ShowSkillEffect : FsmStateAction
    {
        [RequiredField]
        public FsmGameObject actObject;
        public FsmString attachNodename;
        public FsmInt effectIndex = 0;
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
            string nodename = this.attachNodename.Value;
            int index = this.effectIndex.Value;
            if ((procObject != null) && (obj3 != null))
            {
                BattleActionData actionData = null;
                BattleActorControl component = procObject.GetComponent<BattleActorControl>();
                if (component != null)
                {
                    actionData = component.ActionData;
                }
                if (actionData != null)
                {
                    int effectId = actionData.getEffect(index);
                    if (effectId == -1)
                    {
                        base.Finish();
                        return;
                    }
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
                    this.effectObject.transform.localPosition = zero;
                    this.effectObject.transform.localEulerAngles = Vector3.up;
                    this.effectObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    if (!this.isParent.Value)
                    {
                        this.effectObject.transform.parent = component.getFieldRoot();
                    }
                    this.effectObject.SetActive(true);
                    if (!this.storeObject.IsNone)
                    {
                        this.storeObject.Value = this.effectObject;
                    }
                }
            }
            if (!this.isWait.Value)
            {
                base.Finish();
            }
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

