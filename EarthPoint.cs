using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EarthPoint : MonoBehaviour
{
    private Camera earthEffectCamera;
    protected bool IsParticleDisp = true;
    private Vector3 mBaseVec;
    private EarthCore mEarthCore;
    private bool mIsForward;
    private ParticleSystem mParticleSystem;
    private GameObject mParticleSystemObj;
    private UIWidget mWidget;
    public const string NAME_PREFIX = "EarthPoint_";

    public static float Cos2Deg(float _cos) => 
        (Mathf.Acos(_cos) * 57.29578f);

    public float GetDotXZ(ref Vector3 vec)
    {
        vec = base.gameObject.GetPosition() - this.mEarthCore.gameObject.GetPosition();
        vec.y = 0f;
        vec = vec.normalized;
        return Vector3.Dot(vec, Vector3.back);
    }

    public float GetDotXZ_Deg(ref Vector3 vec)
    {
        vec = base.gameObject.GetLocalPosition();
        vec.y = 0f;
        vec = vec.normalized;
        Vector3 rhs = Matrix4x4.TRS(Vector3.zero, this.mEarthCore.gameObject.transform.rotation, Vector3.one).MultiplyPoint3x4(Vector3.back);
        return Cos2Deg(Vector3.Dot(vec, rhs));
    }

    private void LateUpdate()
    {
        if (this.mWidget != null)
        {
            Vector3 zero = Vector3.zero;
            float dotXZ = this.GetDotXZ(ref zero);
            bool mIsForward = this.mIsForward;
            this.mIsForward = dotXZ > 0f;
            Color color = this.mWidget.color;
            color.a = !this.mIsForward ? 0f : dotXZ;
            if (this.mParticleSystem != null)
            {
                this.mParticleSystem.startColor = color;
            }
            this.mWidget.color = color;
            Vector3 vector2 = Vector3.zero;
            if (this.earthEffectCamera != null)
            {
                vector2 = this.earthEffectCamera.WorldToViewportPoint(this.mParticleSystem.transform.position);
                if ((vector2.x < 0f) && this.mIsForward)
                {
                    this.IsParticleDisp = false;
                }
            }
            if (this.mIsForward)
            {
                base.transform.eulerAngles = Vector3.zero;
            }
            if (!this.mIsForward && mIsForward)
            {
                if (this.mParticleSystem != null)
                {
                    this.mParticleSystem.Clear();
                }
                this.IsParticleDisp = false;
            }
            else if ((!this.IsParticleDisp && this.mIsForward) && ((vector2.x >= 0f) && (this.mParticleSystem != null)))
            {
                this.mParticleSystem.gameObject.SetActive(false);
                this.mParticleSystem.gameObject.SetActive(true);
                this.IsParticleDisp = true;
            }
        }
    }

    public void Setup(GameObject eff_obj, Camera earthEffCamera)
    {
        bool flag = eff_obj != null;
        base.gameObject.GetComponent<UISprite>().enabled = false;
        base.gameObject.SetActive(flag);
        GameObject gameObject = base.transform.parent.parent.gameObject;
        this.mWidget = base.gameObject.GetComponent<UIWidget>();
        this.mEarthCore = gameObject.GetComponent<EarthCore>();
        float x = 1f / gameObject.transform.localScale.x;
        base.transform.localScale = new Vector3(x, x, x);
        if (flag && (this.mParticleSystemObj == null))
        {
            GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(eff_obj);
            obj3.transform.parent = base.gameObject.transform;
            obj3.transform.localPosition = Vector3.zero;
            obj3.transform.localRotation = Quaternion.identity;
            obj3.transform.localScale = Vector3.one;
            this.mParticleSystem = obj3.GetComponentInChildren<ParticleSystem>();
            this.mParticleSystemObj = obj3;
        }
        Vector3 zero = Vector3.zero;
        float num2 = this.GetDotXZ_Deg(ref zero);
        if (zero.x < 0f)
        {
            num2 = -num2;
        }
        this.FocusAng = num2;
        if (flag)
        {
            num2 += 45f;
            this.mEarthCore.PointInitAngle = num2;
        }
        this.FocusQua = Quaternion.LookRotation(-base.gameObject.GetLocalPosition().normalized);
        this.FocusQua = Quaternion.Inverse(this.FocusQua);
        this.earthEffectCamera = earthEffCamera;
    }

    public EarthCore EarthCore =>
        this.mEarthCore;

    public float FocusAng { get; private set; }

    public Quaternion FocusQua { get; private set; }

    public bool IsForward =>
        this.mIsForward;
}

