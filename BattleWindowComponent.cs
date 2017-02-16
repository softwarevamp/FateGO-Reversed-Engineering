using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleWindowComponent : BaseMonoBehaviour
{
    protected EndCall call_closeComp;
    protected EndCall call_openComp;
    private float movetime;
    private WINDOWSTATE state;
    private Vector3 tmp_pos;
    private ACTIONTYPE type = ACTIONTYPE.POPUP;

    public virtual void Close(EndCall call = null)
    {
        this.call_closeComp = call;
        if (!base.gameObject.activeSelf)
        {
            if (this.call_closeComp != null)
            {
                this.call_closeComp();
            }
        }
        else
        {
            iTween component = base.gameObject.GetComponent<iTween>();
            if (component != null)
            {
                iTween.Stop(base.gameObject);
                UnityEngine.Object.DestroyImmediate(component);
            }
            Hashtable args = new Hashtable {
                { 
                    "isLocal",
                    true
                },
                { 
                    "oncompletetarget",
                    base.gameObject
                },
                { 
                    "oncomplete",
                    "CompClose"
                },
                { 
                    "time",
                    this.movetime
                }
            };
            if (this.type == ACTIONTYPE.SLIDE)
            {
                args.Add("position", this.tmp_pos - new Vector3(-80f, 0f));
                args.Add("easetype", iTween.EaseType.easeInBack);
                iTween.MoveTo(base.gameObject, args);
            }
            else if (this.type == ACTIONTYPE.POPUP)
            {
                args.Add("scale", new Vector3(0.7f, 0.7f));
                args.Add("easetype", iTween.EaseType.easeInBack);
                iTween.ScaleTo(base.gameObject, args);
            }
            else if (this.type == ACTIONTYPE.ALPHA)
            {
                args.Add("scale", new Vector3(0.9f, 0.9f));
                args.Add("easetype", iTween.EaseType.easeInBack);
                iTween.ScaleTo(base.gameObject, args);
                UIWidget widget = base.gameObject.GetComponent<UIWidget>();
                if (widget != null)
                {
                    widget.alpha = 1f;
                    TweenAlpha.Begin(base.gameObject, this.movetime, 0f).method = UITweener.Method.EaseOutQuad;
                }
            }
            this.state = WINDOWSTATE.ING_CLOSE;
        }
    }

    public virtual void CompClose()
    {
        this.state = WINDOWSTATE.CLOSE;
        base.gameObject.SetActive(false);
        if (this.call_closeComp != null)
        {
            this.call_closeComp();
        }
    }

    public virtual void CompOpen()
    {
        this.state = WINDOWSTATE.OPEN;
        if (this.call_openComp != null)
        {
            this.call_openComp();
        }
    }

    public bool isClose() => 
        (this.state == WINDOWSTATE.CLOSE);

    public bool isOpen() => 
        (this.state == WINDOWSTATE.OPEN);

    public virtual void Open(EndCall call = null)
    {
        this.call_openComp = call;
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            iTween.Stop(base.gameObject);
            UnityEngine.Object.DestroyImmediate(component);
        }
        base.gameObject.SetActive(true);
        base.gameObject.transform.localPosition = this.tmp_pos;
        Hashtable args = new Hashtable {
            { 
                "isLocal",
                true
            },
            { 
                "oncompletetarget",
                base.gameObject
            },
            { 
                "oncomplete",
                "CompOpen"
            },
            { 
                "time",
                this.movetime
            }
        };
        if (this.type == ACTIONTYPE.SLIDE)
        {
            args.Add("position", this.tmp_pos - new Vector3(-80f, 0f));
            args.Add("easetype", iTween.EaseType.easeOutBack);
            iTween.MoveFrom(base.gameObject, args);
        }
        else if (this.type == ACTIONTYPE.POPUP)
        {
            base.transform.localScale = Vector3.one;
            args.Add("scale", new Vector3(0.7f, 0.7f));
            args.Add("easetype", iTween.EaseType.easeOutBack);
            iTween.ScaleFrom(base.gameObject, args);
        }
        else if (this.type == ACTIONTYPE.ALPHA)
        {
            base.transform.localScale = Vector3.one;
            args.Add("scale", new Vector3(0.9f, 0.9f));
            args.Add("easetype", iTween.EaseType.easeOutBack);
            iTween.ScaleFrom(base.gameObject, args);
            UIWidget widget = base.gameObject.GetComponent<UIWidget>();
            if (widget != null)
            {
                widget.alpha = 0f;
                TweenAlpha.Begin(base.gameObject, this.movetime, 1f).method = UITweener.Method.EaseOutQuad;
            }
        }
        this.state = WINDOWSTATE.ING_OPEN;
    }

    public virtual void setClose()
    {
        this.state = WINDOWSTATE.CLOSE;
        base.gameObject.SetActive(false);
    }

    public void setInitData(ACTIONTYPE type, float time = 0.3f, bool ocflg = false)
    {
        this.setInitialPos();
        this.call_openComp = null;
        this.call_closeComp = null;
        this.type = type;
        this.movetime = time;
        if (!ocflg)
        {
            base.gameObject.SetActive(false);
        }
    }

    public virtual void setInitialPos()
    {
        this.tmp_pos = base.gameObject.transform.localPosition;
    }

    public enum ACTIONTYPE
    {
        SLIDE,
        POPUP,
        ALPHA
    }

    public delegate void EndCall();

    private enum WINDOWSTATE
    {
        INIT,
        ING_CLOSE,
        CLOSE,
        OPEN,
        ING_OPEN
    }
}

