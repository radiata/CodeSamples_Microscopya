using System.Collections.Generic;
using UnityEngine;

public class GenerateATP : MonoBehaviour
{
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private float spawnRate = 8f, atpSpeed = 2f;
    [SerializeField] private Transform atpTarget;
    private float timer = 0f;

    [SerializeField] private List<GameObject> validReceivers;

    private void Start()
    {
        timer = spawnRate;
    }

    private void Generate()
    {
        GameObject objectInstance = Instantiate(prefabObject);

        objectInstance.transform.position = new Vector2(gameObject.transform.position.x + Random.Range(-10f, 10f), gameObject.transform.position.y + Random.Range(-3f, 3f));

        GuideATP newComponent = objectInstance.GetComponentInChildren<GuideATP>();

        newComponent.target = new Vector2(atpTarget.position.x + Random.Range(-10f, 10f), atpTarget.position.y + Random.Range(-3f, 3f));
        newComponent.speed = atpSpeed;

        DragAndDrop dragAndDrop = objectInstance.GetComponentInChildren<DragAndDrop>();
        dragAndDrop.AssignValidReceivers(validReceivers);
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            timer = 0f;
            Generate();
        }
    }
}
