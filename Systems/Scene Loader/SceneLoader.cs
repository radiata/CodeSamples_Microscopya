using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] private SceneMapper_SO sceneLibrary;
    private LoadMainMenu loadMainMenu;

    private List<SceneLoadOperation> sceneLoadOperations = new List<SceneLoadOperation>();
    private List<SceneID> loadLockedIDs = new List<SceneID>();

    public delegate void OnSceneActivate(SceneID sceneID);
    public event OnSceneActivate OnSceneActivated;

    public SceneMapper_SO SceneLibrary => sceneLibrary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        loadMainMenu = new LoadMainMenu(sceneLibrary);
    }

    public void LoadMainMenu()
    {
        loadMainMenu.DoLoadMainMenu();
    }

    public void LoadScene(SceneID loadSceneID, SceneID unloadSceneID = SceneID.None)
    {
        List<SceneID> unloadSceneIDs =
            unloadSceneID == SceneID.None ? null : new List<SceneID>() { unloadSceneID };

        LoadScene(loadSceneID, unloadSceneIDs);
    }

    public void LoadScene(SceneID loadSceneID, LoadingScreenType overrideLoadScreenType, SceneID unloadSceneID = SceneID.None)
    {
        List<SceneID> unloadSceneIDs =
            unloadSceneID == SceneID.None ? null : new List<SceneID>() { unloadSceneID };

        LoadScene(loadSceneID, unloadSceneIDs, overrideLoadScreenType);
    }

    public void LoadScene(SceneID loadSceneID, List<SceneID> unloadSceneIDs, LoadingScreenType overrideLoadingScreenType = LoadingScreenType.DoNotOverride)
    {
        if (loadLockedIDs.Contains(loadSceneID) == true)
        {
            return;
        }

        SceneLoadOperation? sceneLoading = sceneLoadOperations.FirstOrDefault(search => search.SceneID == loadSceneID);
        if (sceneLoading.Value.isInitialized == true)
        {
            return;
        }

        loadLockedIDs.Add(loadSceneID);

        SceneDataRequirements_SO dataRequirements = sceneLibrary.GetSceneDataRequirements(loadSceneID);

        LoadingScreenType loadingScreenType =
            overrideLoadingScreenType == LoadingScreenType.DoNotOverride
            ? dataRequirements.LoadingScreenType
            : overrideLoadingScreenType;

        HandleLoadingScreen(loadSceneID, loadingScreenType, true);

        if (unloadSceneIDs != null)
        {
            StartCoroutine(WaitForUnloadScenes(unloadSceneIDs, dataRequirements, overrideLoadingScreenType));
        }
        else
        {
            CompleteLoadScene(dataRequirements, overrideLoadingScreenType);
        }
    }

    public void CompleteLoadScene(SceneDataRequirements_SO dataRequirements, LoadingScreenType overrideLoadingScreenType)
    {
        AsyncOperation sceneAsyncOperation = SceneManager.LoadSceneAsync(dataRequirements.SceneName, LoadSceneMode.Additive);
        sceneAsyncOperation.allowSceneActivation = false;
        SceneLoadOperation sceneLoadOperation = new SceneLoadOperation(sceneAsyncOperation, dataRequirements.SceneID, dataRequirements, overrideLoadingScreenType);
        sceneLoadOperations.Add(sceneLoadOperation);

        var audioRequirements = dataRequirements.LoadAudioAssetReferences();
        StartCoroutine(WaitForSceneLoad(audioRequirements, sceneLoadOperation));

        HandleOnSceneLoadConditions(dataRequirements);

        loadLockedIDs.Remove(dataRequirements.SceneID);
    }

    public void UnloadScene(SceneID sceneID)
    {
        var dataRequirements = sceneLibrary.GetSceneDataRequirements(sceneID);

        Scene[] loadedScenes = new Scene[SceneManager.sceneCount];

        for (int i = 0; i < loadedScenes.Length; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);

            if (loadedScenes[i].name == dataRequirements.SceneName)
            {
                dataRequirements.UnloadAudioAssetReferences();
                SceneManager.UnloadSceneAsync(dataRequirements.SceneName);
                break;
            }
        }
    }

    private void ActivateScene(SceneLoadOperation sceneLoadOperation)
    {
        Resources.UnloadUnusedAssets();

        LoadingScreenType loadingScreenType =
            sceneLoadOperation.OverrideLoadingScreen == LoadingScreenType.DoNotOverride
            ? sceneLoadOperation.SceneDataRequirements.LoadingScreenType
            : sceneLoadOperation.OverrideLoadingScreen;

        HandleLoadingScreen(sceneLoadOperation.SceneID, loadingScreenType, false);

        sceneLoadOperations.Remove(sceneLoadOperation);
        OnSceneActivated?.Invoke(sceneLoadOperation.SceneID);
    }

    private void HandleLoadingScreen(SceneID loadSceneID, LoadingScreenType loadingScreenType, bool enabled)
    {
        if (loadingScreenType == LoadingScreenType.Default)
        {
            if (enabled == true)
            {
                LoadingScreen._instance.EnableDefault(loadSceneID);
            }
            else
            {
                LoadingScreen._instance.DisableDefault();
            }

        }
        if (loadingScreenType == LoadingScreenType.BlackScreen)
        {
            if (enabled == true)
            {
                LoadingScreen._instance.EnableBlackScreen(loadSceneID);
            }
            else
            {
                LoadingScreen._instance.DisableBlackScreen();
            }
        }
    }

    private IEnumerator WaitForUnloadScenes(List<SceneID> unloadSceneID, SceneDataRequirements_SO loadSceneData, LoadingScreenType overrideLoadingScreenType)
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        List<AsyncOperation> unloadOperations = new List<AsyncOperation>();

        foreach (SceneID sceneID in unloadSceneID)
        {
            var dataRequirements = sceneLibrary.GetSceneDataRequirements(sceneID);
            dataRequirements.UnloadAudioAssetReferences();
            unloadOperations.Add(SceneManager.UnloadSceneAsync(dataRequirements.SceneName));
        }

        while (unloadOperations.Count > 0)
        {
            for (int i = unloadOperations.Count - 1; i >= 0; i--)
            {
                if (unloadOperations[i].isDone)
                {
                    unloadOperations.RemoveAt(i);
                    break;
                }
            }
            yield return null;
        }
        CompleteLoadScene(loadSceneData, overrideLoadingScreenType);
    }

    private IEnumerator WaitForSceneLoad(List<LoadedAudioAsset> loadedAudioAssets, SceneLoadOperation sceneLoadOperation)
    {
        int loadComplete = 0;

        while (loadComplete < loadedAudioAssets.Count)
        {
            loadComplete = 0;

            for (int i = 0; i < loadedAudioAssets.Count; i++)
            {
                if (loadedAudioAssets[i].isLoaded)
                {
                    loadComplete += 1;
                }
            }

            yield return null;
        }

        sceneLoadOperation.AsyncOperation.allowSceneActivation = true;
        while (sceneLoadOperation.AsyncOperation.isDone == false)
        {
            yield return null;
        }

        ActivateScene(sceneLoadOperation);
    }

    private IEnumerator UnloadStatusReporter()
    {
        yield return null;
    }

    private void HandleOnSceneLoadConditions(SceneDataRequirements_SO dataRequirements)
    {
        if(dataRequirements.OnSceneLoadClearResearchMode == true)
        {
            ClearResearchMode.ExecuteClearResearchMode();
        }
    }
}
