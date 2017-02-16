using System;

public class BannerEntity : DataEntityBase
{
    public int bannerId;
    public int bannerPriority;
    public int deviceType;
    public long finishedAt;
    public int id;
    public string linkBody;
    public int linkType;
    public long noticeAt;
    public int terminalBannerPriority;

    public Device.Type GetDeviceType() => 
        ((Device.Type) this.deviceType);

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool IsEnable()
    {
        if (this.bannerId <= 0)
        {
            return false;
        }
        if (this.terminalBannerPriority <= 0)
        {
            return false;
        }
        if (!this.IsEnableDevice())
        {
            return false;
        }
        long num = NetworkManager.getTime();
        if (num < this.noticeAt)
        {
            return false;
        }
        if (num >= this.finishedAt)
        {
            return false;
        }
        return true;
    }

    public bool IsEnableDevice()
    {
        switch (this.GetDeviceType())
        {
            case Device.Type.ALL:
                return true;

            case Device.Type.OTHER:
                return false;

            case Device.Type.ANDROID:
                return true;
        }
        return false;
    }
}

