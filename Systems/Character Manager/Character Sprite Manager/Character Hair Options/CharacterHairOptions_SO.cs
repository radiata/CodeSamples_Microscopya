using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Hair Options", menuName = "Custom Menus/Character Settings/Character Settings Scriptable Objects/Character Hair Options")]
public class CharacterHairOptions_SO : ScriptableObject
{
    public delegate void CharacterHairChanged();
    public static event CharacterHairChanged OnCharacterHairChanged;

    private HairStyle_SO currentHairStyle;

    [SerializeField] private HairSelection defaultHairSelection;
    private HairSelection hairSelection;

    [SerializeField] private List<HairStyle_SO> hairStyles;

    public HairStyle_SO CurrentHairStyle => currentHairStyle;

    //eventually we probably want to return these based on a path and use resources load
    public Sprite HairBack => currentHairStyle.HairBack;
    public Sprite HairFront_01 => currentHairStyle.HairFront_01;
    public Sprite HairFront_02 => currentHairStyle.HairFront_02;
    public Sprite HairMiddle => currentHairStyle.HairMiddle;
    public Sprite EyeBrow => currentHairStyle.EyeBrow;

    public void SetHair(HairSelection? hairSelection)
    {
        if (hairSelection == null)
        {
            this.hairSelection = defaultHairSelection;
        }
        else
        {
            this.hairSelection = hairSelection.Value;
        }

        UpdateCurrentHair();
        OnCharacterHairChanged?.Invoke();
    }

    public void SetToDefaultHair()
    {
        SetHair(defaultHairSelection);
    }

    public void UpdateCurrentHair()
    {
        currentHairStyle = GetFirstMatchingHairStyleOrNull(hairSelection);
    }

    public bool UnsavedChanges()
    {
        if (hairSelection != PlayerPrefs_Utilities.GetHairCustomization())
        {
            return true;
        }

        return false;
    }

    public void SaveHairOptions()
    {
        SetPlayerKeys();
    }

    public void LoadHairOptions(bool silent = true)
    {
        hairSelection = PlayerPrefs_Utilities.GetHairCustomization();
        UpdateCurrentHair();

        if(silent == false)
        {
            OnCharacterHairChanged?.Invoke();
        }
    }

    private void SetPlayerKeys()
    {
        PlayerPrefs_Utilities.SetHairCustomization(hairSelection);
    }

    public HairStyle_SO GetFirstMatchingHairStyleOrNull(HairSelection hairSelection)
    {
        HairStyle_SO defaultStyle = null;

        for (int i = 0; i < hairStyles.Count; i++)
        {
            if ((int)hairStyles[i].HairID == 0)
            {
                defaultStyle = hairStyles[i];
            }

            if (hairStyles[i].HairID == hairSelection)
            {
                return hairStyles[i];
            }
        }

        return defaultStyle;
    }    
}
