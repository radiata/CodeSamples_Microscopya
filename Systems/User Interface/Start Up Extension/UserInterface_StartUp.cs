using UnityEngine;

public class UserInterface_StartUp : Base_StartUp
{
    private bool sceneLoaded = false;
    private bool startedLoading = false;
    [SerializeField] private Base_StartUp sceneLoaderDependency;
    [SerializeField] private Base_StartUp languageDependency;

    protected override void RunProcess()
    {
    }

    protected override bool CheckProcessComplete()
    {
        if (StartUpManager.Instance.isProcessCompleted(sceneLoaderDependency) == false)
        {
            return false;
        }

        if (StartUpManager.Instance.isProcessCompleted(languageDependency) == false)
        {
            return false;
        }

        if (startedLoading == false)
        {
            SceneLoader.Instance.LoadScene(SceneID.UserInterface);

            SceneLoader.Instance.OnSceneActivated -= OnSceneActivated;
            SceneLoader.Instance.OnSceneActivated += OnSceneActivated;

            startedLoading = true;
            return false;
        }

        return sceneLoaded;
    }

    public override void FinalizeProcess()
    {
    }

    private void OnSceneActivated(SceneID sceneID)
    {
        if (sceneID == SceneID.UserInterface)
        {
            sceneLoaded = true;
            SceneLoader.Instance.OnSceneActivated -= OnSceneActivated;
        }
    }
}
