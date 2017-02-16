using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/ItemSeed")]
public class ItemSeed : MonoBehaviour
{
    [SerializeField]
    protected GameObject parent;
    [SerializeField]
    protected GameObject prefab;

    public Vector3 GetLocalPosition() => 
        this.parent.transform.InverseTransformPoint(this.GetPosition());

    public Vector3 GetPosition() => 
        base.transform.TransformPoint(0f, 0f, 0f);

    public void SetTransform(GameObject obj)
    {
        obj.transform.localPosition = this.GetLocalPosition();
        obj.transform.localRotation = base.transform.localRotation;
        obj.transform.localScale = base.transform.localScale;
        obj.SendMessage("SetBaseTransform");
    }

    public GameObject Parent =>
        this.parent;

    public GameObject Prefab =>
        this.prefab;
}

