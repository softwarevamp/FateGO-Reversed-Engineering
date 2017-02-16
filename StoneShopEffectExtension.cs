using System;
using System.Runtime.CompilerServices;

public static class StoneShopEffectExtension
{
    public static string GetDisplayName(this StoneShopEffect.Kind @this)
    {
        switch (@this)
        {
            case StoneShopEffect.Kind.EXTEND_FRIEND_MAX:
                return "ExtendFriendMax";

            case StoneShopEffect.Kind.EXTEND_SVT_MAX:
                return "ExtendServantMax";

            case StoneShopEffect.Kind.EXTEND_SVT_EQUIP_MAX:
                return "ExtendServantEquipMax";

            case StoneShopEffect.Kind.CONTINUE:
                return "BattleContinue";

            case StoneShopEffect.Kind.ACT_RECOVER:
                return "ApRecover";
        }
        return @this.ToString();
    }
}

