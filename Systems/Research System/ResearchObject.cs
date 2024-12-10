using System.Collections.Generic;
using UnityEngine;

public class ResearchObject : MonoBehaviour
{
    [SerializeField] private ResearchEntryID researchEntryID;
    [SerializeField] private string labelText;

    [SerializeField] private GameObject magnifyingGlass;

    [SerializeField] private ResearchLabel primaryLabelPin;
    [SerializeField] private List<ResearchLabel> secondaryLabelPins;

    [SerializeField] private GameObject particleAnimation_ResearchMode;
    [SerializeField] private GameObject particleAnimation_Standard;

    [SerializeField] private bool displayMessage = false;
    [SerializeField] private TranslatableText_SO messageText;
    [SerializeField] private float messageDisplayTime = 10f;

    [SerializeField] private bool secondaryOnly = false;

    public void PlayUnlockAnimation()
    {
        GameObject particleAnimation = ResearchModeState.ResearchModeEnabledState == true ? particleAnimation_ResearchMode : particleAnimation_Standard;

        Instantiate(particleAnimation, primaryLabelPin.transform.position, Quaternion.identity);

        if (displayMessage == true)
        {
            Message.Instance.DisplayMessage(messageText, messageDisplayTime);
        }
    }

    public void UnlockResearchEntry()
    {
        ResearchUnlocks.UnlockResearchEntry(researchEntryID, this);
    }

    private void OnResearchEntryUnlocked(ResearchEntryID unlockedID)
    {
        if (researchEntryID != unlockedID)
        {
            return;
        }

        ResearchUnlocks.OnResearchEntryUnlocked -= OnResearchEntryUnlocked;

        if (ResearchModeState.ResearchModeEnabledState == true)
        {
            ActivateResearchMode();
        }
        else
        {
            DeactivateResearchMode();
        }
    }

    private void OnResearchModeStateChange(bool isEnabled)
    {
        if (isEnabled == false)
        {
            DeactivateResearchMode();
        }
        else
        {
            ActivateResearchMode();
        }
    }

    private void ActivateResearchMode()
    {
        bool entryIsUnlocked = ResearchUnlocks.IsResearchEntryUnlocked(researchEntryID);

        if (secondaryOnly == false)
        {
            primaryLabelPin.ActivateAsPrimaryLabel(entryIsUnlocked, labelText);
        }

        foreach (ResearchLabel label in secondaryLabelPins)
        {
            label.ActivateAsSecondaryLabel(entryIsUnlocked, labelText);
        }

        if (secondaryOnly == false)
        {
            magnifyingGlass.SetActive(false);
        }
    }

    private void DeactivateResearchMode()
    {
        DeactivateResearchLabels();

        if (magnifyingGlass != null)
        {
            magnifyingGlass.SetActive(
                ResearchUnlocks.IsResearchEntryUnlocked(researchEntryID) == true ? false : true);
        }
    }

    private void OnEnable()
    {
        DeactivateResearchLabels();

        if (ResearchUnlocks.IsResearchEntryUnlocked(researchEntryID) == true)
        {
            OnResearchEntryUnlocked(researchEntryID);
        }
        else
        {
            ResearchUnlocks.OnResearchEntryUnlocked += OnResearchEntryUnlocked;
        }

        if (ResearchModeState.ResearchModeEnabledState == true)
        {
            OnResearchModeStateChange(true);
        }

        ResearchModeState.OnResearchModeStateChanged += OnResearchModeStateChange;
    }

    private void OnDisable()
    {
        ResearchUnlocks.OnResearchEntryUnlocked -= OnResearchEntryUnlocked;
        ResearchModeState.OnResearchModeStateChanged -= OnResearchModeStateChange;

        DeactivateResearchLabels();

        if (ResearchModeState.ResearchModeEnabledState == true)
        {
            OnResearchModeStateChange(false);
        }
    }

    private void DeactivateResearchLabels()
    {
        foreach (ResearchLabel label in secondaryLabelPins)
        {
            label.DeactivateLabel();
        }

        if (secondaryOnly == false)
        {
            primaryLabelPin.DeactivateLabel();
        }
    }

    private void OnValidate()
    {
        if (secondaryOnly == false)
        {
            primaryLabelPin.EDITOR_SetText(labelText);
        }

        foreach (ResearchLabel label in secondaryLabelPins)
        {
            label.EDITOR_SetText(labelText);
        }
    }

    public void Tutorial_ForcedRefresh(bool researchModeState)
    {
        OnResearchModeStateChange(researchModeState);
    }
}
