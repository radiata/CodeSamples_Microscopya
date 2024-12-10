using System;
using System.Linq;
using UnityEngine;

public abstract class PointerInteractable_Base : MonoBehaviour, IComparable<PointerInteractable_Base>
{
    public abstract int PriorityValue();

    public abstract bool IsClickable();
    public abstract bool IsSwipeable();
    public abstract bool IsHoldable();
    public abstract bool IsDraggable();
    public abstract bool IsPointerContactStartable();

    public abstract PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed);
    public abstract void Holding(Vector3 worldPosition, Vector3 cameraForward);
    public abstract void HoldEnd(Vector3 worldPosition);
    public abstract bool Click(Vector3 worldPosition, Vector3 cameraForward);
    public abstract bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward);
    public abstract void Drag();
    public abstract bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward);

    public abstract bool SendPassThrough();
    public abstract bool ReceivePassThrough();

    public int CompareTo(PointerInteractable_Base other)
    {
        return this.PriorityValue().CompareTo(other.PriorityValue());
    }

    public static PointerInteractable_Base[] GetItemStack(Vector3 raycastWorldPoint, Vector3 raycastDirection, LayerMask layerMask)
    {
        var stack2D = GetItemStack2D(raycastWorldPoint, raycastDirection, layerMask);
        var stack3D = GetItemStack3D(raycastWorldPoint, raycastDirection, layerMask);
        var results = stack2D.Concat(stack3D).ToArray();

        Array.Sort(results);

        return results;
    }

    private static PointerInteractable_Base[] GetItemStack2D(Vector3 raycastWorldPoint, Vector2 raycastDirection, LayerMask layerMask)
    {
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(raycastWorldPoint, raycastDirection, Mathf.Infinity, layerMask);
        PointerInteractable_Base[] results = new PointerInteractable_Base[raycastHits.Length];

        for (int i = 0; i < raycastHits.Length; i++)
        {
            results[i] = raycastHits[i].transform.GetComponent<PointerInteractable_Base>();
            if (results[i] == null)
            {
                Debug.LogError("Missing PointerInteractable component");
            }
        }

        return results;
    }

    private static PointerInteractable_Base[] GetItemStack3D(Vector3 raycastWorldPoint, Vector3 raycastDirection, LayerMask layerMask)
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(raycastWorldPoint, raycastDirection, Mathf.Infinity, layerMask);
        PointerInteractable_Base[] results = new PointerInteractable_Base[raycastHits.Length];

        for (int i = 0; i < raycastHits.Length; i++)
        {
            results[i] = raycastHits[i].transform.GetComponent<PointerInteractable_Base>();
            if (results[i] == null)
            {
                Debug.LogError($"Missing PointerInteractable component on {raycastHits[i].transform}", raycastHits[i].transform.gameObject);
            }
        }

        return results;
    }
}
