namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOActorNodeEffectEx : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("GameObject name to create. Usually a Prefab."), CheckForComponent(typeof(BattleActorControl))]
        public FsmString gameObjectName;
        public FsmString nodename;
        public FsmVector3 position;
        [HutongGames.PlayMaker.Tooltip("Folder for load"), RequiredField]
        public ResourceFolder resourceFolder;
        public FsmGameObject storeObject;
        public FsmGameObject targetObject;

        private GameObject getGameObject()
        {
            switch (this.resourceFolder)
            {
                case ResourceFolder.COMMON_EFFECT:
                    return Resources.Load<GameObject>("Battle/CommonEffects/" + this.gameObjectName);

                case ResourceFolder.ACTOR_EFFECT:
                    return this.targetObject.Value.GetComponent<BattleActorControl>().getActorEffect(this.gameObjectName.Value);

                case ResourceFolder.BATTLE_EFFECT:
                    return Resources.Load<GameObject>("effect/" + this.gameObjectName);
            }
            return null;
        }

        public override void OnEnter()
        {
            GameObject original = FGOActionUtil.getEffectObject(this.resourceFolder, this.gameObjectName.Value, this.targetObject.Value);
            GameObject obj3 = this.targetObject.Value;
            if (original != null)
            {
                Vector3 zero = Vector3.zero;
                Vector3 up = Vector3.up;
                GameObject obj4 = (GameObject) UnityEngine.Object.Instantiate(original, zero, Quaternion.Euler(up));
                if (!this.position.IsNone)
                {
                    zero += this.position.Value;
                }
                Transform transform = obj3.transform.getNodeFromLvName(this.nodename.Value, -1);
                obj4.transform.parent = transform;
                obj4.transform.localPosition = zero;
                obj4.transform.localEulerAngles = up;
                obj4.transform.localScale = new Vector3(1f, 1f, 1f);
                this.storeObject.Value = obj4;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.resourceFolder = ResourceFolder.COMMON_EFFECT;
            this.gameObjectName = string.Empty;
            this.targetObject = null;
            this.nodename = null;
        }
    }
}

