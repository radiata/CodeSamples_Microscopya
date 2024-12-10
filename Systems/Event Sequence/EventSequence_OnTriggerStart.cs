using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EventSequence_OnTriggerStart : MonoBehaviour
{
    [SerializeField] private string collisionTag = "mainCharacter";
    [SerializeField] private EventSequence eventSequence;

    [SerializeField] private bool singleTimeEvent = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(collisionTag) == true)
        {
            eventSequence.StartOnCallSequence();
            if (singleTimeEvent == true)
            {
                Destroy(gameObject);
            }
        }
    }
}
