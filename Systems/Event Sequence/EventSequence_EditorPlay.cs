#if UNITY_EDITOR
using System.Collections;
using UnityEngine;

public class EventSequence_EditorPlay : MonoBehaviour
{
    [SerializeField] private EventSequence eventSequence;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        eventSequence.DEBUG_RunSequence();
    }
}
#endif