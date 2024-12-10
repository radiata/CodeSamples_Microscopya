using UnityEngine;

public class Event_LoadScene : Base_Event
{
    [SerializeField] private SceneID sceneToLoad;
    [SerializeField] private SceneID sceneToUnload;
    internal override void HandleEvent()
    {
        SceneLoader.Instance.LoadScene(loadSceneID: sceneToLoad, unloadSceneID: sceneToUnload);
        CompleteEvent();
    }
}
