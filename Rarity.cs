using System;

public class Rarity
{
    private static readonly string[] baseFileList = new string[] { "listframes0_bg", "listframes1_bg", "listframes1_bg", "listframes2_bg", "listframes3_bg", "listframes3_bg" };
    private static readonly string[] battleIconfilelist = new string[] { "img_common_frame01", "icon_rarity_r", "icon_rarity_r", "icon_rarity_r", "icon_rarity_sr", "icon_rarity_ssr" };
    private static readonly int[] classIconTable = new int[] { 2, 1, 1, 2, 3, 3 };
    private static readonly string[] designCardFileList = new string[] { "class_b_", "class_b_", "class_b_", "class_s_", "class_g_", "class_g_" };
    private static readonly string[] formationBaseFileList = new string[] { "formation_blank_01", "formation_framebg_1", "formation_framebg_1", "formation_framebg_2", "formation_framebg_3", "formation_framebg_3", "formation_blank_01" };
    private static readonly string[] formationFrameFileList = new string[] { "formation_blank_01", "formation_frame_1", "formation_frame_1", "formation_frame_2", "formation_frame_3", "formation_frame_3", "formation_blank_01" };
    private static readonly string[] formationFrameForSupportSelectFileList = new string[] { "formation_blank_01", "formation_frame_support1", "formation_frame_support1", "formation_frame_support2", "formation_frame_support3", "formation_frame_support3", "formation_blank_01" };
    private static readonly int[] frameTypeImageTable = new int[] { 0, 1, 1, 2, 3, 3 };
    private static readonly int[] svtTypeImgTable = new int[] { 2, 1, 1, 2, 3, 3 };

    public static string getBattleFrameIcon(int rarityId) => 
        battleIconfilelist[rarityId];

    public static string getDesignCardPrefix(int rarityId) => 
        designCardFileList[rarityId];

    public static string getFaceBaseIcon(int rarityId) => 
        baseFileList[rarityId];

    public static string getFaceFrameIcon(int rarityId) => 
        "img_common_frame01";

    public static string getFaceFrameOnlyIcon(int rarityId) => 
        "img_common_frame01";

    public static string getFormationBase(int rarityId)
    {
        int index = rarityId;
        if (rarityId == -1)
        {
            index = formationBaseFileList.Length - 1;
        }
        return formationBaseFileList[index];
    }

    public static string getFormationFrame(int rarityId)
    {
        int index = rarityId;
        if (rarityId == -1)
        {
            index = formationFrameFileList.Length - 1;
        }
        return formationFrameFileList[index];
    }

    public static string getFormationFrameForSupportSelect(int rarityId)
    {
        int index = rarityId;
        if (rarityId == -1)
        {
            index = formationFrameForSupportSelectFileList.Length - 1;
        }
        return formationFrameForSupportSelectFileList[index];
    }

    public static int getFrameTypeImage(int rarityId) => 
        frameTypeImageTable[rarityId];

    public static string getIcon(int rarityId) => 
        ("icon_rarity_" + rarityId);

    public static int getLowerColorRarity(int rarityId)
    {
        switch (rarityId)
        {
            case 1:
                return 1;

            case 2:
                return 1;

            case 3:
                return 1;

            case 4:
                return 3;

            case 5:
                return 3;
        }
        return 1;
    }

    public static int getServantClassIcon(int rarityId) => 
        classIconTable[rarityId];

    public static int getServantTypeImg(int rarityId) => 
        svtTypeImgTable[rarityId];

    public static int getUpperColorRarity(int rarityId)
    {
        switch (rarityId)
        {
            case 1:
                return 3;

            case 2:
                return 3;

            case 3:
                return 4;

            case 4:
                return 5;

            case 5:
                return 5;
        }
        return 1;
    }

    public enum TYPE
    {
        ACCESSORY = 0x65,
        COMMON = 1,
        RARE = 3,
        SRARE = 4,
        SSRARE = 5,
        SUB_EQUIP = 0x66,
        UNCOMMON = 2
    }
}

