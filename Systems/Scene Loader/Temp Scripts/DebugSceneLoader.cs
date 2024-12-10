using UnityEngine;

public class DebugSceneLoader : MonoBehaviour
{
    public void LoadCytoplasm()
    {
        SceneLoader.Instance.LoadScene(SceneID.ChapterOne_CytoplasmFirstVisit, SceneID.MainMenu);
    }

    public void LoadMitochondria()
    {
        SceneLoader.Instance.LoadScene(SceneID.ChapterOne_Mitochondria, SceneID.MainMenu);
    }

    public void LoadMotor()
    {
        SceneLoader.Instance.LoadScene(SceneID.ChapterOne_CytoplasmSecondVisit, SceneID.MainMenu);
    }

    public void LoadER()
    {
        SceneLoader.Instance.LoadScene(SceneID.ChapterTwo_ER, SceneID.MainMenu);
    }
}
