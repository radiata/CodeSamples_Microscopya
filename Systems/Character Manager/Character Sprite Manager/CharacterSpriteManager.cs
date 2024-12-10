using System.Collections.Generic;
using UnityEngine;

public class CharacterSpriteManager : MonoBehaviour
{
    [SerializeField] private CharacterColorOptions_SO characterColorOptions_SO;
    private CharacterColorOptions characterColorOptions;

    [SerializeField] private CharacterHairOptions_SO characterHairOptions_SO;
    private CharacterHairOptions characterHairOptions;

    private CharacterSortingOrder characterSortingOrder;

    [SerializeField] private HairSprites hairSprites;
    [SerializeField] private List<SpriteRenderer> skinSprites;
    [SerializeField] private List<SpriteRenderer> coatSprites;
    [SerializeField] private List<SpriteRenderer> dressSprites;
    [SerializeField] private List<SpriteRenderer> shoeSprites;
    [SerializeField] private List<SpriteRenderer> eyeSprites;

    [Header("Sprites to enable/disable when the model is facing the corresponding direction")]
    [SerializeField] private List<SpriteRenderer> leftFacingSprites;
    [SerializeField] private List<SpriteRenderer> rightFacingSprites;

    private void Awake()
    {
        characterSortingOrder = new CharacterSortingOrder();
        characterSortingOrder.InitializeSortingOrders(hairSprites.spriteRenderers, skinSprites, coatSprites, dressSprites, shoeSprites, eyeSprites);

        characterColorOptions = new CharacterColorOptions(characterColorOptions_SO);
        characterHairOptions = new CharacterHairOptions(characterHairOptions_SO);
        UpdateColors();
        UpdateHair();
    }

    private void OnEnable()
    {
        CharacterColorOptions_SO.OnCharacterColorChanged += UpdateColors;
        CharacterHairOptions_SO.OnCharacterHairChanged += UpdateHair;
        CharacterFacingReporter.OnCharacterFacingChanged += SetFacing;
        CharacterNavigationObjectReporter.OnNavigationObjectChanged += UpdateSortingLayers;
        CharacterNavigationObjectReporter.OnSortingOrderChange += UpdateSortingLayers;
    }

    private void OnDisable()
    {
        CharacterColorOptions_SO.OnCharacterColorChanged -= UpdateColors;
        CharacterHairOptions_SO.OnCharacterHairChanged -= UpdateHair;
        CharacterFacingReporter.OnCharacterFacingChanged -= SetFacing;
        CharacterNavigationObjectReporter.OnNavigationObjectChanged -= UpdateSortingLayers;
        CharacterNavigationObjectReporter.OnSortingOrderChange -= UpdateSortingLayers;
    }

    private void UpdateColors()
    {
        characterColorOptions.ApplyColorOptions(hairSprites.spriteRenderers, skinSprites, coatSprites, dressSprites, shoeSprites);
    }

    private void UpdateHair()
    {
        characterHairOptions.ApplyHairOptions(hairSprites);
    }

    private void UpdateSortingLayers(NavigationObject navigationObject)
    {
        if(navigationObject == null)
        {
            return;
        }

        characterSortingOrder.ApplySortingOrderChange(navigationObject.CharacterSortingOrder,
            hairSprites.spriteRenderers, skinSprites, coatSprites, dressSprites, shoeSprites, eyeSprites);
    }

    private void SetFacing(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.uninitialized:
                break;
            case FacingDirection.left:
                SetLeftFacingSprites();
                break;
            case FacingDirection.right:
                SetRightFacingSprites();
                break;
        }
    }

    private void SetRightFacingSprites()
    {
        foreach (SpriteRenderer spriteRenderer in leftFacingSprites)
        {
            spriteRenderer.enabled = false;
        }
        foreach (SpriteRenderer spriteRenderer in rightFacingSprites)
        {
            spriteRenderer.enabled = true;
        }
    }
    private void SetLeftFacingSprites()
    {
        foreach (SpriteRenderer spriteRenderer in rightFacingSprites)
        {
            spriteRenderer.enabled = false;
        }
        foreach (SpriteRenderer spriteRenderer in leftFacingSprites)
        {
            spriteRenderer.enabled = true;
        } 
    }
}
