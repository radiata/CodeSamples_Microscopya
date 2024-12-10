using UnityEngine;

public class IntroLoadHelper : MonoBehaviour
{
    private void OnEnable()
    {
        SceneLoader.Instance.LoadScene(SceneID.ChapterOne_CytoplasmFirstVisit, LoadingScreenType.BlackScreen, SceneID.ChapterOne_Intro);
    }
}
