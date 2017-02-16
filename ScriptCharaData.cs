using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScriptCharaData
{
    protected string changeKind;
    protected float changeSpeed;
    protected UIScriptChara chara;
    protected int defaultDepth;
    protected Face.Type faceType;
    protected GameObject gameObject;
    protected string imageName;
    protected Kind kind;
    protected string name;
    protected State state;
    protected string talkName;

    public ScriptCharaData(Kind kind, string name, string imageName, ItemSeed seed)
    {
        this.kind = kind;
        this.name = name;
        this.imageName = string.Empty;
        this.gameObject = UnityEngine.Object.Instantiate<GameObject>(seed.Prefab);
        this.chara = this.gameObject.GetComponent<UIScriptChara>();
        switch (kind)
        {
            case Kind.FIGURE:
                this.gameObject.name = "Figure" + name;
                this.faceType = Face.Type.NORMAL;
                break;

            case Kind.EQUIP:
                this.gameObject.name = "Equip" + name;
                break;

            case Kind.IMAGE:
                this.gameObject.name = "Image" + name;
                break;
        }
        Transform transform = this.gameObject.transform;
        transform.parent = seed.Parent.transform;
        transform.position = seed.GetPosition();
        transform.localRotation = seed.transform.localRotation;
        transform.localScale = seed.transform.localScale;
        this.gameObject.layer = seed.Parent.layer;
        this.chara.SetBasePosition(ScriptPosition.GetPosition(0f, 0f));
        this.SetCharacter(imageName);
        this.defaultDepth = 0;
        this.SetDepth(this.defaultDepth);
    }

    public void ChangeCharacter(string kind, float speed, string imageName, int type)
    {
        this.imageName = imageName;
        this.faceType = (Face.Type) type;
        this.changeKind = kind;
        this.changeSpeed = speed;
        this.state = State.MOVE;
        this.chara.ChangeCharacter(this.changeKind, this.changeSpeed, this.imageName, this.faceType, new System.Action(this.EndChange));
    }

    public void Destroy()
    {
        this.state = State.DESTROY;
        if (this.gameObject != null)
        {
            UnityEngine.Object.Destroy(this.gameObject);
            this.chara = null;
            this.gameObject = null;
        }
    }

    protected void EndChange()
    {
        this.state = State.IDLE;
    }

    protected void EndLoadAsset()
    {
        this.state = State.IDLE;
    }

    public bool IsBackEffect() => 
        this.chara.IsBackEffect();

    public bool IsBackEffect(string n) => 
        this.chara.IsBackEffect(n);

    public bool IsChange() => 
        ((this.state == State.LOAD) || this.chara.IsChange());

    public bool IsCut() => 
        this.chara.IsCut();

    public bool IsEffect() => 
        this.chara.IsEffect();

    public bool IsEffect(string n) => 
        this.chara.IsEffect(n);

    public bool IsLoad() => 
        (this.state == State.LOAD);

    public bool IsMove() => 
        this.chara.IsMove();

    public bool IsMoveAlpha() => 
        this.chara.IsBusyMoveAlpha();

    public bool IsShake() => 
        this.chara.IsShake();

    public bool IsSpecialEffect() => 
        this.chara.IsSpecialEffect();

    public bool IsSpecialEffect(string n) => 
        this.chara.IsSpecialEffect(n);

    public void MoveAlpha(float duration, float alpha)
    {
        this.chara.MoveAlpha(duration, alpha);
    }

    public void MoveAttack(string kind, float duration, int index)
    {
        this.MoveAttack(kind, duration, ScriptPosition.GetPosition(index));
    }

    public void MoveAttack(string kind, float duration, Vector3 v)
    {
        this.chara.MoveAttack(kind, duration, v);
    }

    public void MoveAttack(string kind, float duration, float x, float y)
    {
        this.MoveAttack(kind, duration, ScriptPosition.GetPosition(x, y));
    }

    public void MovePosition(float duration, int index)
    {
        this.MovePosition(duration, ScriptPosition.GetPosition(index));
    }

    public void MovePosition(float duration, Vector3 v)
    {
        this.chara.MovePosition(duration, v);
    }

    public void MovePosition(float duration, float x, float y)
    {
        this.MovePosition(duration, ScriptPosition.GetPosition(x, y));
    }

    public void MoveReturnPosition(float duration)
    {
        this.chara.MoveReturnPosition(duration);
    }

    public void MoveReturnPosition(float duration, int index)
    {
        this.MoveReturnPosition(duration, ScriptPosition.GetPosition(index));
    }

    public void MoveReturnPosition(float duration, Vector3 v)
    {
        this.chara.MoveReturnPosition(duration, v);
    }

    public void MoveReturnPosition(float duration, float x, float y)
    {
        this.MoveReturnPosition(duration, ScriptPosition.GetPosition(x, y));
    }

    public void RecoverDepth()
    {
        this.chara.SetDepth(this.defaultDepth);
    }

    public void SetAlpha(float a)
    {
        this.chara.SetAlpha(a);
    }

    public void SetBackEffect(string n, bool isSkip)
    {
        this.chara.SetBackEffect(n, isSkip);
    }

    public void SetBackEffect(string n, Vector3 p, bool isSkip)
    {
        this.chara.SetBackEffect(n, p, isSkip);
    }

    public void SetCharacter(string imageName)
    {
        this.imageName = imageName;
        this.state = State.LOAD;
        this.chara.SetCharacter(imageName, this.faceType, new System.Action(this.EndLoadAsset));
    }

    public void SetCutin(string n, float time, float mgd, bool isSkip)
    {
        this.chara.SetCutin(n, time, mgd, isSkip);
    }

    public void SetCutout(float time, bool isSkip)
    {
        this.chara.SetCutout(time, isSkip);
    }

    public void SetDepth(int d)
    {
        this.defaultDepth = d;
        this.RecoverDepth();
    }

    public void SetEffect(string n, bool isSkip)
    {
        this.chara.SetEffect(n, isSkip);
    }

    public void SetEffect(string n, Vector3 p, bool isSkip)
    {
        this.chara.SetEffect(n, p, isSkip);
    }

    public void SetFace(int type)
    {
        this.faceType = (Face.Type) type;
        this.chara.SetFace(this.faceType);
    }

    public void SetFilter(string filterName, Color filterColor)
    {
        this.chara.SetFilter(filterName, filterColor);
    }

    public void SetPosition(int index)
    {
        this.chara.SetBasePosition(ScriptPosition.GetPosition(index));
    }

    public void SetPosition(Vector3 v)
    {
        this.chara.SetBasePosition(v);
    }

    public void SetPosition(float x, float y)
    {
        this.chara.SetBasePosition(ScriptPosition.GetPosition(x, y));
    }

    public void SetScale(float v)
    {
        this.chara.SetScale(v);
    }

    public void SetShadow(bool isShadow)
    {
        this.chara.SetShadow(isShadow);
    }

    public void SetSpecialEffect(string n, Vector3 pos, float time, Color color, bool isSkip)
    {
        this.chara.SetSpecialEffect(n, pos, time, color, isSkip);
    }

    public void SetTalkDepth()
    {
        this.chara.SetDepth(9);
    }

    public void SetTalkMask(bool isMask)
    {
        this.chara.SetTalkMask(isMask);
    }

    public void SetTalkName(string name)
    {
        this.talkName = name;
    }

    public void Shake(float duration, float cycle, float x, float y)
    {
        this.chara.Shake(duration, cycle, x, y);
    }

    public void ShakeStop()
    {
        this.chara.Shake(0f, 0f, 0f, 0f);
    }

    public void StopBackEffect(bool isSkip = false)
    {
        this.chara.StopBackEffect(isSkip);
    }

    public void StopBackEffect(string n, bool isSkip = false)
    {
        this.chara.StopBackEffect(n, isSkip);
    }

    public void StopCut()
    {
        this.chara.StopSpecialEffect();
    }

    public void StopEffect(bool isSkip = false)
    {
        this.chara.StopEffect(isSkip);
    }

    public void StopEffect(string n, bool isSkip = false)
    {
        this.chara.StopEffect(n, isSkip);
    }

    public void StopSpecialEffect()
    {
        this.chara.StopSpecialEffect();
    }

    public void StopSpecialEffect(string n)
    {
        this.chara.StopSpecialEffect(n);
    }

    public Kind DispKind =>
        this.kind;

    public string Name =>
        this.name;

    public string TalkName =>
        this.talkName;

    public enum Kind
    {
        FIGURE,
        EQUIP,
        IMAGE
    }

    public enum State
    {
        LOAD,
        IDLE,
        MOVE,
        DESTROY
    }
}

