using System.Collections;
using UnityEngine;

public abstract class Base_StartUp : MonoBehaviour
{
    [SerializeField] public int InitializationPriority = 0;

    protected abstract void RunProcess();
    protected abstract bool CheckProcessComplete();
    public abstract void FinalizeProcess();

    private IEnumerator Start()
    {
        RegisterProcess();
        RunProcess();

        yield return null;
        while (CheckProcessComplete() == false)
        {
            yield return null;
        }

        ReportProcessComplete();
    }

    private void RegisterProcess()
    {
        StartUpManager.Instance.RegisterStartUpProcess(this);
    }

    private void ReportProcessComplete()
    {
        StartUpManager.Instance.ReportProcessCompletion(this);
    }
}
