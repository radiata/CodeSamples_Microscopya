using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
    [SerializeField] private PuzzleKey puzzleKey;
    [SerializeField] private GameObject hintHolder;
    
    private void OnEnable()
    {
        if(PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == true)
        {
            Destroy(hintHolder);
            return;
        }
        BasePuzzle.OnPuzzleCompleted += OnPuzzleCompleted;
    }

    private void OnDisable()
    {
        BasePuzzle.OnPuzzleCompleted -= OnPuzzleCompleted;
    }

    private void OnPuzzleCompleted(PuzzleKey puzzleKey)
    {
        if (puzzleKey != this.puzzleKey)
        {
            return;
        }

        Destroy(hintHolder);
    }
}
