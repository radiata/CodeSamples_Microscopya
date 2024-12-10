using UnityEngine;

public class FreeFloat : MonoBehaviour
{
    [SerializeField] private bool isTethered = false;
    [SerializeField] private float tetherRangeXAxis;
    [SerializeField] private float tetherRangeYAxis;

    [SerializeField] private float speed = 1f;

    [Tooltip("Currently Local Space relative is forced on in awake")]
    [SerializeField] private bool localSpaceRelative = true;
    private Vector2 randomDestination;

    private float distanceTolerance = .1f;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        localSpaceRelative = true;
    }

    void Update()
    {
        if (localSpaceRelative)
        {
            ExecuteFreeFloat_LocalSpaceRelative();
        }
        else
        {
            ExecuteFreeFloat_WorldSpaceRelative();
        }
    }

    private void ExecuteFreeFloat_LocalSpaceRelative()
    {
        if (Vector2.Distance(transform.localPosition, randomDestination) < distanceTolerance)
        {
            randomDestination = new Vector2(Random.Range(-tetherRangeXAxis, tetherRangeXAxis), Random.Range(-tetherRangeYAxis, tetherRangeYAxis));
        }

        transform.localPosition = Vector2.MoveTowards(transform.localPosition, randomDestination, speed * Time.deltaTime);
    }

    private void ExecuteFreeFloat_WorldSpaceRelative()
    {
        throw new System.NotImplementedException();
    }
}
