using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadMainMenu
{
    private SceneMapper_SO sceneLibrary;

    public LoadMainMenu(SceneMapper_SO sceneLibrary)
    {
        this.sceneLibrary = sceneLibrary;
    }

    public void DoLoadMainMenu()
    {
        Scene[] loadedScenes = new Scene[SceneManager.sceneCount];
        SceneDataRequirements_SO loadingScene = sceneLibrary.GetSceneDataRequirements(SceneID.Loading);
        SceneDataRequirements_SO userInterfaceScene = sceneLibrary.GetSceneDataRequirements(SceneID.UserInterface);

        var loadedScenesByID = new List<SceneID>();

        for (int i = 0; i < loadedScenes.Length; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);

            if (loadedScenes[i].name != loadingScene.SceneName
                && loadedScenes[i].name != userInterfaceScene.SceneName)
            {
                loadedScenesByID.Add(sceneLibrary.GetSceneID(loadedScenes[i].name));
            }
        }

        SceneLoader.Instance.LoadScene(SceneID.MainMenu, loadedScenesByID);
    }
}
