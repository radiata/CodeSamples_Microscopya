using System.Collections.Generic;
using UnityEngine;

public class TRNA : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    [SerializeField] private TRNAType tRNAType;
    [SerializeField] private AminoAcidChain aminoAcidChain;

    [SerializeField] private AminoAcid attachedAminoAcid;

    [SerializeField] private FloatAwayAndFadeOut floatAndFadeAway;
    [SerializeField] private FloatInAndFadeIn floatAndFadeIn;
    public FloatInAndFadeIn FloatInAndFadeIn => floatAndFadeIn;

    [SerializeField] private Animator animator;
    [SerializeField] private string releaseAminoAcidTrigger = "ReleaseAminoAcidTrigger";
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DragAndDrop dragAndDrop;
    [SerializeField] private FreeFloat freeFloat;

    [SerializeField] private Base_NegativeFeedback negativeFeedback;

    private bool releasedAcid = false;
    private Transform nextTRNA;

    public TRNAType Type_tRNA => tRNAType;
    public Transform AminoAcidAnchor => attachedAminoAcid.transform.parent;
    public FreeFloat FreeFloat => freeFloat;

    public delegate void AnchorSetEvent();
    public event AnchorSetEvent OnAnchorSet;

    public void SetAnchor(Transform transform)
    {
        rootTransform.SetParent(transform);
        rootTransform.localPosition = Vector3.zero;
        rootTransform.localRotation = Quaternion.identity;
        rootTransform.localScale = Vector3.one;

        DisableInteractions();
        OnAnchorSet?.Invoke();
    }

    public void DisableInteractions()
    {
        gameObject.layer = LayerReferences.NonInteractableLayer;
    }

    public void EnableInteractions()
    {
        gameObject.layer = LayerReferences.InteractablePuzzleObjectsLayer;
    }

    public void ReleaseAminoAcid()
    {
        releasedAcid = true;
        aminoAcidChain.ProgressAminoAcidChain(attachedAminoAcid, nextTRNA);
    }

    public void CompleteReleaseAminoAcid()
    {
        aminoAcidChain.AminoAcidChainUpdated();
    }

    public void PlayReleaseAminoAcidAnimation(Transform nextTRNA)
    {
        this.nextTRNA = nextTRNA;
        animator.SetTrigger(releaseAminoAcidTrigger);
    }

    public void ReleaseTRNA(Transform newParent)
    {
        rootTransform.SetParent(newParent);
        floatAndFadeAway.StartBehaviour(!releasedAcid);
    }

    public void HideAminoAcid()
    {
        attachedAminoAcid.SpriteRenderer.enabled = false;
    }

    public void SetTRNAType(TRNAType type)
    {
        tRNAType = type;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetAminoAcidChain(AminoAcidChain chain)
    {
        aminoAcidChain = chain;
    }

    public void SetReceiver(List<GameObject> validReceivers)
    {
        dragAndDrop.AssignValidReceivers(validReceivers);
    }

    public void SetPuzzleManager(PuzzleManager puzzleManager)
    {
        dragAndDrop.AssignPuzzleManager(puzzleManager);
    }

    public void SetParent(Transform parent)
    {
        rootTransform.SetParent(parent);
        rootTransform.localPosition = Vector3.zero;
        rootTransform.localRotation = Quaternion.identity;
        rootTransform.localScale = Vector3.one;
    }

    private void OnEnable()
    {
        negativeFeedback.OnNegativeFeedbackStart += OnStartNegativeFeedback;
        negativeFeedback.OnNegativeFeedbackEnd += OnEndNegativeFeedback;
    }

    private void OnDisable()
    {
        negativeFeedback.OnNegativeFeedbackStart -= OnStartNegativeFeedback;
        negativeFeedback.OnNegativeFeedbackEnd -= OnEndNegativeFeedback;
    }

    private void OnStartNegativeFeedback()
    {
        DisableInteractions();
        freeFloat.enabled = false;
    }

    private void OnEndNegativeFeedback()
    {
        EnableInteractions();
        freeFloat.enabled = true;
    }
}
