using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "mRNA Settings", menuName = "Puzzles/Translation Puzzle/mRNA Settings")]
public class MRNA_SO : ScriptableObject
{
    [Tooltip("Must be an odd value")]
    [SerializeField] private int mRNAActiveSetsCount = 13;
    public int ActiveSetsCount => mRNAActiveSetsCount % 2 == 0 ? mRNAActiveSetsCount + 1 : mRNAActiveSetsCount;

    [SerializeField] private float mRNATotalWidth = 7.36f;
    [SerializeField] private float mRNASpacingAdjustment = -.086172f;
    [SerializeField] private float mRNAShadingAdjustment = 0f;

    [SerializeField] private GameObject template;
    [SerializeField] private GameObject mRNACap;
    [SerializeField] private GameObject mRNATail;

    [SerializeField] private Sprite left_A;
    [SerializeField] private Sprite left_C;
    [SerializeField] private Sprite left_G;
    [SerializeField] private Sprite left_U;

    [SerializeField] private Sprite center_A;
    [SerializeField] private Sprite center_C;
    [SerializeField] private Sprite center_G;
    [SerializeField] private Sprite center_U;

    [SerializeField] private Sprite right_A;
    [SerializeField] private Sprite right_C;
    [SerializeField] private Sprite right_G;
    [SerializeField] private Sprite right_U;

    public float mRNAWidth => mRNATotalWidth + mRNASpacingAdjustment;
    private float mRNAShadingWidth => mRNAWidth + mRNAShadingAdjustment;

    public MRNASet GenerateMRNAGameObject(int3 sequence, Transform parent)
    {
        var MRNAObject = Instantiate(template, parent);
        var set = MRNAObject.GetComponent<MRNASet>();

        set.SetMRNASequence(sequence.x, sequence.y, sequence.z);

        float overlayWidth = set.OverlayRenderer.sprite.bounds.size.x;
        overlayWidth = mRNAWidth / overlayWidth;
        set.OverlayRenderer.transform.localScale = new Vector3(1 * overlayWidth, 1, 1);

        switch (sequence.x)
        {
            case 0:
                set.SetLeftSprite(left_A);
                break;
            case 1:
                set.SetLeftSprite(left_C);
                break;
            case 2:
                set.SetLeftSprite(left_G);
                break;
            case 3:
                set.SetLeftSprite(left_U);
                break;
        }

        switch (sequence.y)
        {
            case 0:
                set.SetCenterSprite(center_A);
                break;
            case 1:
                set.SetCenterSprite(center_C);
                break;
            case 2:
                set.SetCenterSprite(center_G);
                break;
            case 3:
                set.SetCenterSprite(center_U);
                break;
        }

        switch (sequence.z)
        {
            case 0:
                set.SetRightSprite(right_A);
                break;
            case 1:
                set.SetRightSprite(right_C);
                break;
            case 2:
                set.SetRightSprite(right_G);
                break;
            case 3:
                set.SetRightSprite(right_U);
                break;
        }

        MRNAObject.name = $"mRNA - {MRNAMap.Int3ToString(sequence).ToUpper()}";
        return set;
    }

    public MRNASet GenerateEmptyMRNAGameObject(Transform parent)
    {
        var MRNAObject = Instantiate(template, parent);
        var set = MRNAObject.GetComponent<MRNASet>();

        float overlayWidth = set.OverlayRenderer.sprite.bounds.size.x;
        overlayWidth = mRNAWidth / overlayWidth;
        set.OverlayRenderer.transform.localScale = new Vector3(1 * overlayWidth, 1, 1);

        MRNAObject.name = $"mRNA - {"EMPTY".ToUpper()}";
        return set;
    }
}
