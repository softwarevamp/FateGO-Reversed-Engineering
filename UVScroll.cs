using System;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    public Vector2 m_Offset = new Vector2(0f, 0f);

    private void Start()
    {
    }

    private void Update()
    {
        this.UpdateUV();
    }

    public void UpdateUV()
    {
        Material material;
        if (Application.isPlaying)
        {
            material = base.gameObject.GetComponent<Renderer>().material;
        }
        else
        {
            material = new Material(base.gameObject.GetComponent<Renderer>().sharedMaterial);
        }
        material.SetTextureOffset("_MainTex", this.m_Offset);
        if (!Application.isPlaying)
        {
            base.gameObject.GetComponent<Renderer>().sharedMaterial = material;
        }
    }
}

