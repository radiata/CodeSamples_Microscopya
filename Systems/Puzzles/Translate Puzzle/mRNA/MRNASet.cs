using Unity.Mathematics;
using UnityEngine;

public class MRNASet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer leftRenderer;
    [SerializeField] private SpriteRenderer centerRenderer;
    [SerializeField] private SpriteRenderer rightRenderer;
    [SerializeField] private SpriteRenderer overlayRenderer;
    public SpriteRenderer OverlayRenderer => overlayRenderer;

    [SerializeField] private int3 mRNASequence = new int3();

    [SerializeField] private Transform tRNA_Anchor;
    public Transform Anchor_tRNA => tRNA_Anchor;

    private Vector3 startLocalPosition;
    private Vector3 nextLocalPosition;
    public (Vector3, Vector3) GetLocalPositions => (startLocalPosition, nextLocalPosition);
    public void SetLocalPositions(Vector3 start, Vector3 next)
    {
        startLocalPosition = start;
        nextLocalPosition = next;
    }

    public void SetLeftSprite(Sprite sprite)
    {
        leftRenderer.sprite = sprite;
    }

    public void SetCenterSprite(Sprite sprite)
    {
        centerRenderer.sprite = sprite;
    }

    public void SetRightSprite(Sprite sprite)
    {
        rightRenderer.sprite = sprite;
    }

    public void ApplyMaterial(Material material)
    {
        leftRenderer.material = material;
        centerRenderer.material = material;
        rightRenderer.material = material;
    }

    public void SetMRNASequence(int firstType, int secondType, int thirdType)
    {
        mRNASequence.x = firstType;
        mRNASequence.y = secondType;
        mRNASequence.z = thirdType;
    }

    public TRNAType GetCorrespondingTRNAType()
    {
        int3 tRNA_int3 = new int3();
        tRNA_int3.x = MRNAMap.GetIntPair(mRNASequence.x);
        tRNA_int3.y = MRNAMap.GetIntPair(mRNASequence.y);
        tRNA_int3.z = MRNAMap.GetIntPair(mRNASequence.z);

        return MRNAMap.Int3ToTRNAType(tRNA_int3);
    }

    public bool isEquivalentSequence(int3 comparableSequence)
    {
        return math.all(mRNASequence == comparableSequence);
    }
}
