using UnityEngine;

public class InputHandler_StartUp : Base_StartUp
{
    [SerializeField] private InputHandler_Settings_SO loadedSettings;
 
    private static InputHandler activeInputHandler;

    public override void FinalizeProcess()
    {
        activeInputHandler.enabled = true;
    }

    protected override void RunProcess()
    {
        CreateActiveInputManager();
    }

    protected override bool CheckProcessComplete()
    {
        activeInputHandler.SetInputManagerSettings(loadedSettings);
        return true;
    }

    private static void CreateActiveInputManager()
    {
        if (activeInputHandler != null)
        {
            return;
        }

        activeInputHandler = new GameObject("Input Manager").AddComponent<InputHandler>();
        activeInputHandler.enabled = false;
        DontDestroyOnLoad(activeInputHandler);
    }
}
