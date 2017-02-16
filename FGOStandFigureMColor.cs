using System;
using UnityEngine;

public class FGOStandFigureMColor : MonoBehaviour
{
    protected Color backupColor = Color.red;
    public Color color = Color.white;
    public MeshRenderer[] renderers;

    public void OnUpdate()
    {
        if (this.backupColor != this.color)
        {
            this.backupColor = this.color;
            if (this.renderers != null)
            {
                foreach (MeshRenderer renderer in this.renderers)
                {
                    renderer.material.color = this.color;
                }
            }
        }
    }

    private void Start()
    {
        this.backupColor = this.color;
    }

    private void Update()
    {
        this.OnUpdate();
    }
}

