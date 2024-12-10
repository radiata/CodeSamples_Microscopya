using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitOxygenPuzzle : MonoBehaviour
{
    [SerializeField] private string characterTag = "mainCharacter";

    [SerializeField] private GameObject oxygenPuzzleNavigationObject;
    [SerializeField] private GameObject oxygenPuzzleNavigationLink;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(characterTag) == false)
        {
            return;
        }

        if(PlayerPrefs_Utilities.GetPuzzleSaveState(PuzzleKey.Mitochondria_Oxygen_01) == false)
        {
            return;
        }

        oxygenPuzzleNavigationObject.SetActive(false);
        oxygenPuzzleNavigationLink.SetActive(false);

        Destroy(gameObject);
    }
}
