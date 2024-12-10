using UnityEngine;

public class ADP : MonoBehaviour
{
    [SerializeField] private FloatAway floatAway;

    public void Release()
    {
        transform.parent = null;
        floatAway.enabled = true;
    }

    private void Awake()
    {
        floatAway.enabled = false;
    }
}
