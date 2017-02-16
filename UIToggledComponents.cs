using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(UIToggle)), AddComponentMenu("NGUI/Interaction/Toggled Components")]
public class UIToggledComponents : MonoBehaviour
{
    public List<MonoBehaviour> activate;
    public List<MonoBehaviour> deactivate;
    [SerializeField, HideInInspector]
    private bool inverse;
    [HideInInspector, SerializeField]
    private MonoBehaviour target;

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

    public void Toggle()
    {
        if (base.enabled)
        {
            for (int i = 0; i < this.activate.Count; i++)
            {
                MonoBehaviour behaviour = this.activate[i];
                behaviour.enabled = UIToggle.current.value;
            }
            for (int j = 0; j < this.deactivate.Count; j++)
            {
                MonoBehaviour behaviour2 = this.deactivate[j];
                behaviour2.enabled = !UIToggle.current.value;
            }
        }
    }
}

