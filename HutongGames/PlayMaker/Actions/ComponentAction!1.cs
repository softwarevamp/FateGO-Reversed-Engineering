namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    public abstract class ComponentAction<T> : FsmStateAction where T: Component
    {
        private GameObject cachedGameObject;
        private T component;

        protected ComponentAction()
        {
        }

        protected bool UpdateCache(GameObject go)
        {
            if (go == null)
            {
                return false;
            }
            if ((this.component == null) || (this.cachedGameObject != go))
            {
                this.component = go.GetComponent<T>();
                this.cachedGameObject = go;
                if (this.component == null)
                {
                    this.LogWarning("Missing component: " + typeof(T).FullName + " on: " + go.name);
                }
            }
            return (this.component != null);
        }

        protected Animation animation =>
            (this.component as Animation);

        protected AudioSource audio =>
            (this.component as AudioSource);

        protected Camera camera =>
            (this.component as Camera);

        protected GUIText guiText =>
            (this.component as GUIText);

        protected GUITexture guiTexture =>
            (this.component as GUITexture);

        protected Light light =>
            (this.component as Light);

        protected NetworkView networkView =>
            (this.component as NetworkView);

        protected Renderer renderer =>
            (this.component as Renderer);

        protected Rigidbody rigidbody =>
            (this.component as Rigidbody);
    }
}

