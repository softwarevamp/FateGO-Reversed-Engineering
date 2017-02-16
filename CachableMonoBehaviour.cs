using UnityEngine;

public class CachableMonoBehaviour : MonoBehaviour
{
    private Renderer mRenderer;
    private Rigidbody mRigidbody;
    private Transform mTransform;

    public Renderer renderer
    {
        get
        {
            if (this.mRenderer == null)
            {
                this.mRenderer = base.GetComponent<Renderer>();
            }
            return this.mRenderer;
        }
    }

    public Rigidbody rigidbody
    {
        get
        {
            if (this.mRigidbody == null)
            {
                this.mRigidbody = base.GetComponent<Rigidbody>();
            }
            return this.mRigidbody;
        }
    }

    public Transform transform
    {
        get
        {
            if (this.mTransform == null)
            {
                this.mTransform = base.GetComponent<Transform>();
            }
            return this.mTransform;
        }
    }
}

