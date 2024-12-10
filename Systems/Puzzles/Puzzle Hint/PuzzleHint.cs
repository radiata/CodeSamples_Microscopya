using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHint : MonoBehaviour, I_ClickResponder
{
    private bool revealed = false;

    public bool OnClick(Vector3 worldPosition)
    {
        throw new System.NotImplementedException();
    }

    private void EnableHint()
    {
        throw new System.NotImplementedException();

    }

    private void RevealHint()
    {
        throw new System.NotImplementedException();
        revealed = true;
    }
}
