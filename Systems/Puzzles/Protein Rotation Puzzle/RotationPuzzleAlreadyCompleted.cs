using UnityEngine;

public class RotationPuzzleAlreadyCompleted : MonoBehaviour
{
    [SerializeField] private GameObject staggeredEnabling;

    private void Start()
    {
        if(PlayerPrefs_Utilities.GetPuzzleSaveState(PuzzleKey.RoughER_ProteinRotation) == true)
        {
            staggeredEnabling.SetActive(true);
        }
    }
}
