using System.Collections.Generic;
using UnityEngine;

public class CharacterColorOptions
{
    private CharacterColorOptions_SO characterColorOptions_SO;

    public CharacterColorOptions(CharacterColorOptions_SO characterColorOptions_SO)
    {
        this.characterColorOptions_SO = characterColorOptions_SO;
        characterColorOptions_SO.LoadColors();
    }

    public void ApplyColorOptions(List<SpriteRenderer> hairSprites, List<SpriteRenderer> skinSprites, List<SpriteRenderer> coatSprites, List<SpriteRenderer> dressSprites, List<SpriteRenderer> shoeSprites)
    {
        ApplyColorToSpriteRenderers(characterColorOptions_SO.HairColor, hairSprites);
        ApplyColorToSpriteRenderers(characterColorOptions_SO.SkinColor, skinSprites);
        ApplyColorToSpriteRenderers(characterColorOptions_SO.CoatColor, coatSprites);
        ApplyColorToSpriteRenderers(characterColorOptions_SO.DressColor, dressSprites);
        ApplyColorToSpriteRenderers(characterColorOptions_SO.ShoeColor, shoeSprites);
    }

    private void ApplyColorToSpriteRenderers(Color color, List<SpriteRenderer> spriteRenderers)
    {
        if(spriteRenderers == null)
        {
            return;
        }

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = color;
        }
    }
}
