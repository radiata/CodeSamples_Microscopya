using System.Collections.Generic;
using UnityEngine;

public class AdenosineTriphosphate : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;

    [SerializeField] private Transform spawnPointADP;
    [SerializeField] private GameObject prefabADP;

    [SerializeField] private Transform spawnPointPhosphate;
    [SerializeField] private GameObject prefabPhosphate;

    [SerializeField] private List<GameObject> attachedPhosphates = new List<GameObject>();

    public ADP ConvertToADP()
    {
        CreateProton();

        DestroyATP();

        return CreateADP();
    }

    private ADP CreateADP()
    {
        GameObject objectInstance = Instantiate(prefabADP);
        objectInstance.transform.position = spawnPointADP.position;
        objectInstance.transform.parent = transform.parent;
        return objectInstance.GetComponent<ADP>();
    }

    private void CreateProton()
    {
        GameObject proton = Instantiate(prefabPhosphate, spawnPointPhosphate.transform);
        proton.transform.SetParent(null);
    }

    private void DestroyATP()
    {
        Destroy(rootObject);
        Destroy(gameObject);

        foreach(GameObject gameObject in attachedPhosphates)
        {
            Destroy(gameObject);
        }
    }
}
