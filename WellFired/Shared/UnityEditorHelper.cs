namespace WellFired.Shared
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UnityEditorHelper : IUnityEditorHelper
    {
        [CompilerGenerated]
        private static Action <>f__am$cache1;
        private Action listeners;

        public UnityEditorHelper()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = new Action(UnityEditorHelper.<listeners>m__262);
            }
            this.listeners = <>f__am$cache1;
        }

        [CompilerGenerated]
        private static void <listeners>m__262()
        {
        }

        public void AddUpdateListener(Action listener)
        {
        }

        public bool IsPrefab(GameObject testObject) => 
            false;

        public void RemoveUpdateListener(Action listener)
        {
        }

        private void Update()
        {
            this.listeners();
        }
    }
}

