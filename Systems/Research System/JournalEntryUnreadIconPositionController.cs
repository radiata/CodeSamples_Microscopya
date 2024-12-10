using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryUnreadIconPositionController : MonoBehaviour
{
    [SerializeField] private Image referenceLabelImage;
    [SerializeField] private float referenceLabelImage_HorizontalDeadSpace;
    [SerializeField] private float referenceLabelImage_VerticalDeadSpace;

    [Range(0f, 1f)]
    [SerializeField] private float horizontalOffset;
    [Range(0f, 1f)]
    [SerializeField] private float verticalOffset;

    [SerializeField] private List<RectTransform> unreadImages;

    private IEnumerator Start()
    {
        yield return null;
        SetUnreadImagesPositions();
    }

    [ContextMenu("Set Unread Image Positions")]
    private void SetUnreadImagesPositions()
    {
        foreach (RectTransform unreadImage in unreadImages)
        {
            SetUnreadImagePosition(unreadImage);
        }
    }

    private void SetUnreadImagePosition(RectTransform unreadImage)
    {
        //we get the rect transform (graphicRect is just the rect transform size, it does not account for image resizing due to preserve aspect)
        Rect graphicRect = referenceLabelImage.GetPixelAdjustedRect();

        //then we need to determine what portion of it the image is actually taking up
        //-we are using preserve aspect, so it will be the largest something that fits in the space at 4:1 (ref image is 2048x512)
        float imageAspectRatio = referenceLabelImage.sprite.texture.width / referenceLabelImage.sprite.texture.height;

        Vector2 dimensions = Vector2.zero;
        if (graphicRect.width / imageAspectRatio > graphicRect.height)
        {
            //we are height restricted/bound
            dimensions.x = graphicRect.height * imageAspectRatio;
            dimensions.y = graphicRect.height;
        }
        else
        {
            //we are width restricted/bound
            dimensions.x = graphicRect.width;
            dimensions.y = graphicRect.width / imageAspectRatio;
        }

        //once we find the space the image takes up, reduce the viable area by the proportion of dead space
        float normalHeightDeadSpace = referenceLabelImage_VerticalDeadSpace / referenceLabelImage.sprite.texture.height;
        float normalWidthDeadSpace = referenceLabelImage_HorizontalDeadSpace / referenceLabelImage.sprite.texture.width;

        dimensions.x = dimensions.x - (dimensions.x * normalWidthDeadSpace);
        dimensions.y = dimensions.y - (dimensions.y * normalHeightDeadSpace);

        //within the final area, set anchor position of the image to size * offset
        //-somehow relative to the position of the pivot point?
        //negative pivot?
        Vector2 finalPosition = Vector2.zero;
        finalPosition.x = (dimensions.x * horizontalOffset) - (dimensions.x * unreadImage.pivot.x);
        finalPosition.y = (dimensions.y * verticalOffset) - (dimensions.y * unreadImage.pivot.y);

        //change the anchors to pivot point
        Vector2 cachedSize = unreadImage.rect.size;

        unreadImage.anchorMin = unreadImage.pivot;
        unreadImage.anchorMax = unreadImage.pivot;
        //set position
        unreadImage.anchoredPosition = finalPosition;
        unreadImage.sizeDelta = cachedSize;

        //profit?

        //we aren't properly accounting for anchors. That will take a lot more work that isn't entirely necessary...
    }
}
