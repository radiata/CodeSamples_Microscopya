[System.Serializable]
public class BehaviourRequest
{
    public static int requestIncrementor;

    private int requestID;
    public int RequestID => requestID;

    public delegate void ReleaseRequestEvent(int requestID);
    public static ReleaseRequestEvent OnReleaseRequest;

    public BehaviourRequest()
    {
        requestID = requestIncrementor++;
    }

    public void ReleaseRequest()
    {
        OnReleaseRequest?.Invoke(requestID);
    }
}
