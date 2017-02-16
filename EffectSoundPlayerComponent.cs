using System;

public class EffectSoundPlayerComponent : BaseMonoBehaviour
{
    protected void PlaySe(string name)
    {
        char[] separator = new char[] { ':' };
        string[] strArray = name.Split(separator);
        if (strArray.Length >= 2)
        {
            SoundManager.playSe(strArray[0], strArray[1]);
        }
        else
        {
            SoundManager.playSe(strArray[0]);
        }
    }
}

