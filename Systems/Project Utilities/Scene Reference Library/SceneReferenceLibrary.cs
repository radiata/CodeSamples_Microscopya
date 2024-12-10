public static class SceneReferenceLibrary
{
    #region Chapter 1 Scenes
    public static readonly SceneInformation OpeningScene = new SceneInformation()
    {
        SceneName = "Opening Scene"
    };

    public static readonly SceneInformation CytoplasmScene = new SceneInformation()
    {
        SceneName = "Cytoplasm Scene"
    };

    public static readonly SceneInformation MitochondriaScene = new SceneInformation()
    {
        SceneName = "Mitochondria Scene"
    };

    public static readonly SceneInformation MotorScene = new SceneInformation()
    {
        SceneName = "Motor Scene"
    };
    #endregion

    #region Settings and Menu Scenes
    public static readonly SceneInformation AboutScene = new SceneInformation()
    {
        SceneName = "About"
    }; 

    public static readonly SceneInformation ContactScene = new SceneInformation()
    {
        SceneName = "Contact"
    }; 

    public static readonly SceneInformation MainMenuScene = new SceneInformation()
    {
        SceneName = "MainMenu"
    }; 

    public static readonly SceneInformation TrophyScene = new SceneInformation()
    {
        SceneName = "Trophy Scene"
    };
    #endregion

    #region Utility Scenes
    public static readonly SceneInformation DestroyAllScene = new SceneInformation()
    {
        SceneName = "DestroyAll"
    };

    public static readonly SceneInformation StartUpScene = new SceneInformation()
    {
        SceneName = "StartUp Scene"
    };
    #endregion
}
