using UnityEngine;

public class SpawnATPOnEnable : MonoBehaviour
{
    [SerializeField] private Transform parentForSpawnedATP;
    [SerializeField] private GameObject prefabATP;

    private void OnEnable()
    {
        var ATP = Instantiate(prefabATP, parentForSpawnedATP);
        ATP.transform.position = transform.position;
        ATP.SetActive(true);
    }
}
