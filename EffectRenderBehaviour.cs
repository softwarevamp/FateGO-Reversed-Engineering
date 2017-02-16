using System;
using UnityEngine;

[ExecuteInEditMode]
public class EffectRenderBehaviour : MonoBehaviour
{
    private bool mbCustomShader;

    private Shader _getCustomShader(Shader sh) => 
        Shader.Find(sh.name + "(Custom)");

    private Shader _getNonCustomShader(Shader sh)
    {
        if (sh == null)
        {
            return null;
        }
        if (!this._isCustomShader(sh))
        {
            return sh;
        }
        return Shader.Find(sh.name.Replace("(Custom)", string.Empty));
    }

    private bool _isCustomShader(Shader sh) => 
        (sh?.name.IndexOf("(Custom)") >= 0);

    public void Awake()
    {
        if (base.GetComponent<ParticleSystem>() != null)
        {
            this.mbCustomShader = false;
            ParticleSystemRenderer component = (ParticleSystemRenderer) base.GetComponent<ParticleSystem>().GetComponent<Renderer>();
            Material material = component.material;
            if (!this._isCustomShader(material.shader))
            {
                Shader shader = this._getCustomShader(material.shader);
                if (shader != null)
                {
                    material.shader = shader;
                    this.mbCustomShader = true;
                }
                else
                {
                    this.mbCustomShader = false;
                }
            }
            else
            {
                this.mbCustomShader = true;
            }
        }
    }

    public void OnDestroy()
    {
    }

    public void OnWillRenderObject()
    {
        if ((base.GetComponent<ParticleSystem>() != null) && this.mbCustomShader)
        {
            ParticleSystemRenderer component = (ParticleSystemRenderer) base.GetComponent<ParticleSystem>().GetComponent<Renderer>();
            component.material.SetVector("_Center", base.transform.position);
            component.material.SetVector("_Scaling", base.transform.lossyScale);
            component.material.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
            component.material.SetMatrix("_CameraInv", Camera.current.worldToCameraMatrix.inverse);
            component.material.SetInt("_RenderType", (int) component.renderMode);
        }
    }
}

