using UnityEngine.SceneManagement;

public struct SceneInformation
{
    public string SceneName;
    public Scene Scene
    {  
        get
        {
            return SceneManager.GetSceneByName(SceneName);
        }
    }
}