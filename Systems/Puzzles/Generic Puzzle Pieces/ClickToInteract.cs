using UnityEngine;

public class ClickToInteract : MonoBehaviour, I_ClickablePuzzlePiece
{
    [SerializeField] private ClickResponder_Reference clickResponder_Reference;
    private I_ClickResponder clickResponder;
    [SerializeField] private bool updateClickResponder;

    [SerializeField] private bool navigateToPuzzleOnInteract = true;
    [SerializeField] private PuzzleManager puzzleManager;

    [SerializeField] private SoundEffect OnClickFail_Sound;
    [SerializeField] private SoundEffect OnClickSuccess_Sound;

    public void OnClick(Vector3 worldPosition)
    {
        if (navigateToPuzzleOnInteract == true)
        {
            puzzleManager.Navigate();
        }

        if(updateClickResponder == true)
        {
            ResolveResponderInteractions(worldPosition);
        }
        else
        {
            AudioController.Instance.PlaySoundEffect(OnClickFail_Sound, false);
        }
        
    }

    private void ResolveResponderInteractions(Vector3 worldPosition)
    {
        if(clickResponder.OnClick(worldPosition))
        {
            AudioController.Instance.PlaySoundEffect(OnClickSuccess_Sound, false);
        }
        else
        {
            AudioController.Instance.PlaySoundEffect(OnClickFail_Sound, false);
        }
    }

    private void Start()
    {
        clickResponder = clickResponder_Reference.GetClickResponder();
    }
}
