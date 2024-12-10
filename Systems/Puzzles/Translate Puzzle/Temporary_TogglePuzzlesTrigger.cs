using UnityEngine;

public class Temporary_TogglePuzzlesTrigger : MonoBehaviour
{
    [SerializeField] private string characterTag = "mainCharacter";
    [SerializeField] private GameObject rotationPuzzle;

    private bool triggered = false;

    private void Start()
    {
        if (PlayerPrefs_Utilities.GetPuzzleSaveState(PuzzleKey.RoughER_ProteinRotation) == true)
        {
            triggered = true;
            rotationPuzzle.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered == true
            || collision.CompareTag(characterTag) == false)
        {
            return;
        }

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(PuzzleKey.RoughER_ProteinTranslation) == true)
        {
            triggered = true;
            rotationPuzzle.SetActive(true);
        }
    }
}
