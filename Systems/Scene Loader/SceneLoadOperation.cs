using UnityEngine;

public struct SceneLoadOperation
{
    public bool isInitialized;
    public AsyncOperation AsyncOperation;
    public SceneID SceneID;
    public SceneDataRequirements_SO SceneDataRequirements;
    public LoadingScreenType OverrideLoadingScreen;

    public SceneLoadOperation(AsyncOperation asyncOperation, SceneID sceneID, SceneDataRequirements_SO sceneDataRequirements, LoadingScreenType overrideLoadingScreen)
    {
        AsyncOperation = asyncOperation;
        SceneID = sceneID;
        SceneDataRequirements = sceneDataRequirements;
        isInitialized = true;
        OverrideLoadingScreen = overrideLoadingScreen;
    }
}
