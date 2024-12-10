using UnityEngine;

[CreateAssetMenu(fileName = "Research Journal Settings Data", menuName = "Research System/Research Journal/Research Journal Settings")]
public class ResearchJournalSettings_SO : ScriptableObject
{
    [SerializeField] private Sprite emptyButtonSprite;
    [SerializeField] private Sprite lockedButtonSprite;

    [SerializeField] private SoundEffect onClick_UnlockedSound;
    [SerializeField] private SoundEffect onClick_LockedSound;

    public Sprite EmptyButtonSprite => emptyButtonSprite;

    public Sprite LockedButtonSprite => lockedButtonSprite;

    public SoundEffect OnClickSound(bool isUnlocked) => isUnlocked ? onClick_UnlockedSound : onClick_LockedSound;
}
