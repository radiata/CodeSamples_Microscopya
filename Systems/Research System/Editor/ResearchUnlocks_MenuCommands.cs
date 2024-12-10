using UnityEditor;
using UnityEngine;

public class ResearchUnlocks_MenuCommands
{
    [MenuItem("Tools/Saved Data/Research Journal/Unlock All Entries")]
    private static void UnlockResearchAllEntries()
    {
        ResearchUnlocks.UnlockAllEntries();
    }

    [MenuItem("Tools/Saved Data/Research Journal/Lock All Entries")]
    private static void LockResearchAllEntries()
    {
        ResearchUnlocks.LockAllEntries();
    }
}
