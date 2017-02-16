using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardUIDragDropListViewSurface")]
public class SampleCardUIDragDropListViewSurface : UIDragDropListViewSurface
{
    [SerializeField]
    protected SampleCardListViewDropObject dropObject;

    public SampleCardListViewDropObject DropObject =>
        this.dropObject;
}

