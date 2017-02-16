using System;
using System.Runtime.InteropServices;

public class SupportInfoJump
{
    protected FollowerInfo followerInfo;
    protected bool isSelect;
    protected FriendStatus.Kind kind;
    protected OtherUserGameEntity otherUserGameEntity;
    protected string returnSceneName;
    public int selectClassId;

    public SupportInfoJump(FollowerInfo followerInfo, FriendStatus.Kind kind, bool isSelect = false)
    {
        this.followerInfo = followerInfo;
        this.kind = kind;
        this.isSelect = isSelect;
    }

    public SupportInfoJump(OtherUserGameEntity entity, FriendStatus.Kind kind, bool isSelect = false)
    {
        this.otherUserGameEntity = entity;
        this.kind = kind;
        this.isSelect = isSelect;
    }

    public FollowerInfo GetFollowerInfo() => 
        this.followerInfo;

    public OtherUserGameEntity GetFriendInfo() => 
        this.otherUserGameEntity;

    public bool IsEnableReturnScene() => 
        (this.returnSceneName != null);

    public bool ReturnScene(SceneManager.FadeType fade = 1, object data = null)
    {
        if (this.returnSceneName != null)
        {
            SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(this.returnSceneName, fade, data);
            return true;
        }
        return false;
    }

    public void SetReturnNowScene()
    {
        this.returnSceneName = SingletonMonoBehaviour<SceneManager>.Instance.GetNowSceneName();
    }

    public void SetReturnScene(SceneList.Type type)
    {
        this.returnSceneName = SceneList.getSceneName(type);
    }

    public void SetSelectClassId(int classId)
    {
        this.selectClassId = classId;
    }

    public bool IsSelect =>
        this.isSelect;

    public FriendStatus.Kind Kind =>
        this.kind;

    public int SelectClassId =>
        this.selectClassId;
}

