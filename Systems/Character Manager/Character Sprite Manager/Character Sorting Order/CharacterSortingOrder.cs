using System.Collections.Generic;
using UnityEngine;

public class CharacterSortingOrder
{
    private List<int> hairSortingOrders;
    private List<int> skinSortingOrders;
    private List<int> coatSortingOrders;
    private List<int> dressSortingOrders;
    private List<int> shoeSortingOrders;
    private List<int> eyeSortingOrders;

    public void InitializeSortingOrders(List<SpriteRenderer> hairSprites, List<SpriteRenderer> skinSprites, List<SpriteRenderer> coatSprites,
    List<SpriteRenderer> dressSprites, List<SpriteRenderer> shoeSprites, List<SpriteRenderer> eyeSprites)
    {
        hairSortingOrders = InitializeSortingOrder(hairSprites);
        skinSortingOrders = InitializeSortingOrder(skinSprites);
        coatSortingOrders = InitializeSortingOrder(coatSprites);
        dressSortingOrders = InitializeSortingOrder(dressSprites);
        shoeSortingOrders = InitializeSortingOrder(shoeSprites);
        eyeSortingOrders = InitializeSortingOrder(eyeSprites);
    }

    public void ApplySortingOrderChange(int newBaseLayer, List<SpriteRenderer> hairSprites, List<SpriteRenderer> skinSprites, List<SpriteRenderer> coatSprites,
        List<SpriteRenderer> dressSprites, List<SpriteRenderer> shoeSprites, List<SpriteRenderer> eyeSprites)
    {
        UpdateSortingLayer(hairSprites, hairSortingOrders, newBaseLayer);
        UpdateSortingLayer(skinSprites, skinSortingOrders, newBaseLayer);
        UpdateSortingLayer(coatSprites, coatSortingOrders, newBaseLayer);
        UpdateSortingLayer(dressSprites, dressSortingOrders, newBaseLayer);
        UpdateSortingLayer(shoeSprites, shoeSortingOrders, newBaseLayer);
        UpdateSortingLayer(eyeSprites, eyeSortingOrders, newBaseLayer);
    }

    private void UpdateSortingLayer(List<SpriteRenderer> spriteRenderers, List<int> sortingOrders, int newBaseLayer)
    {
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].sortingOrder = (int)(sortingOrders[i] + newBaseLayer + 1);
        }
    }

    private List<int> InitializeSortingOrder(List<SpriteRenderer> spriteRenderers)
    {
        List<int> sortingOrderList = new List<int>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            sortingOrderList.Add(spriteRenderer.sortingOrder);
        }

        return sortingOrderList;
    }
}
