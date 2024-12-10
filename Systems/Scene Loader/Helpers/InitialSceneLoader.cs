using UnityEngine;

public class InitialSceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
