using UnityEngine;

public class TriggerSceneLoad : MonoBehaviour
{
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private string tagToLookFor = "mainCharacter";
    [SerializeField] private float fadeTimer = 1f;

    enum TriggerType { Load, Transition }
    enum SceneType { Mito, Cyto, Motor }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tagToLookFor))
        {
            return;
        }

        switch (triggerType)
        {
            case TriggerType.Load:
                break;
            case TriggerType.Transition:
                if (sceneType == SceneType.Mito)
                {
                    SceneLoader.Instance.LoadScene(SceneID.ChapterOne_Mitochondria, SceneID.ChapterOne_CytoplasmFirstVisit);
                }
                if (sceneType == SceneType.Cyto)
                {
                    SceneLoader.Instance.LoadScene(SceneID.ChapterOne_CytoplasmFirstVisit, SceneID.ChapterOne_Intro);
                }
                if (sceneType == SceneType.Motor)
                {
                    SceneLoader.Instance.LoadScene(SceneID.ChapterOne_CytoplasmSecondVisit, SceneID.ChapterOne_Mitochondria);
                }
                break;
        }
    }
}
