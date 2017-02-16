using System;
using UnityEngine;

public class MaskWithOpening : MonoBehaviour
{
    [SerializeField]
    private BoxCollider mask1;
    [SerializeField]
    private BoxCollider mask2;
    [SerializeField]
    private BoxCollider mask3;
    [SerializeField]
    private BoxCollider mask4;

    public void SetBlock()
    {
        this.mask1.enabled = true;
        this.mask1.center = new Vector3(0f, 0f, 0f);
        this.mask1.size = new Vector3((float) ManagerConfig.WIDTH, (float) ManagerConfig.HEIGHT, 0f);
        this.mask2.enabled = false;
        this.mask3.enabled = false;
        this.mask4.enabled = false;
    }

    public void SetDepth(int depth)
    {
        base.GetComponent<UIPanel>().depth = depth;
    }

    public void SetOpening(Rect hole, int depth)
    {
        this.SetDepth(depth);
        this.mask1.enabled = true;
        this.mask2.enabled = true;
        this.mask3.enabled = true;
        this.mask4.enabled = true;
        float wIDTH = ManagerConfig.WIDTH;
        float hEIGHT = ManagerConfig.HEIGHT;
        this.mask1.center = new Vector3(0f, -(hEIGHT / 2f) + ((hole.yMin + (hEIGHT / 2f)) / 2f), 0f);
        this.mask1.size = new Vector3(wIDTH, hole.yMin + (hEIGHT / 2f), 0f);
        this.mask2.center = new Vector3(0f, (hEIGHT / 2f) - (((hEIGHT / 2f) - hole.yMax) / 2f), 0f);
        this.mask2.size = new Vector3(wIDTH, (hEIGHT / 2f) - hole.yMax, 0f);
        this.mask3.center = new Vector3(-(wIDTH / 2f) + ((hole.xMin + (wIDTH / 2f)) / 2f), hole.center.y, 0f);
        this.mask3.size = new Vector3(hole.xMin + (wIDTH / 2f), hole.height, 0f);
        this.mask4.center = new Vector3((wIDTH / 2f) - (((wIDTH / 2f) - hole.xMax) / 2f), hole.center.y, 0f);
        this.mask4.size = new Vector3((wIDTH / 2f) - hole.xMax, hole.height, 0f);
    }

    private void Start()
    {
        base.gameObject.transform.localScale = Vector3.one;
    }

    private void Update()
    {
    }
}

