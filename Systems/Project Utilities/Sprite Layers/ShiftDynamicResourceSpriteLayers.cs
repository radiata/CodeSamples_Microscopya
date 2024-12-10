using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShiftDynamicResourceSpriteLayers : MonoBehaviour
{
    [SerializeField] private int shiftBy = 50;
    [SerializeField] private List<TextureObject> textureObjects;

    [SerializeField] private List<TextureObject_LayerBucket> layerBuckets;
    
    [ContextMenu("Populate Texture Objects")]
    private void PopulateTextureObjects()
    {
        TextureObject[] gameObjects = FindObjectsByType<TextureObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        textureObjects = gameObjects.ToList();
    }

    [ContextMenu("Sort Objects into Buckets")]
    private void SortIntoLayerBuckets()
    {
        foreach (TextureObject textureObject in textureObjects)
        {
            SpriteRenderer spriteRenderer = textureObject.ReturnSpriteRenderer();
            TextureObject_LayerBucket bucket = SearchForBucket(spriteRenderer.sortingOrder);
            bucket.LayerBucket.Add(textureObject);
        }
    }

    private TextureObject_LayerBucket SearchForBucket(int targetLayer)
    {
        for (int i = 0; i < layerBuckets.Count; i++)
        {
            if (layerBuckets[i].Layer == targetLayer)
            {
                return layerBuckets[i];
            }
        }

        TextureObject_LayerBucket newBucket = new TextureObject_LayerBucket(targetLayer, new List<TextureObject>());
        layerBuckets.Add(newBucket);
        return layerBuckets.Last();
    }

    [ContextMenu("Sort Buckets by Layer")]
    private void SortBucketsByLayer()
    {
        layerBuckets = layerBuckets.OrderBy(bucket => bucket.Layer).ToList();
    }
}

[System.Serializable]
public struct TextureObject_LayerBucket
{
    public int Layer;
    public List<TextureObject> LayerBucket;

    public TextureObject_LayerBucket(int layer, List<TextureObject> layerBucket)
    {
        Layer = layer;
        LayerBucket = layerBucket;
    }
}
