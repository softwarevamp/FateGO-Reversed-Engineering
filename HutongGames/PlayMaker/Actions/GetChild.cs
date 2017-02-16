namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [ActionCategory(ActionCategory.GameObject), HutongGames.PlayMaker.Tooltip("Finds the Child of a GameObject by Name and/or Tag. Use this to find attach points etc. NOTE: This action will search recursively through all children and return the first match; To find a specific child use Find Child.")]
    public class GetChild : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The name of the child to search for.")]
        public FsmString childName;
        [HutongGames.PlayMaker.Tooltip("The GameObject to search."), RequiredField]
        public FsmOwnerDefault gameObject;
        [HutongGames.PlayMaker.Tooltip("Store the result in a GameObject variable."), RequiredField, UIHint(UIHint.Variable)]
        public FsmGameObject storeResult;
        [UIHint(UIHint.Tag), HutongGames.PlayMaker.Tooltip("The Tag to search for. If Child Name is set, both name and Tag need to match.")]
        public FsmString withTag;

        private static GameObject DoGetChildByName(GameObject root, string name, string tag)
        {
            if (root != null)
            {
                IEnumerator enumerator = root.transform.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Transform current = (Transform) enumerator.Current;
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (current.name == name)
                            {
                                if (!string.IsNullOrEmpty(tag))
                                {
                                    if (current.tag.Equals(tag))
                                    {
                                        return current.gameObject;
                                    }
                                }
                                else
                                {
                                    return current.gameObject;
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(tag) && (current.tag == tag))
                        {
                            return current.gameObject;
                        }
                        GameObject obj2 = DoGetChildByName(current.gameObject, name, tag);
                        if (obj2 != null)
                        {
                            return obj2;
                        }
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
            return null;
        }

        public override string ErrorCheck()
        {
            if (string.IsNullOrEmpty(this.childName.Value) && string.IsNullOrEmpty(this.withTag.Value))
            {
                return "Specify Child Name, Tag, or both.";
            }
            return null;
        }

        public override void OnEnter()
        {
            this.storeResult.Value = DoGetChildByName(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.childName.Value, this.withTag.Value);
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.childName = string.Empty;
            this.withTag = "Untagged";
            this.storeResult = null;
        }
    }
}

