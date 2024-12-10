using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPageContent : MonoBehaviour
{
    [SerializeField] private Image journalPageImageRenderer;
    [SerializeField] private string journalPageResourcesPath;
    [SerializeField] private List<ImageRelativePositioning> imageRelativeComponents;

    private void Start()
    {
        LoadResourceImage();
        UpdateImageRelativePositions();
    }

    private void LoadResourceImage()
    {
        journalPageImageRenderer.sprite = Resources.Load<Sprite>(journalPageResourcesPath);
    }

    private void UpdateImageRelativePositions()
    {
        foreach (ImageRelativePositioning component in imageRelativeComponents)
        {
            component.UpdatePosition();
        }
    }
}
