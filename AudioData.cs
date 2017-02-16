using System;
using UnityEngine;

[Serializable]
public class AudioData
{
    [SerializeField]
    protected AudioClip data;
    [SerializeField]
    protected string name;

    public AudioData(AudioClip clip)
    {
        this.name = clip.name;
        this.data = clip;
    }

    public AudioData(string name, AudioClip clip)
    {
        this.name = name;
        this.data = clip;
    }

    public AudioData(string name, float[] buf)
    {
        this.name = name;
        this.data = AudioClip.Create(name, buf.Length, 1, 0xac44, false, false);
        this.data.SetData(buf, 0);
    }

    public bool IsSame(string name) => 
        this.name.Equals(name);

    public AudioClip Data =>
        this.data;

    public string Name =>
        this.name;
}

