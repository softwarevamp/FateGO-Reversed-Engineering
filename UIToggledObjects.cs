using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
    public List<GameObject> activate;
    public List<GameObject> deactivate;
    [HideInInspector, SerializeField]
    private bool inverse;
    [HideInInspector, SerializeField]
    private GameObject target;

    private void Awake()
    {
        if (this.target != null)
        {
            if ((this.activate.Count == 0) && (this.deactivate.Count == 0))
            {
                if (this.inverse)
                {
                    this.deactivate.Add(this.target);
                }
                else
                {
                    this.activate.Add(this.target);
                }
            }
            else
            {
                this.target = null;
            }
        }
        EventDelegate.Add(base.GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.Toggle));
    }

    private void Set(GameObject go, bool state)
    {
        if (go != null)
        {
            NGUITools.SetActive(go, state);
        }
    }

    public void Toggle()
    {
        bool state = UIToggle.current.value;
        if (base.enabled)
        {
            for (int i = 0; i < this.activate.Count; i++)
            {
                this.Set(this.activate[i], state);
            }
            for (int j = 0; j < this.deactivate.Count; j++)
            {
                this.Set(this.deactivate[j], !state);
            }
        }
    }
}

