namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ExAction")]
    public class SetPositionEx : FsmStateAction
    {
        public FsmVector3 addposition;
        public FsmBool flipX = 0;
        [RequiredField]
        public FsmGameObject gameObject;
        public FsmGameObject spawnPoint;

        public override void OnEnter()
        {
            GameObject obj2 = this.gameObject.Value;
            GameObject obj3 = this.spawnPoint.Value;
            Vector3 vector = this.addposition.Value;
            if (obj2 != null)
            {
                obj2.transform.position = obj3.transform.position;
                if (this.addposition != null)
                {
                    vector.x = obj2.transform.localPosition.x + vector.x;
                    vector.y = obj2.transform.localPosition.y + vector.y;
                    vector.z = obj2.transform.localPosition.z + vector.z;
                    obj2.transform.localPosition = vector;
                }
                if (this.flipX.Value)
                {
                    Vector3 position = obj3.transform.position;
                    obj2.transform.position = new Vector3(-position.x, position.y, position.z);
                }
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.addposition = null;
        }
    }
}

