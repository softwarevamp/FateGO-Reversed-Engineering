using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestBoardListViewEarthLine : MonoBehaviour
{
    public const float BLINK_TIME = 0.5f;
    public const int LINE_RENDER_QUEUE = 0xc1c;
    public const float LINE_WIDTH = 0.01f;
    private EarthCore mEarthCore;
    private EarthPoint mEarthPoint;
    private CStateManager<QuestBoardListViewEarthLine> mFSM;
    private LineRenderer mLineRenderer;
    private Vector3 mLineStartPos;
    private QuestBoardListViewObject mListViewObject;
    private StandFigureSlideComponent mTerminalServant;
    public const float SCREEN_TOP_LIMIT_OFS = 80f;
    public const float TGT_SPD_TIME = 0.5f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<QuestBoardListViewEarthLine>(this, 2);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateMain());
            this.SetState(STATE.NONE);
        }
    }

    private Vector3 GetEndScreenPosition()
    {
        Vector3 position = this.mEarthPoint.gameObject.GetPosition();
        position.x /= base.transform.lossyScale.x;
        position.y /= base.transform.lossyScale.y;
        position.z = 0f;
        return position;
    }

    private Vector3 GetStartScreenPosition() => 
        (this.GetThisScreenPosition() + this.mLineStartPos);

    private STATE GetState() => 
        ((STATE) this.mFSM.getState());

    private Vector3 GetThisScreenPosition()
    {
        Vector3 position = base.gameObject.GetPosition();
        position.x /= base.transform.lossyScale.x;
        position.y /= base.transform.lossyScale.y;
        position.z = 0f;
        return position;
    }

    public void Hide()
    {
        if (this.GetState() != STATE.NONE)
        {
            this.SetState(STATE.NONE);
        }
    }

    private bool IsVisiblePossible()
    {
        if (!SingletonTemplate<TerminalDebugWindow>.Instance.TopWin.IsEnableSafe(5))
        {
            return false;
        }
        if (MainMenuBar.IsEnableOutSideCollider)
        {
            return false;
        }
        if (SingletonMonoBehaviour<CommonUI>.Instance.IsActive_UserPresentBoxWindow())
        {
            return false;
        }
        if (SingletonMonoBehaviour<CommonUI>.Instance.IsActive_ApRecvDlgComp())
        {
            return false;
        }
        if (SingletonTemplate<TerminalDebugWindow>.Instance.IsActive())
        {
            return false;
        }
        if (this.mEarthPoint == null)
        {
            return false;
        }
        if (!this.mEarthPoint.IsForward)
        {
            return false;
        }
        if (this.mTerminalServant == null)
        {
            return false;
        }
        if (this.mTerminalServant.IsLoding())
        {
            return false;
        }
        if (this.mTerminalServant.IsMoving())
        {
            return false;
        }
        if (this.mEarthCore == null)
        {
            return false;
        }
        if (this.mEarthCore.IsFocusMoving)
        {
            return false;
        }
        if (this.mTerminalServant.IsFrameIn())
        {
            return false;
        }
        if (!this.mEarthCore.IsFocusIn)
        {
            return false;
        }
        if (this.mListViewObject == null)
        {
            return false;
        }
        if (!this.mListViewObject.IsStateInput())
        {
            return false;
        }
        Vector3 startScreenPosition = this.GetStartScreenPosition();
        if (startScreenPosition.y > ((ManagerConfig.HEIGHT / 2) - 80f))
        {
            return false;
        }
        if (startScreenPosition.y < -(ManagerConfig.HEIGHT / 2))
        {
            return false;
        }
        Vector3 endScreenPosition = this.GetEndScreenPosition();
        if (endScreenPosition.x < -(ManagerConfig.WIDTH / 2))
        {
            return false;
        }
        if (startScreenPosition.x <= endScreenPosition.x)
        {
            return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
        if (this.IsVisiblePossible())
        {
            this.Visible();
        }
        else
        {
            this.Hide();
        }
    }

    private void OnDisable()
    {
        this.Hide();
    }

    private void SetState(STATE state)
    {
        if (this.mFSM != null)
        {
            this.mFSM.setState((int) state);
        }
    }

    public void SetupFirst(LineRenderer lr)
    {
        this.mLineRenderer = lr;
        this.mLineRenderer.useWorldSpace = false;
        this.mLineRenderer.SetWidth(0.01f, 0.01f);
        this.mLineRenderer.SetVertexCount(2);
        this.mLineRenderer.SetPosition(0, Vector3.zero);
        this.mLineRenderer.SetPosition(1, Vector3.zero);
        this.mLineRenderer.sharedMaterial.renderQueue = 0xc1c;
    }

    public void SetupSecond(Vector3 st_pos, QuestBoardListViewObject lvo, int war_id)
    {
        this.mLineStartPos = st_pos;
        this.mListViewObject = null;
        this.mEarthCore = TerminalSceneComponent.Instance.EarthCore;
        this.mTerminalServant = TerminalSceneComponent.Instance.TerminalServant;
        this.mEarthPoint = this.mEarthCore.GetEarthPoint(war_id);
        if (this.mEarthPoint != null)
        {
            this.mListViewObject = lvo;
        }
    }

    private void Visible()
    {
        if (this.GetState() != STATE.MAIN)
        {
            this.SetState(STATE.MAIN);
        }
    }

    public enum STATE
    {
        NONE,
        MAIN,
        SIZEOF
    }

    private class StateMain : IState<QuestBoardListViewEarthLine>
    {
        private float mStartTime;
        private float mTgtRate;

        public void begin(QuestBoardListViewEarthLine that)
        {
            <begin>c__AnonStoreyBC ybc = new <begin>c__AnonStoreyBC {
                <>f__this = this
            };
            that.mLineRenderer.SetPosition(0, that.mLineStartPos);
            this.mStartTime = Time.realtimeSinceStartup;
            ybc.eo = that.gameObject.SafeGetComponent<EasingObject>();
            ybc.eo.enabled = true;
            ybc.eo.Play(0.5f, new System.Action(ybc.<>m__1B6), new System.Action(ybc.<>m__1B7), 0f, Easing.TYPE.EXPONENTIAL_OUT);
        }

        public void end(QuestBoardListViewEarthLine that)
        {
        }

        public void update(QuestBoardListViewEarthLine that)
        {
            float num = Time.realtimeSinceStartup - this.mStartTime;
            that.mLineRenderer.enabled = (num >= 0.5f) || !that.mLineRenderer.enabled;
            Vector3 position = that.GetEndScreenPosition() - that.GetThisScreenPosition();
            if (this.mTgtRate < 1f)
            {
                Vector3 vector2 = position - that.mLineStartPos;
                float num2 = vector2.magnitude * this.mTgtRate;
                position = that.mLineStartPos + ((Vector3) (vector2.normalized * num2));
            }
            that.mLineRenderer.SetPosition(1, position);
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyBC
        {
            internal QuestBoardListViewEarthLine.StateMain <>f__this;
            internal EasingObject eo;

            internal void <>m__1B6()
            {
                this.<>f__this.mTgtRate = this.eo.Now();
            }

            internal void <>m__1B7()
            {
                this.<>f__this.mTgtRate = 1f;
            }
        }
    }

    private class StateNone : IState<QuestBoardListViewEarthLine>
    {
        public void begin(QuestBoardListViewEarthLine that)
        {
            if (that.mLineRenderer != null)
            {
                that.mLineRenderer.enabled = false;
                that.gameObject.SafeGetComponent<EasingObject>().enabled = false;
            }
        }

        public void end(QuestBoardListViewEarthLine that)
        {
        }

        public void update(QuestBoardListViewEarthLine that)
        {
        }
    }
}

