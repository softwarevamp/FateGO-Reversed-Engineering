using System;
using UnityEngine;

public class TitleEffect : MonoBehaviour
{
    public ParticleSystem titlePsLeft;
    public ParticleSystem titlePsRight;

    private void Start()
    {
        this.titlePsLeft.Play();
        this.titlePsRight.Play();
    }

    private void Update()
    {
    }
}

