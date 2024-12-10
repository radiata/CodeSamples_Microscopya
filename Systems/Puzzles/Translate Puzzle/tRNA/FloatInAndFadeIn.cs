using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInAndFadeIn : MonoBehaviour
{
    [SerializeField] private AlphaBlendAnimator alphaBlendAnimator;
    [SerializeField] private AlphaBlendAnimator secondaryAlphaBlendAnimator;

    [SerializeField] private float timeToReachRandomDestination;
    [SerializeField] private GameObject rootObject;

    [SerializeField] private List<Vector2> randomRangesX;
    [SerializeField] private List<Vector2> randomRangesY;

    private Vector3 randomLocation;

    public delegate void LerpFromLocationCompleteEvent();
    public  event LerpFromLocationCompleteEvent OnLerpFromLocationCompleted;

    public float TimeToDestination => timeToReachRandomDestination;

    public void StartBehaviour()
    {
        GenerateRandomLocalLocation();
        SetPositionToGeneratedLocation();
        alphaBlendAnimator.AnimateAlphaBlend(0, 1);
        secondaryAlphaBlendAnimator.AnimateAlphaBlend(0, 1);
        StartCoroutine(LerpFromLocation());
    }

    public void StartBehaviour_FromCharacter(Vector3 localPosition)
    {
        randomLocation = localPosition;
        StartCoroutine(LerpFromLocation());
    }

    private void GenerateRandomLocalLocation()
    {
        int xIndex = Random.Range(0, randomRangesX.Count);
        int yIndex = Random.Range(0, randomRangesY.Count);

        float randomX = Random.Range(randomRangesX[xIndex].x, randomRangesX[xIndex].y);
        float randomY = Random.Range(randomRangesY[yIndex].x, randomRangesY[yIndex].y);

        randomLocation = new Vector3(randomX, randomY, rootObject.transform.localPosition.z);
    }

    private void SetPositionToGeneratedLocation()
    {
        rootObject.transform.localPosition = randomLocation;
    }

    private IEnumerator LerpFromLocation()
    {
        float elapsedTime = 0;
        float normalTime = 0;

        while ( elapsedTime < timeToReachRandomDestination)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            normalTime = Mathf.Clamp01(elapsedTime / timeToReachRandomDestination);

            rootObject.transform.localPosition = Vector2.Lerp(randomLocation, Vector2.zero, normalTime);
        }

        rootObject.transform.localPosition = Vector2.Lerp(randomLocation, Vector2.zero, 1);

        OnLerpFromLocationCompleted?.Invoke();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
