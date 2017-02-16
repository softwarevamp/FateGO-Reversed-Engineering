using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommandSpellIconManager : SingletonMonoBehaviour<CommandSpellIconManager>
{
    [SerializeField]
    protected GameObject commandSpellPrefab;

    public CommandSpellIconComponent Create(UserGameEntity dat, int width = 80, int height = 80)
    {
        int spellImageId = dat.SpellImageId;
        int remain = dat.getCommandSpell();
        return this.Create(spellImageId, remain, width, height);
    }

    public CommandSpellIconComponent Create(int imageType, int remain, int width = 80, int height = 80)
    {
        CommandSpellIconComponent component = UnityEngine.Object.Instantiate<GameObject>(this.commandSpellPrefab).GetComponent<CommandSpellIconComponent>();
        component.SetImageType(imageType);
        component.SetRemain(remain);
        component.SetSize(new Vector2((float) width, (float) height));
        return component;
    }
}

