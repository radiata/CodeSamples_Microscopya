using UnityEngine;

[CreateAssetMenu(fileName = "Character Color Options", menuName = "Custom Menus/Character Settings/Character Settings Scriptable Objects/Character Color Options")]
public class CharacterColorOptions_SO : ScriptableObject
{
    public delegate void CharacterColorChanged();
    public static event CharacterColorChanged OnCharacterColorChanged;

    private const string CharacterColor_HairColor = "CharacterColor.HairColor";
    [SerializeField] private Color defaultHairColor = new Color(41f / 255f, 105f / 255f, 191f / 255f);
    private Color hairColor;

    private const string CharacterColor_SkinColor = "CharacterColor.SkinColor";
    [SerializeField] private Color defaultSkinColor = new Color(178f / 255f, 215f / 255f, 1f);
    private Color skinColor;

    private const string CharacterColor_CoatColor = "CharacterColor.CoatColor";
    [SerializeField] private Color defaultCoatColor = new Color(1f, 1f, 1f);
    private Color coatColor;

    private const string CharacterColor_DressColor = "CharacterColor.DressColor";
    [SerializeField] private Color defaultDressColor = new Color(173f / 255f, 68f / 255f, 156f / 255f);
    private Color dressColor;

    private const string CharacterColor_ShoeColor = "CharacterColor.ShoeColor";
    [SerializeField] private Color defaultShoeColor = new Color(115f / 255f, 89f / 255f, 41f / 255f);
    private Color shoeColor;

    public Color HairColor => hairColor;
    public Color SkinColor => skinColor;
    public Color CoatColor => coatColor;
    public Color DressColor => dressColor;
    public Color ShoeColor => shoeColor;

    #region Set Colors
    public void SetHairColor(Color? newHairColor)
    {
        if (newHairColor == null)
        {
            hairColor = defaultHairColor;
        }
        else
        {
            hairColor = newHairColor.Value;
        }

        OnCharacterColorChanged?.Invoke();
    }

    public void SetSkinColor(Color? newSkinColor)
    {
        if (newSkinColor == null)
        {
            skinColor = defaultSkinColor;
        }
        else
        {
            skinColor = newSkinColor.Value;
        }

        OnCharacterColorChanged?.Invoke();
    }

    public void SetCoatColor(Color? newCoatColor)
    {
        if (newCoatColor == null)
        {
            coatColor = defaultCoatColor;
        }
        else
        {
            coatColor = newCoatColor.Value;
        }

        OnCharacterColorChanged?.Invoke();
    }

    public void SetDressColor(Color? newDressColor)
    {
        if (newDressColor == null)
        {
            dressColor = defaultDressColor;
        }
        else
        {
            dressColor = newDressColor.Value;
        }

        OnCharacterColorChanged?.Invoke();
    }

    public void SetShoeColor(Color? newShoeColor)
    {
        if (newShoeColor == null)
        {
            shoeColor = defaultShoeColor;
        }
        else
        {
            shoeColor = newShoeColor.Value;
        }

        OnCharacterColorChanged?.Invoke();
    }

    public void SetToDefaultColors()
    {
        hairColor = defaultHairColor;
        skinColor = defaultSkinColor;
        coatColor = defaultCoatColor;
        dressColor = defaultDressColor;
        shoeColor = defaultShoeColor;
        OnCharacterColorChanged?.Invoke();
    }
    #endregion

    public void SaveColors()
    {
        SetPlayerKeys();
    }

    public bool UnsavedChanges()
    {
        if(hairColor != PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_HairColor, defaultHairColor)
            || skinColor != PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_SkinColor, defaultSkinColor)
            || coatColor != PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_CoatColor, defaultCoatColor)
            || dressColor != PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_DressColor, defaultDressColor)
            || shoeColor != PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_ShoeColor, defaultShoeColor))
        {
            return true;
        }

        return false;
    }

    public void LoadColors(bool silent = true)
    {
        hairColor = PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_HairColor, defaultHairColor);
        skinColor = PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_SkinColor, defaultSkinColor);
        coatColor = PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_CoatColor, defaultCoatColor);
        dressColor = PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_DressColor, defaultDressColor);
        shoeColor = PlayerPrefs_Utilities.GetColorFromPreferencesKey(CharacterColor_ShoeColor, defaultShoeColor);

        if(silent == false)
        {
            OnCharacterColorChanged?.Invoke();
        }
    }

    private void SetPlayerKeys()
    {
        PlayerPrefs_Utilities.SetColorToPreferencesKey(CharacterColor_HairColor, hairColor);
        PlayerPrefs_Utilities.SetColorToPreferencesKey(CharacterColor_SkinColor, skinColor);
        PlayerPrefs_Utilities.SetColorToPreferencesKey(CharacterColor_CoatColor, coatColor);
        PlayerPrefs_Utilities.SetColorToPreferencesKey(CharacterColor_DressColor, dressColor);
        PlayerPrefs_Utilities.SetColorToPreferencesKey(CharacterColor_ShoeColor, shoeColor);
    }
}
