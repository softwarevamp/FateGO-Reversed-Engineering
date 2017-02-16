using System;
using UnityEngine;

public class changeVColor : MonoBehaviour
{
    public Color color;

    private void Update()
    {
        this.UpdateVColor();
    }

    public void UpdateVColor()
    {
        Mesh sharedMesh;
        if (Application.isPlaying)
        {
            sharedMesh = base.GetComponent<MeshFilter>().mesh;
        }
        else
        {
            sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
        }
        Color[] colors = sharedMesh.colors;
        int length = sharedMesh.colors.Length;
        for (int i = 0; i < length; i++)
        {
            colors[i] = this.color;
        }
        sharedMesh.colors = colors;
    }
}

