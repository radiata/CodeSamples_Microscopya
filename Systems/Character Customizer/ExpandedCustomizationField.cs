using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandedCustomizationField : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private GameObject raycastSearchTarget;
    [SerializeField] private GameObject expandedField;

    private bool activeState = false;

    private PointerEventData cachedPointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    public void ToggleActiveState(bool? toggleTo = null)
    {
        if (toggleTo != null)
        {
            if (toggleTo.Value == true)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
            return;
        }

        if (activeState == false)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;
        InputHandler.OnPointerContactOccurred += OnPointerContactOccurred;

        expandedField.SetActive(true);
        activeState = true;
    }

    private void Deactivate()
    {
        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;

        if (expandedField != null)
        {
            expandedField.SetActive(false);
        }

        activeState = false;
    }

    private void OnPointerContactOccurred(Vector2 pointerScreenPosition)
    {
        cachedPointerEventData.position = pointerScreenPosition;
        raycastResults.Clear();

        graphicRaycaster.Raycast(cachedPointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            Debug.Log(raycastResults[i].gameObject.name, raycastResults[i].gameObject);
        }

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject == raycastSearchTarget)
            {
                return;
            }
        }

        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;
        StartCoroutine(DeactivateAtEndOfFrame());
    }

    private void Awake()
    {
        Deactivate();
    }

    private void OnEnable()
    {
        cachedPointerEventData = new PointerEventData(EventSystem.current);
    }

    private void OnDisable()
    {
        Deactivate();
        StopAllCoroutines();
    }

    private IEnumerator DeactivateAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Deactivate();
    }
}
