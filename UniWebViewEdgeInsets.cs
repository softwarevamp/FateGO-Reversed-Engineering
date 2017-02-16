using System;

[Serializable]
public class UniWebViewEdgeInsets
{
    public int bottom;
    public int left;
    public int right;
    public int top;

    public UniWebViewEdgeInsets(int aTop, int aLeft, int aBottom, int aRight)
    {
        this.top = aTop;
        this.left = aLeft;
        this.bottom = aBottom;
        this.right = aRight;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || (base.GetType() != obj.GetType()))
        {
            return false;
        }
        UniWebViewEdgeInsets insets = (UniWebViewEdgeInsets) obj;
        return ((((this.top == insets.top) && (this.left == insets.left)) && (this.bottom == insets.bottom)) && (this.right == insets.right));
    }

    public override int GetHashCode()
    {
        int num = ((this.top + this.left) + this.bottom) + this.right;
        return num.GetHashCode();
    }

    public static bool operator ==(UniWebViewEdgeInsets inset1, UniWebViewEdgeInsets inset2) => 
        inset1.Equals(inset2);

    public static bool operator !=(UniWebViewEdgeInsets inset1, UniWebViewEdgeInsets inset2) => 
        !inset1.Equals(inset2);
}

