using System;

public class ConstantStrMaster : DataMasterBase
{
    public ConstantStrMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CONSTANT_STR);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ConstantStrEntity[1]);
        }
    }

    public string[][] GetCombineReturnVoiceList()
    {
        ConstantStrEntity entity = base.getEntityFromKey<ConstantStrEntity>("COMBINE_SCENE_VOICE_RETURN");
        if (entity == null)
        {
            return null;
        }
        char[] separator = new char[] { '/' };
        string[] strArray = entity.value.Split(separator);
        string[][] strArray2 = new string[strArray.Length][];
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] chArray2 = new char[] { ',' };
            strArray2[i] = strArray[i].Split(chArray2);
        }
        return strArray2;
    }

    public string[][] GetCombineWelcomeVoiceList()
    {
        ConstantStrEntity entity = base.getEntityFromKey<ConstantStrEntity>("COMBINE_SCENE_VOICE_WELCOME");
        if (entity == null)
        {
            return null;
        }
        char[] separator = new char[] { '/' };
        string[] strArray = entity.value.Split(separator);
        string[][] strArray2 = new string[strArray.Length][];
        for (int i = 0; i < strArray.Length; i++)
        {
            char[] chArray2 = new char[] { ',' };
            strArray2[i] = strArray[i].Split(chArray2);
        }
        return strArray2;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ConstantStrEntity>(obj);

    public static string getValue(string name)
    {
        ConstantStrEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ConstantStrMaster>(DataNameKind.Kind.CONSTANT_STR).getEntityFromKey<ConstantStrEntity>(name);
        if (entity != null)
        {
            return entity.value;
        }
        return null;
    }

    public string GetValue(string name)
    {
        ConstantStrEntity entity = base.getEntityFromKey<ConstantStrEntity>(name);
        if (entity != null)
        {
            return entity.value;
        }
        return null;
    }
}

