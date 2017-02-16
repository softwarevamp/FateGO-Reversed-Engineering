using System;
using UnityEngine;

public class FadeOutScript : MonoBehaviour
{
    private Color alpha = new Color(0f, 0f, 0f, 0.01f);

    private void Start()
    {
    }

    private void Update()
    {
        if (base.GetComponent<Renderer>().material.color.a >= 0f)
        {
            Material material = base.GetComponent<Renderer>().material;
            material.color -= this.alpha;
        }
    }
}

