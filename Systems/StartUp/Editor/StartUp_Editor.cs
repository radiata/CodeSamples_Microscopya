using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public static class StartUp_Editor
{
    private static string sceneName = "StartUp Scene";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            return;
        }

        Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive).WaitForCompletion();
    }
}
