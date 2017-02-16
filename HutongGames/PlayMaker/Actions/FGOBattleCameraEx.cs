namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOBattleCameraEx : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("actor game object. require: ResourceFolder == ACTORE_EFFECT"), CheckForComponent(typeof(BattleActorControl)), RequiredField]
        public FsmGameObject actorGameObject;
        [RequiredField]
        public FsmGameObject cameraObject;
        public FsmString eventname;
        public FlipMode flipMode;
        [HutongGames.PlayMaker.Tooltip("Camera GameObject name to create. Usually a Prefab.")]
        public FsmString gameObjectName;
        public FsmGameObject parentGameObject;
        [HutongGames.PlayMaker.Tooltip("Position. If a Spawn Point is defined, this is used as a local offset from the Spawn Point position.")]
        public FsmVector3 position;
        [HutongGames.PlayMaker.Tooltip("Folder for load")]
        public ResourceFolder resourceFolder;
        [HutongGames.PlayMaker.Tooltip("Rotation. NOTE: Overrides the rotation of the Spawn Point.")]
        public FsmVector3 rotation;
        public FsmString side;
        public FsmBool sideflip;
        [HutongGames.PlayMaker.Tooltip("Optional Spawn Point.")]
        public FsmGameObject spawnPoint;

        public override void OnEnter()
        {
            GameObject original = FGOActionUtil.getEffectObject(this.resourceFolder, this.gameObjectName.Value, this.actorGameObject.Value);
            GameObject obj3 = this.cameraObject.Value;
            if ((obj3 != null) && (original != null))
            {
                Vector3 zero = Vector3.zero;
                Vector3 up = Vector3.up;
                Debug.Log("L:FGOBattleCameraEx");
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
                obj4.transform.position = zero;
                obj4.transform.eulerAngles = up;
                obj4.transform.localScale = new Vector3(1f, 1f, 1f);
                obj4.transform.Rotate((float) 0f, 90f, (float) 0f);
                if (this.sideflip.Value && this.actorGameObject.Value.GetComponent<BattleActorControl>().IsEnemy)
                {
                    Vector3 localScale;
                    switch (this.flipMode)
                    {
                        case FlipMode.Y_ROT:
                            obj4.transform.Rotate((float) 0f, 180f, (float) 0f);
                            break;

                        case FlipMode.X_SCALE:
                            localScale = obj4.transform.localScale;
                            localScale.x *= -1f;
                            obj4.transform.localScale = localScale;
                            break;

                        case FlipMode.Z_SCALE:
                            localScale = obj4.transform.localScale;
                            localScale.z *= -1f;
                            obj4.transform.localScale = localScale;
                            break;
                    }
                }
                GameObject obj5 = this.parentGameObject.Value;
                if (obj5 != null)
                {
                    obj4.transform.parent = obj5.transform;
                }
                PlayMakerFSM component = obj3.GetComponent<PlayMakerFSM>();
                component.FsmVariables.GetFsmGameObject("AnimCameraObject").Value = obj4;
                if (!this.side.IsNone)
                {
                    Debug.Log("L:Camera:NOBLE_ANIM" + this.side.Value);
                    component.SendEvent("NOBLE_ANIM" + this.side.Value);
                }
                else
                {
                    Debug.Log("L:Camera: NOBLE_ANIM_PLAYER");
                    component.SendEvent("NOBLE_ANIM_PLAYER");
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.cameraObject = null;
            this.actorGameObject = null;
            this.resourceFolder = ResourceFolder.ACTOR_EFFECT;
            this.gameObjectName = string.Empty;
            this.spawnPoint = null;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.position = vector;
            vector = new FsmVector3 {
                UseVariable = true
            };
            this.rotation = vector;
            this.sideflip = 0;
            this.flipMode = FlipMode.Y_ROT;
            this.side = string.Empty;
            this.eventname = string.Empty;
        }
    }
}

