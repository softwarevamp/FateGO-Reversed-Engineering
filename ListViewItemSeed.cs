using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewItemSeed")]
public class ListViewItemSeed : ItemSeed
{
    public Arrangement arrangement;
    public Vector2 arrangementPich = new Vector2(1f, 1f);
    public Vector2 arrangementVolume = new Vector2(1f, 1f);

    public Vector3 GetBlank(GameObject obj)
    {
        BoxCollider component = obj.GetComponent<BoxCollider>();
        if (this.arrangement == Arrangement.Horizontal)
        {
            return new Vector3(this.arrangementPich.x - component.size.x, 0f, 0f);
        }
        return new Vector3(0f, this.arrangementPich.y - component.size.y, 10f);
    }

    public Vector3 GetLocalPosition(int index) => 
        base.parent.transform.InverseTransformPoint(this.GetPosition(index));

    public Vector3 GetPosition(int index)
    {
        float num;
        float num2;
        if (this.arrangement == Arrangement.Horizontal)
        {
            int y = (int) this.arrangementVolume.y;
            if (y < 1)
            {
                y = 1;
            }
            num2 = (index % y) - (((float) (y - 1)) / 2f);
            num = index / y;
        }
        else
        {
            int x = (int) this.arrangementVolume.x;
            if (x < 1)
            {
                x = 1;
            }
            num = (index % x) - (((float) (x - 1)) / 2f);
            num2 = index / x;
        }
        return base.transform.TransformPoint(this.arrangementPich.x * num, this.arrangementPich.y * num2, 0f);
    }

    public void SetTransform(GameObject obj, int index)
    {
        obj.transform.localPosition = this.GetLocalPosition(index);
        obj.transform.localRotation = base.transform.localRotation;
        obj.transform.localScale = base.transform.localScale;
        obj.SendMessage("SetBaseTransform");
    }

    public enum Arrangement
    {
        Horizontal,
        Vertical
    }
}

