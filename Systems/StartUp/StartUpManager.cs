using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpManager : MonoBehaviour
{
    public static StartUpManager Instance;
    [SerializeField] private GameObject initialSceneLoader;

    private List<Base_StartUp> startUpProcesses = new List<Base_StartUp>();
    private List<Base_StartUp> completedProcesses = new List<Base_StartUp>();

    public bool isProcessCompleted(Base_StartUp process) => completedProcesses.Contains(process);

    public void RegisterStartUpProcess(Base_StartUp process)
    {
        startUpProcesses.Add(process);
    }

    public void ReportProcessCompletion(Base_StartUp process)
    {
        completedProcesses.Add(process);
        startUpProcesses.Remove(process);

        if(startUpProcesses.Count == 0)
        {
            CompletedLoading();
        }
    }

    public void CompletedLoading()
    {
        FinalizeProcesses();

        if (SceneManager.GetActiveScene() == SceneReferenceLibrary.StartUpScene.Scene)
        {
            initialSceneLoader.SetActive(true);
        }
        
    }

    private void FinalizeProcesses()
    {
        completedProcesses = completedProcesses.OrderBy(process => process.InitializationPriority).ToList();

        for(int i = 0; i < completedProcesses.Count; i++)
        {
            completedProcesses[i].FinalizeProcess();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
