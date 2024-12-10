using UnityEngine;

public class UserInterface_LayoutDataInjector : MonoBehaviour
{
    [Header("Top Right Layout")]
    [SerializeField] private GameObject topRightButtons_ButtonSlot00;
    [SerializeField] private GameObject topRightButtons_ButtonSlot01;
    [SerializeField] private GameObject topRightButtons_ButtonSlot02;

    [Header("Top Left Layout")]
    [SerializeField] private GameObject topLeftButtons_ButtonSlot00;
    [SerializeField] private GameObject topLeftButtons_ButtonSlot01;

    [Header("Bottom Right Layout")]
    [SerializeField] private GameObject bottomRightButtons_ButtonSlot00;

    [Header("Objective")]
    [SerializeField] private GameObject objectiveText;

    [Header("Buttons")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject volumeButton;
    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject trophyButton;
    [SerializeField] private GameObject journalButton;
    [SerializeField] private GameObject researchModeButton;


    public void InjectDataTo(UserInterface_Layout layout)
    {
        layout.SetVariables(
            topRightButtons_ButtonSlot00, topRightButtons_ButtonSlot01, topRightButtons_ButtonSlot02
            , topLeftButtons_ButtonSlot00, topLeftButtons_ButtonSlot01
            , bottomRightButtons_ButtonSlot00
            ,objectiveText, pauseButton, volumeButton, hintButton
            , trophyButton, journalButton, researchModeButton);
    }
}
