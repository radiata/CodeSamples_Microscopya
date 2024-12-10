using UnityEngine;

public class ClickResponderTRNA : MonoBehaviour, I_ClickResponder
{
    [SerializeField] private CharacterTRNAHolder characterTRNAHolder;
    [SerializeField] private CollectableTRNA tRNA;

    public bool OnClick(Vector3 worldPosition)
    {
        if(characterTRNAHolder.AttemptToCollect(tRNA) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
