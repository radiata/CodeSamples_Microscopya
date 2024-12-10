using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAwayAndFadeOut : MonoBehaviour
{
    // Fade Related
    [SerializeField] private float fadeStartTime;
    [SerializeField] private AlphaBlendAnimator alphaBlendAnimator;
    [SerializeField] private AlphaBlendAnimator secondaryAlphaBlendAnimator;


    //float related
    [SerializeField] private float timeToReachRandomDestination;
    [SerializeField] private GameObject rootObject;

    [SerializeField] private List<Vector2> randomRangesX;
    [SerializeField] private List<Vector2> randomRangesY;

    private Vector3 randomLocation;
    private float speed = 1f;
    private float destroyBuffer = .01f;

    private bool useSecondaryAnimator = false;

    public delegate void LerpToLocationCompleteEvent();
    public event LerpToLocationCompleteEvent OnLerpToLocationCompleted;

    public void StartBehaviour(bool useSecondaryAnimator)
    {
        this.useSecondaryAnimator = useSecondaryAnimator;
        GenerateRandomLocation();
        speed = Vector2.Distance(randomLocation, transform.localPosition) / timeToReachRandomDestination;
        Invoke("StartFade", fadeStartTime);
        Invoke("DestroyObject", fadeStartTime + alphaBlendAnimator.TransitionTime + destroyBuffer);
        StartCoroutine(LerpToLocation());
    }

    public void StartBehaviour_NonDestructive()
    {
        GenerateRandomLocation();
        speed = Vector2.Distance(randomLocation, transform.localPosition) / timeToReachRandomDestination;
        Invoke("StartFade", fadeStartTime);
        StartCoroutine(LerpToLocation());
    }

    private void StartFade()
    {
        alphaBlendAnimator.AnimateAlphaBlend(1, 0, timeToReachRandomDestination - fadeStartTime);

        if (useSecondaryAnimator == true)
        {
            secondaryAlphaBlendAnimator.AnimateAlphaBlend(1, 0, timeToReachRandomDestination - fadeStartTime);
        }
    }

    private void DestroyObject()
    {
        Destroy(rootObject);
    }

    private void GenerateRandomLocation()
    {
        int xIndex = Random.Range(0, randomRangesX.Count);
        int yIndex = Random.Range(0, randomRangesY.Count);

        float randomX = Random.Range(randomRangesX[xIndex].x, randomRangesX[xIndex].y);
        float randomY = Random.Range(randomRangesY[yIndex].x, randomRangesY[yIndex].y);

        randomLocation = new Vector3(rootObject.transform.localPosition.x + randomX, rootObject.transform.localPosition.y + randomY, rootObject.transform.localPosition.z);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator LerpToLocation()
    {
        float elapsedTime = 0;
        float normalTime = 0;
        Vector3 startPosition = rootObject.transform.localPosition;

        while (elapsedTime < timeToReachRandomDestination)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            normalTime = Mathf.Clamp01(elapsedTime / timeToReachRandomDestination);

            rootObject.transform.localPosition = Vector2.Lerp(startPosition, randomLocation, normalTime);
        }

        rootObject.transform.localPosition = Vector2.Lerp(startPosition, randomLocation, 1);

        OnLerpToLocationCompleted?.Invoke();
    }
}
