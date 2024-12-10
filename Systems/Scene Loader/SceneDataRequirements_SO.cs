using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data Requirements", menuName = "Scene Requirements/Scriptable Objects/Scene Data Requirements")]
public class SceneDataRequirements_SO : ScriptableObject
{
    [SerializeField] private SceneID sceneID;
    [SerializeField] private string sceneName;

    [SerializeField] private List<MusicTrack> preloadMusicTracks;
    [SerializeField] private List<SoundEffect> preloadSoundEffects;

    [SerializeField] private LoadingScreenType loadingScreenType;

    [SerializeField] private bool onSceneLoadClearResearchMode = false;

    public delegate void LoadStatus(float percentComplete);
    public event LoadStatus OnLoadStatusUpdate;
    public event LoadStatus OnUnloadStatusUpdate;

    private List<LoadedAudioAsset> loadedAudioAssets = new List<LoadedAudioAsset>();

    public SceneID SceneID => sceneID;
    public string SceneName => sceneName;

    public bool OnSceneLoadClearResearchMode => onSceneLoadClearResearchMode;
    public LoadingScreenType LoadingScreenType => loadingScreenType;

    public List<LoadedAudioAsset> LoadAudioAssetReferences()
    {
        foreach (MusicTrack track in preloadMusicTracks)
        {
            loadedAudioAssets.Add( AudioController.Instance.PreloadAudioAsset(track));
        }

        foreach (SoundEffect soundEffect in preloadSoundEffects)
        {
            loadedAudioAssets.Add(AudioController.Instance.PreloadAudioAsset(soundEffect));
        }

        return loadedAudioAssets;
    }

    public void UnloadAudioAssetReferences()
    {
        foreach(LoadedAudioAsset asset in loadedAudioAssets)
        {
            AudioController.Instance.UnloadAsset(asset);
        }

        loadedAudioAssets.Clear();
    }
}
