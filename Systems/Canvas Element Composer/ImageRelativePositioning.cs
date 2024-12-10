using UnityEngine;
using UnityEngine.UI;

public class ImageRelativePositioning : MonoBehaviour
{
    [Header("Image we are positioning on")]
    [SerializeField] private Image baseImage;
    [SerializeField] private RectTransform baseImage_RectTransform;

    [Header("Image Dead Space")]
    [SerializeField] private bool useImageDeadSpace = false;
    [SerializeField] private float imageTopDeadSpace;
    [SerializeField] private float imageBottomDeadSpace;
    [SerializeField] private float imageLeftDeadSpace;
    [SerializeField] private float imageRightDeadSpace;

    [Header("Object to position")]
    [SerializeField] private RectTransform controlledRectTransform;
    [Range(0f, 1f)]
    [SerializeField] private float horizontalAnchorMin;
    [Range(0f, 1f)]
    [SerializeField] private float horizontalAnchorMax;
    [Range(0f, 1f)]
    [SerializeField] private float verticalAnchorMin;
    [Range(0f, 1f)]
    [SerializeField] private float verticalAnchorMax;

    [Header("Image Alignment")]
    [SerializeField] private HorizontalAlignmentMode horizontalAlignment = HorizontalAlignmentMode.Center;
    [SerializeField] private VerticalAlignmentMode verticalAlignment = VerticalAlignmentMode.Center;

    [ContextMenu("Update Position")]
    public void UpdatePosition()
    {
        Vector2 dimensions = useImageDeadSpace == true
            ? GetDeadSpaceAdjustedImageDimensions() : GetRawImageDimensions();

        controlledRectTransform.anchorMin = GetAnchorMin(dimensions);
        controlledRectTransform.anchorMax = GetAnchorMax(dimensions);
    }

    private Vector2 GetRawImageDimensions()
    {
        Rect imageRect = baseImage_RectTransform.rect;
        Vector2 imageDimensions = imageRect.size;

        if (baseImage.preserveAspect == false)
        {
            return imageDimensions;
        }

        float imageAspectRatio = baseImage.sprite.texture.width / baseImage.sprite.texture.height;


        if (imageRect.width / imageAspectRatio > imageRect.height)
        {
            //we are height restricted/bound
            imageDimensions.x = imageRect.height * imageAspectRatio;
        }
        else
        {
            //we are width restricted/bound
            imageDimensions.y = imageRect.width / imageAspectRatio;
        }

        return imageDimensions;
    }

    private Vector2 GetDeadSpaceAdjustedImageDimensions()
    {
        Vector2 imageDimensions = GetRawImageDimensions();

        float normalizedTop = imageTopDeadSpace / baseImage.sprite.texture.height;
        float normalizedBottom = imageBottomDeadSpace / baseImage.sprite.texture.height;
        float normalizedLeft = imageLeftDeadSpace / baseImage.sprite.texture.width;
        float normalizedRight = imageRightDeadSpace / baseImage.sprite.texture.width;

        imageDimensions.x = imageDimensions.x - (imageDimensions.x * normalizedTop) - (imageDimensions.x * normalizedBottom);
        imageDimensions.y = imageDimensions.y - (imageDimensions.y * normalizedLeft) - (imageDimensions.y * normalizedRight);

        return imageDimensions;
    }

    private Vector4 GetAnchorPositions(Vector2 dimensions)
    {
        float anchorMinX = 0f;
        float anchorMaxX = 0f;
        float anchorMinY = 0f;
        float anchorMaxY = 0f;

        float normalizedHalfExcessWidth = ((baseImage_RectTransform.rect.width - dimensions.x) * .5f) / baseImage_RectTransform.rect.width;
        float normalizedHalfExcessHeight = ((baseImage_RectTransform.rect.height - dimensions.y) * .5f) / baseImage_RectTransform.rect.height;

        anchorMinX = ((horizontalAnchorMin * dimensions.x) / baseImage_RectTransform.rect.width) + normalizedHalfExcessWidth;
        anchorMinY = ((verticalAnchorMin * dimensions.y) / baseImage_RectTransform.rect.height) + normalizedHalfExcessHeight;
        anchorMaxX = ((horizontalAnchorMax * dimensions.x) / baseImage_RectTransform.rect.width) + normalizedHalfExcessWidth;
        anchorMaxY = ((verticalAnchorMax * dimensions.y) / baseImage_RectTransform.rect.height) + normalizedHalfExcessHeight;

        switch (verticalAlignment)
        {
            case VerticalAlignmentMode.Top:
                anchorMinY += normalizedHalfExcessHeight;
                anchorMaxY += normalizedHalfExcessHeight;
                break;
            case VerticalAlignmentMode.Bottom:
                anchorMinY -= normalizedHalfExcessHeight;
                anchorMaxY -= normalizedHalfExcessHeight;
                break;
        }

        switch (horizontalAlignment)
        {
            case HorizontalAlignmentMode.Left:
                anchorMinX -= normalizedHalfExcessWidth;
                anchorMaxX -= normalizedHalfExcessWidth;
                break;
            case HorizontalAlignmentMode.Right:
                anchorMinX += normalizedHalfExcessWidth;
                anchorMaxX += normalizedHalfExcessWidth;
                break;
        }

        return new Vector4(anchorMinX, anchorMaxX, anchorMinY, anchorMaxY);
    }

    private Vector2 GetAnchorMin(Vector2 dimensions)
    {
        float anchorMinX = 0f;
        float anchorMinY = 0f;

        float normalizedHalfExcessWidth = ((baseImage_RectTransform.rect.width - dimensions.x) * .5f) / baseImage_RectTransform.rect.width;
        float normalizedHalfExcessHeight = ((baseImage_RectTransform.rect.height - dimensions.y) * .5f) / baseImage_RectTransform.rect.height;

        anchorMinX = ((horizontalAnchorMin * dimensions.x) / baseImage_RectTransform.rect.width) + normalizedHalfExcessWidth;
        anchorMinY = ((verticalAnchorMin * dimensions.y) / baseImage_RectTransform.rect.height) + normalizedHalfExcessHeight;

        switch (verticalAlignment)
        {
            case VerticalAlignmentMode.Top:
                anchorMinY += normalizedHalfExcessHeight;
                break;
            case VerticalAlignmentMode.Bottom:
                anchorMinY -= normalizedHalfExcessHeight;
                break;
        }

        switch (horizontalAlignment)
        {
            case HorizontalAlignmentMode.Left:
                anchorMinX -= normalizedHalfExcessWidth;
                break;
            case HorizontalAlignmentMode.Right:
                anchorMinX += normalizedHalfExcessWidth;
                break;
        }

        return new Vector2(anchorMinX, anchorMinY);
    }

    private Vector2 GetAnchorMax(Vector2 dimensions)
    {
        float anchorMaxX = 0f;
        float anchorMaxY = 0f;

        float normalizedHalfExcessWidth = ((baseImage_RectTransform.rect.width - dimensions.x) * .5f) / baseImage_RectTransform.rect.width;
        float normalizedHalfExcessHeight = ((baseImage_RectTransform.rect.height - dimensions.y) * .5f) / baseImage_RectTransform.rect.height;

        anchorMaxX = ((horizontalAnchorMax * dimensions.x) / baseImage_RectTransform.rect.width) + normalizedHalfExcessWidth;
        anchorMaxY = ((verticalAnchorMax * dimensions.y) / baseImage_RectTransform.rect.height) + normalizedHalfExcessHeight;

        switch (verticalAlignment)
        {
            case VerticalAlignmentMode.Top:
                anchorMaxY += normalizedHalfExcessHeight;
                break;
            case VerticalAlignmentMode.Bottom:
                anchorMaxY -= normalizedHalfExcessHeight;
                break;
        }

        switch (horizontalAlignment)
        {
            case HorizontalAlignmentMode.Left:
                anchorMaxX -= normalizedHalfExcessWidth;
                break;
            case HorizontalAlignmentMode.Right:
                anchorMaxX += normalizedHalfExcessWidth;
                break;
        }

        return new Vector4(anchorMaxX, anchorMaxY);
    }

    [ContextMenu("Copy Anchors")]
    private void CopyControlledRectAnchors()
    {
        horizontalAnchorMin = controlledRectTransform.anchorMin.x;
        horizontalAnchorMax = controlledRectTransform.anchorMax.x;
        verticalAnchorMin = controlledRectTransform.anchorMin.y;
        verticalAnchorMax = controlledRectTransform.anchorMax.y;
    }
}

[System.Serializable]
public enum HorizontalAlignmentMode
{
    Center = 0,
    Left = 1,
    Right = 2,
}

[System.Serializable]
public enum VerticalAlignmentMode
{
    Center = 0,
    Top = 1,
    Bottom = 2,
}