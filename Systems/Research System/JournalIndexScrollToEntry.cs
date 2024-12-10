using System.Collections.Generic;
using UnityEngine;

public class JournalIndexScrollToEntry : MonoBehaviour
{
    [SerializeField] private RectTransform journalIndexContent;
    [SerializeField] private RectTransform journalIndexScrollView;

    [SerializeField] private List<GameObject> journalEntryObjects = new List<GameObject>();
    [SerializeField] private List<JournalEntry> journalEntries = new List<JournalEntry>();

    private List<GameObject> activeEntryObjects;

    public void ScrollToEntry(JournalEntry journalEntry)
    {
        int index = GetEntryIndex(journalEntry);
        if(index == -1)
        {
            return;
        }

        index = GetActiveObjectIndex(index);

        float entrySize = journalIndexContent.sizeDelta.y / activeEntryObjects.Count;
        float scrollMax = journalIndexContent.sizeDelta.y - journalIndexScrollView.sizeDelta.y;
        float scrollPosition = index * entrySize;
        scrollPosition = Mathf.Clamp(scrollPosition, 0, scrollMax);

        journalIndexContent.anchoredPosition = new Vector2(journalIndexContent.anchoredPosition.x, scrollPosition);
    }

    public int GetEntryIndex(JournalEntry journalEntry)
    {
        int index = -1;

        for (int i = 0; i < journalEntries.Count; i++)
        {
            if (journalEntries[i] == journalEntry)
            {
                index = i;
                break;
            }
        }

        if (index == -1
            || journalEntryObjects[index].activeSelf == false)
        {
            return -1;
        }

        return index;
    }

    private void Awake()
    {
        activeEntryObjects = new List<GameObject>(journalEntryObjects);

        for (int i = activeEntryObjects.Count - 1; i >= 0; i--)
        {
            if (activeEntryObjects[i].activeSelf == false)
            {
                activeEntryObjects.RemoveAt(i);
            }
        }
    }

    private int GetActiveObjectIndex(int entryIndex)
    {
        for (int i = 0; i < activeEntryObjects.Count; i++)
        {
            if (activeEntryObjects[i] == journalEntryObjects[entryIndex])
            {
                return i;
            }
        }

        return -1;
    }
}
