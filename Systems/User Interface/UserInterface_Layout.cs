using UnityEngine;

public class UserInterface_Layout : MonoBehaviour
{
    [Header("Top Right Buttons")]
    [SerializeField] private bool pauseButton_Active;
    [SerializeField] private bool volumeButton_Active;
    [SerializeField] private bool hintButton_Active;

    [Header("Top Left Buttons")]
    [SerializeField] private bool trophyButton_Active;
    [SerializeField] private bool journalButton_Active;
    
    [Header("Bottom Right Buttons")]
    [SerializeField] private bool researchModeButton_Active;

    [Header("Objective Information")]
    [SerializeField] private bool objectiveText_Active;

    [Header("Active Panel")]
    [SerializeField] private GameObject activePanel;

    //index counts left to right
    private GameObject topRightButtons_ButtonSlot00;
    private GameObject topRightButtons_ButtonSlot01;
    private GameObject topRightButtons_ButtonSlot02;

    //index counts left to right
    private GameObject topLeftButtons_ButtonSlot00;
    private GameObject topLeftButtons_ButtonSlot01;

    //index counts left to right
    private GameObject bottomRightButtons_ButtonSlot00;

    //reference to the objective asset, doesn't need an associated slot currently
    private GameObject objectiveText;

    //references to the actual usable buttons
    private GameObject pauseButton;
    private GameObject volumeButton;
    private GameObject hintButton;
    private GameObject trophyButton;
    private GameObject journalButton;
    private GameObject researchModeButton;

    public void DeactivateLayout()
    {
        SetButtonsActive(false);

        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }
    }

    public void ActivateLayout()
    {
        SetTopRightLayout();
        SetTopLeftLayout();
        SetBottomRightLayout();
        SetButtonsActive(true);

        if (activePanel != null)
        {
            activePanel.SetActive(true);
        }
    }

    private void SetTopRightLayout()
    {
        if (pauseButton_Active == true)
        {
            pauseButton.transform.SetParent(topRightButtons_ButtonSlot02.transform, false);
        }

        if (volumeButton_Active == true)
        {
            if (pauseButton_Active == true)
            {
                volumeButton.transform.SetParent(topRightButtons_ButtonSlot01.transform, false);
            }
            else
            {
                volumeButton.transform.SetParent(topRightButtons_ButtonSlot02.transform, false);
            }
        }

        if (hintButton_Active == true)
        {
            if (pauseButton_Active == true)
            {
                if (volumeButton_Active == true)
                {
                    hintButton.transform.SetParent(topRightButtons_ButtonSlot00.transform, false);
                }
                else
                {
                    hintButton.transform.SetParent(topRightButtons_ButtonSlot01.transform, false);
                }
            }
            else
            {
                if (volumeButton_Active == true)
                {
                    hintButton.transform.SetParent(topRightButtons_ButtonSlot01.transform, false);
                }
                else
                {
                    hintButton.transform.SetParent(topRightButtons_ButtonSlot02.transform, false);
                }
            }
        }
    }

    private void SetTopLeftLayout()
    {
        if(trophyButton_Active == true)
        {
            trophyButton.transform.SetParent(topLeftButtons_ButtonSlot00.transform, false);
        }

        if(journalButton_Active == true)
        {
            if(trophyButton_Active == true)
            {
                journalButton.transform.SetParent(topLeftButtons_ButtonSlot01.transform, false);
            }
            else
            {
                journalButton.transform.SetParent(topLeftButtons_ButtonSlot00.transform, false);
            }
        }
    }

    private void SetBottomRightLayout()
    {
        if(researchModeButton_Active == true)
        {
            researchModeButton.transform.SetParent(bottomRightButtons_ButtonSlot00.transform, false);
        }
    }

    private void SetButtonsActive(bool active)
    {
        pauseButton.SetActive(active && pauseButton_Active);
        volumeButton.SetActive(active && volumeButton_Active);
        hintButton.SetActive(active && hintButton_Active);
        trophyButton.SetActive(active && trophyButton_Active);
        journalButton.SetActive(active && journalButton_Active);
        researchModeButton.SetActive(active && researchModeButton_Active);
        objectiveText.SetActive(active && objectiveText_Active);
    }

    public void SetVariables(GameObject TR_00, GameObject TR_01, GameObject TR_02
        , GameObject TL_00, GameObject TL_01
        , GameObject BR_00
        , GameObject objectiveText, GameObject pauseButton, GameObject volumeButton, GameObject hintButton
        , GameObject trophyButton, GameObject journalButton, GameObject researchModeButton)
    {
        topRightButtons_ButtonSlot00 = TR_00;
        topRightButtons_ButtonSlot01 = TR_01;
        topRightButtons_ButtonSlot02 = TR_02;

        topLeftButtons_ButtonSlot00 = TL_00;
        topLeftButtons_ButtonSlot01 = TL_01;

        bottomRightButtons_ButtonSlot00 = BR_00;

        this.objectiveText = objectiveText;
        this.pauseButton = pauseButton;
        this.volumeButton = volumeButton;   
        this.hintButton = hintButton;
        this.trophyButton = trophyButton;
        this.journalButton = journalButton;
        this.researchModeButton = researchModeButton;
    }
}
