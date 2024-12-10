using UnityEngine;

public class FloatAway : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;

    private float rangeMinimum = 10f;
    private float rangeXAxisVariance = 10f;
    private float rangeYAxisVariance = 10f;

    private float distanceTolerance = 1f;
    private Vector3 randomLocation;

    private float speed = 1f;

    private void OnEnable()
    {
        GenerateRandomLocation();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, randomLocation) < distanceTolerance)
        {
            Destroy(rootObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, randomLocation, speed * Time.fixedDeltaTime);
    }

    private void GenerateRandomLocation()
    {
        float randomX = Random.Range(rangeMinimum, rangeMinimum + rangeXAxisVariance);
        if (Random.Range(0, 2) == 1)
        { randomX *= -1; }

        float randomY = Random.Range(rangeMinimum, rangeMinimum + rangeYAxisVariance);
        if (Random.Range(0, 2) == 1)
        { randomY *= -1; }

        randomLocation = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
    }
}
