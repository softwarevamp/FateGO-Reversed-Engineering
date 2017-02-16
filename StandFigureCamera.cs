using System;
using UnityEngine;

public class StandFigureCamera : MonoBehaviour
{
    [SerializeField]
    protected StandFigureManager manager;
    protected RenderTexture recycleTexture;
    protected State state;

    protected void OnPostRender()
    {
        if (this.state == State.RENDER)
        {
            RenderTexture targetTexture = base.GetComponent<Camera>().targetTexture;
            if (!targetTexture.IsCreated())
            {
                Debug.LogWarning("Wait Render Texture Create");
            }
            else
            {
                Debug.Log("StandFigureCamera:CreateRenderTeture");
                base.gameObject.SetActive(false);
                if (this.recycleTexture != null)
                {
                    base.GetComponent<Camera>().targetTexture = this.recycleTexture;
                    this.recycleTexture = null;
                }
                else
                {
                    base.GetComponent<Camera>().targetTexture = RenderTexture.GetTemporary(0x400, 0x400, 0, RenderTextureFormat.ARGB32);
                }
                base.GetComponent<Camera>().targetTexture.name = "RednerTexture";
                targetTexture.name = "TempStandFigurenRednerTexture";
                this.manager.OnRenderEnd(targetTexture);
                this.state = State.IDLE;
            }
        }
    }

    protected void OnPreRender()
    {
        if (this.state == State.START)
        {
            this.state = State.RENDER;
        }
    }

    public bool Request(RenderTexture recycleTexture)
    {
        Debug.Log("StandFigureCamera:RequestRenderTexture " + this.state);
        if (this.state != State.IDLE)
        {
            return false;
        }
        this.state = State.START;
        this.recycleTexture = base.GetComponent<Camera>().targetTexture;
        if (this.recycleTexture != null)
        {
            RenderTexture.ReleaseTemporary(this.recycleTexture);
        }
        base.gameObject.SetActive(true);
        base.GetComponent<Camera>().aspect = 1f;
        base.GetComponent<Camera>().targetTexture = RenderTexture.GetTemporary(0x400, 0x400, 0, RenderTextureFormat.ARGB32);
        base.GetComponent<Camera>().targetTexture.name = "RednerTexture";
        if (recycleTexture != null)
        {
            recycleTexture.name = "RecycleRednerTexture";
            this.recycleTexture = recycleTexture;
        }
        else
        {
            this.recycleTexture = null;
        }
        return true;
    }

    protected enum State
    {
        IDLE,
        START,
        RENDER,
        SEND,
        ERROR
    }
}

