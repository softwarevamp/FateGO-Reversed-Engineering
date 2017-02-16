namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ActionCategory("FGOAction")]
    public class FGOBattleCameraParent : FsmStateAction
    {
        [CompilerGenerated]
        private static Func<Transform, bool> <>f__am$cache3;
        public FsmGameObject cameraObject;
        public FsmString nodename;
        public FsmGameObject targetObject;

        public override void OnEnter()
        {
            GameObject obj2 = this.cameraObject.Value;
            GameObject obj3 = this.targetObject.Value;
            if ((obj2 == null) || (obj3 == null))
            {
                goto Label_00F6;
            }
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = p => p.gameObject.name.StartsWith("TAcam");
            }
            IEnumerable<Transform> enumerable = obj3.GetComponentsInChildren<Transform>().Where<Transform>(<>f__am$cache3);
            Transform transform = null;
            IEnumerator<Transform> enumerator = enumerable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    transform = enumerator.Current;
                    goto Label_009B;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        Label_009B:
            obj2.transform.parent = transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
        Label_00F6:
            base.Finish();
        }

        public override void Reset()
        {
            this.cameraObject = null;
            this.targetObject = null;
            this.nodename = string.Empty;
        }
    }
}

