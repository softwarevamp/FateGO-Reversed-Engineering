using System;
using UnityEngine;

public class ForceDisableObject : MonoBehaviour
{
    public Transform[] disableObjects;

    public void DisableAllObjects()
    {
        if (this.disableObjects != null)
        {
            foreach (Transform transform in this.disableObjects)
            {
                if (transform != null)
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }
    }

    private void Start()
    {
        this.DisableAllObjects();
    }

    private void Update()
    {
    }
}

