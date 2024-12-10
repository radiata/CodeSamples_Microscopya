using UnityEngine;
using UnityEngine.Splines;

public class DespawnATPAfterAnimation : MonoBehaviour
{
    [SerializeField] private SplineAnimate splineAnimate;

    private float timeElapsed = 0;
    private float despawnTime;

    private void OnEnable()
    {
            despawnTime = splineAnimate.Duration;
    }

    private void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= despawnTime)
        {
            Destroy(gameObject);
        }
    }
}
