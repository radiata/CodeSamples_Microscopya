using UnityEngine;

public class SceneLoader_StartUp : Base_StartUp
{
    [SerializeField] private Template_SO sceneLoaderTemplate_SO;

    public static SceneLoader activeSceneLoader;

    protected override void RunProcess()
    {
        activeSceneLoader = sceneLoaderTemplate_SO.InstantiateTemplateObject().GetComponentInChildren<SceneLoader>();
        DontDestroyOnLoad(activeSceneLoader);
    }

    protected override bool CheckProcessComplete()
    {
        return true;
    }

    public override void FinalizeProcess()
    {
    }
}
