namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction"), HutongGames.PlayMaker.Tooltip("Creates a Game Object, usually from a Prefab.")]
    public class FGOActorTreasureEffect : FsmStateAction
    {
        [RequiredField, CheckForComponent(typeof(BattleActorControl)), HutongGames.PlayMaker.Tooltip("actor game object. require: ResourceFolder == ACTORE_EFFECT")]
        public FsmGameObject actorGameObject;
        public FlipMode flipMode;
        [HutongGames.PlayMaker.Tooltip("GameObject name to create. Usually a Prefab.")]
        public FsmString gameObjectName;
        public FsmGameObject parentGameObject;
        [HutongGames.PlayMaker.Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
        public FsmVector3 position;
        [HutongGames.PlayMaker.Tooltip("Folder for load")]
        public ResourceFolder resourceFolder;
        [HutongGames.PlayMaker.Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
        public FsmVector3 rotation;
        public FsmBool sideflip;
        [HutongGames.PlayMaker.Tooltip("Optional Spawn Point.")]
        public FsmGameObject spawnPoint;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Optionally store the created object.")]
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject original = FGOActionUtil.getEffectObject(this.resourceFolder, this.gameObjectName.Value, this.actorGameObject.Value);
            GameObject obj3 = this.parentGameObject.Value;
            if (original != null)
            {
                Vector3 zero = Vector3.zero;
                Vector3 up = Vector3.up;
                if (this.spawnPoint.Value != null)
                {
                    zero = this.spawnPoint.Value.transform.position;
                    if (!this.position.IsNone)
                    {
                        zero += this.position.Value;
                    }
                    up = this.rotation.IsNone ? this.spawnPoint.Value.transform.eulerAngles : this.rotation.Value;
                }
                else
                {
                    if (!this.position.IsNone)
                    {
                        zero = this.position.Value;
                    }
                    if (!this.rotation.IsNone)
                    {
                        up = this.rotation.Value;
                    }
                }
                GameObject obj4 = (GameObject) UnityEngine.Object.Instantiate(original, zero, Quaternion.Euler(up));
                this.storeObject.Value = obj4;
                if (obj3 != null)
                {
                    obj4.transform.parent = obj3.transform;
                }
                obj4.transform.position = zero;
                obj4.transform.eulerAngles = up;
                obj4.transform.localScale = new Vector3(1f, 1f, 1f);
                if (this.sideflip.Value && this.actorGameObject.Value.GetComponent<BattleActorControl>().IsEnemy)
                {
                    switch (this.flipMode)
                    {
                        case FlipMode.Y_ROT:
                            obj4.transform.Rotate((float) 0f, 180f, (float) 0f);
                            break;

                        case FlipMode.X_SCALE:
                        {
                            Vector3 localScale = obj4.transform.localScale;
                            localScale.x *= -1f;
                            obj4.transform.localScale = localScale;
                            break;
                        }
                    }
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.resourceFolder = ResourceFolder.COMMON_EFFECT;
            this.gameObjectName = string.Empty;
            this.parentGameObject = null;
            this.spawnPoint = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.position = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.rotation = vector;
            this.storeObject = null;
            this.sideflip = 0;
            this.flipMode = FlipMode.Y_ROT;
        }
    }
}

