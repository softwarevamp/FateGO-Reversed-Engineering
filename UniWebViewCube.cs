using System;
using UnityEngine;

public class UniWebViewCube : MonoBehaviour
{
    private bool firstHit = true;
    private float startTime;
    public UniWebDemo webViewDemo;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Target")
        {
            this.webViewDemo.ShowAlertInWebview(Time.time - this.startTime, this.firstHit);
            this.firstHit = false;
        }
    }

    private void Start()
    {
        this.startTime = Time.time;
    }
}

