using UnityEngine;

public class PartyOrganizationUIDragDropListViewSurface : UIDragDropListViewSurface
{
    [SerializeField]
    protected PartyOrganizationListViewDropObject dropObject;

    public PartyOrganizationListViewDropObject DropObject =>
        this.dropObject;
}

