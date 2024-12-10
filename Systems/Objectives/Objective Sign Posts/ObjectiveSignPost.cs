using UnityEngine;

public class ObjectiveSignPost : MonoBehaviour
{
    [SerializeField] private GameObject signPostParent;

    public void EnableSignPost()
    {
        signPostParent.SetActive(true);
    }

    public void DisableSignPost()
    {
        signPostParent.SetActive(false);
    }

    private void Reset()
    {
        signPostParent = gameObject;
    }
}
