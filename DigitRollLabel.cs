using System;
using System.Collections;
using UnityEngine;

public class DigitRollLabel : MonoBehaviour
{
    public UILabel nextlabel;
    private string nexttext = string.Empty;
    public UILabel nowlabel;

    public void Awake()
    {
        this.nexttext = this.nextlabel.text;
    }

    public void changeSpeed(int speed)
    {
        IEnumerator enumerator = base.gameObject.GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = speed * 1.9f;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public void endChange(string text)
    {
        Animation component = base.gameObject.GetComponent<Animation>();
        this.nowlabel.text = this.nextlabel.text;
        this.nextlabel.text = text;
        component.Stop();
        component.Play();
    }

    public void startChange(string text)
    {
        Animation component = base.gameObject.GetComponent<Animation>();
        if (!component.isPlaying && !text.Equals(this.nexttext))
        {
            this.nowlabel.text = this.nextlabel.text;
            this.nextlabel.text = text;
            component.Stop();
            component.Play();
            this.nexttext = text;
        }
    }
}

