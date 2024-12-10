using UnityEngine;

public class ClearTrophyUnread : MonoBehaviour
{
    private void OnEnable()
    {
        TrophyUnlocked.NotifyTrophyRead();
    }
}
