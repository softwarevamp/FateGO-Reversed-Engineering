using System;
using UnityEngine;

public class MapEffectComponent : CommonEffectComponent
{
    [SerializeField]
    private bool isCameraFollow;
    private MapCamera mMapCamera;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        this.UpdateCameraFollow();
    }

    public void Setup(GameObject parent, MapCamera map_camera)
    {
        base.gameObject.SafeSetParent(parent);
        this.mMapCamera = map_camera;
    }

    private void UpdateCameraFollow()
    {
        if (this.isCameraFollow)
        {
            float zoomSize = this.mMapCamera.Zoom.GetZoomSize();
            base.gameObject.SetLocalScale(zoomSize, zoomSize);
            Vector2 scrlPos = this.mMapCamera.Scrl.GetScrlPos();
            base.gameObject.SetLocalPosition(scrlPos);
        }
    }
}

