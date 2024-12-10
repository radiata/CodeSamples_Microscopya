public class LanguageSetting_StartUp : Base_StartUp
{
    public override void FinalizeProcess()
    {
    }

    protected override bool CheckProcessComplete()
    {
        if (LanguageSetting.CurrentLanguage == Languages.Uninitialized)
        {
            return false;
        }

        return true;
    }

    protected override void RunProcess()
    {
        LanguageSetting.LoadLanguageSetting();
    }
}
