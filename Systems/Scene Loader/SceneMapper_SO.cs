using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Library", menuName = "Scene Requirements/Scriptable Objects/Scene Library")]
public class SceneMapper_SO : ScriptableObject
{
    [SerializeField] private List<SceneDataRequirements_SO> scenes;

    public SceneDataRequirements_SO GetSceneDataRequirements(SceneID sceneID)
    {
        if (sceneID == SceneID.None)
        {
            return null;
        }

        return scenes.First(search => search.SceneID == sceneID);
    }

    public SceneID GetSceneID(string sceneName)
    {
        return scenes.First(search => search.SceneName == sceneName).SceneID;
    }
}
