using UnityEngine;
using UnityEngine.UI;

public class JournalIndexPositionController : MonoBehaviour
{
    [SerializeField] private RectTransform journalIndexScrollView;

    [SerializeField] private Image journalImage;
    [SerializeField] private float journalImage_VerticalDeadSpace = 506;

    [Range(0f, 1f)]
    [SerializeField] private float journalImage_horizontalOffset;

    [ContextMenu("Set Journal Index Position")]
    private void SetJournalIndexPosition()
    {
        SetJournalIndexHeight();
        SetJournalIndexWidth();
    }

    private void Start()
    {
        SetJournalIndexPosition();
    }

    private void SetJournalIndexWidth()
    {
        float offsetAmount = GetJournalImageSize() * journalImage_horizontalOffset;

        Vector2 newOffsetMin = Vector2.zero;
        newOffsetMin.x = -offsetAmount;
        newOffsetMin.y = journalIndexScrollView.offsetMin.y;

        journalIndexScrollView.offsetMin = newOffsetMin;
    }

    private void SetJournalIndexHeight()
    {
        Vector2 newSize = Vector2.zero;
        newSize.x = journalIndexScrollView.sizeDelta.x;
        newSize.y = GetJournalImageAdjustedHeight();

        journalIndexScrollView.sizeDelta = newSize;
    }

    private float GetJournalImageAdjustedHeight()
    {
        float size = GetJournalImageSize();
        float proportionalDeadSpace = journalImage_VerticalDeadSpace / journalImage.sprite.texture.height;

        size = size - (size * proportionalDeadSpace);
        return size;
    }

    private float GetJournalImageSize()
    {
        Rect graphicRect = journalImage.GetPixelAdjustedRect();

        if (graphicRect.width <= graphicRect.height)
        {
            return graphicRect.width;
        }
        else
        {
            return graphicRect.height;
        }
    }
}
