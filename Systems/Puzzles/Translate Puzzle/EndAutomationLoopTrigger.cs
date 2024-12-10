using UnityEngine;

public class EndAutomationLoopTrigger : MonoBehaviour
{
    [SerializeField] private string characterTag = "mainCharacter";
    [SerializeField] private TranslationPuzzleAutomation translationPuzzleAutomation;
    [SerializeField] private GameObject foldingPuzzle;
    [SerializeField] private GameObject aminoAcidChain;

    private bool triggered = false;

    [ContextMenu("End Automation Loop")]
    public void EndAutomationLoop()
    {
        translationPuzzleAutomation.OnAutomationComplete -= RevealFoldingPuzzle;
        translationPuzzleAutomation.OnAutomationComplete += RevealFoldingPuzzle;

        translationPuzzleAutomation.EndAutomation();
    }

    [ContextMenu("Enable Folding Puzzle")]
    public void RevealFoldingPuzzle()
    {
        foldingPuzzle.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered == true
            || translationPuzzleAutomation.IsAutomated == false
            || collision.CompareTag(characterTag) == false)
        {
            return;
        }

        triggered = true;
        aminoAcidChain.SetActive(false);
        EndAutomationLoop();
    }

    private void OnDisable()
    {
        translationPuzzleAutomation.OnAutomationComplete -= RevealFoldingPuzzle;
    }
}
