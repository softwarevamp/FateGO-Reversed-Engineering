namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ExAction"), HutongGames.PlayMaker.Tooltip("Creates a Game Object, usually from a Prefab.")]
    public class CreateObjectEx : FsmStateAction
    {
        [RequiredField, HutongGames.PlayMaker.Tooltip("GameObject to create. Usually a Prefab.")]
        public FsmGameObject gameObject;
        [HutongGames.PlayMaker.Tooltip("Usually 0. The group number allows you to group together network messages which allows you to filter them if so desired.")]
        public FsmInt networkGroup;
        [HutongGames.PlayMaker.Tooltip("Use Network.Instantiate to create a Game Object on all clients in a networked game.")]
        public FsmBool networkInstantiate;
        public FsmGameObject parentGameObject;
        [HutongGames.PlayMaker.Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
        public FsmVector3 position;
        [HutongGames.PlayMaker.Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
        public FsmVector3 rotation;
        [HutongGames.PlayMaker.Tooltip("Optional Spawn Point.")]
        public FsmGameObject spawnPoint;
        [UIHint(UIHint.Variable), HutongGames.PlayMaker.Tooltip("Optionally store the created object.")]
        public FsmGameObject storeObject;

        public override void OnEnter()
        {
            GameObject original = this.gameObject.Value;
            GameObject obj3 = this.parentGameObject.Value;
            if (original != null)
            {
                GameObject obj4;
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
                if (!this.networkInstantiate.Value)
                {
                    obj4 = (GameObject) UnityEngine.Object.Instantiate(original, zero, Quaternion.Euler(up));
                }
                else
                {
                    obj4 = (GameObject) Network.Instantiate(original, zero, Quaternion.Euler(up), this.networkGroup.Value);
                }
                this.storeObject.Value = obj4;
                if (obj3 != null)
                {
                    obj4.transform.parent = obj3.transform;
                }
                obj4.transform.position = zero;
                obj4.transform.eulerAngles = up;
                obj4.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
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
            this.networkInstantiate = 0;
            this.networkGroup = 0;
        }
    }
}

