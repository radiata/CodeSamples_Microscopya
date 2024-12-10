
public class CharacterHairOptions
{
    private CharacterHairOptions_SO characterHairOptions_SO;

    public CharacterHairOptions(CharacterHairOptions_SO characterHairOptions_SO)
    {
        this.characterHairOptions_SO = characterHairOptions_SO;
        characterHairOptions_SO.LoadHairOptions();
    }

    public void ApplyHairOptions(HairSprites hairSprites)
    {
        hairSprites.HairBack.sprite = characterHairOptions_SO.HairBack;
        hairSprites.HairFront_01.sprite = characterHairOptions_SO.HairFront_01;
        hairSprites.HairFront_02.sprite = characterHairOptions_SO.HairFront_02;
        hairSprites.HairMiddle.sprite = characterHairOptions_SO.HairMiddle;
        hairSprites.EyeBrow.sprite = characterHairOptions_SO.EyeBrow;
    }
}
