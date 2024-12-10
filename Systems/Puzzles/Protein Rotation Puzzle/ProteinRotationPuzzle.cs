using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProteinRotationPuzzle : BasePuzzle
{
    [SerializeField] private GameObject puzzleRootObject;
    [SerializeField] private GameObject proteinRotator;
    [SerializeField] private List<ProteinLink> proteinLinks;

    [Header("On Puzzle Complete")]
    [SerializeField] private GameObject guidedProteins;

    [Header("Materials")]
    [SerializeField] private Material partialLinkMaterial;
    [SerializeField] private Material inactiveLinkMaterial;
    [SerializeField] private Material activeLinkMaterial;
    [SerializeField] private Material solvedLinkMaterial;

    [Header("Outline Modifiers")]
    [SerializeField] private Gradient activeOutlineGradient;

    [Header("Puzzle Settings")]
    [SerializeField] private int inactiveSegments = 2;
    [SerializeField] private int partiallyHiddenSegments = 1;

    private int jointIndex = -1;

    public override void ActivatePuzzle()
    {
        proteinRotator.SetActive(true);
    }

    public override void DeactivatePuzzle()
    {
        proteinRotator.SetActive(false);
    }

    public override void InitializePuzzle_Awake()
    {
        proteinRotator.SetActive(false);

        foreach (ProteinLink proteinLink in proteinLinks)
        {
            proteinLink.SetOutlineGradient(activeOutlineGradient);
        }
    }
    public override void InitializePuzzle_Start()
    { }

    public void Start()
    {
        for (int i = 0; i < proteinLinks.Count; i++)
        {
            proteinLinks[i].AssignMaterials(partialLinkMaterial, inactiveLinkMaterial, activeLinkMaterial, solvedLinkMaterial);

            if (i + 1 < proteinLinks.Count)
            {
                proteinLinks[i].Initialize(this, proteinLinks[i + 1]);
            }
            else
            {
                proteinLinks[i].Initialize(this, null);
            }
        }

        jointIndex = -1;
        ActivateNextIndex();
    }

    public void SetProteinLinkList(List<ProteinLink> proteinLinks)
    {
        this.proteinLinks = proteinLinks;
    }

    public void SetVariables(PuzzleKey puzzleKey, PuzzleManager_PointerInteractable pointerInteractable
        , PuzzleManager puzzleManager, GameObject puzzleRootObject
        , Material partialLinkMaterial, Material inactiveLinkMaterial, Material activeLinkMaterial, Material solvedLinkMaterial
        , Gradient activeOutlineGradient, GameObject proteinRotator)
    {
        base.puzzleKey = puzzleKey;
        base.puzzleManager_PointerInteractable_GameObject = pointerInteractable.gameObject;
        base.puzzleManager = puzzleManager;
        this.puzzleRootObject = puzzleRootObject;
        this.partialLinkMaterial = partialLinkMaterial;
        this.inactiveLinkMaterial = inactiveLinkMaterial;
        this.activeLinkMaterial = activeLinkMaterial;
        this.solvedLinkMaterial = solvedLinkMaterial;
        this.activeOutlineGradient = activeOutlineGradient;
        this.proteinRotator = proteinRotator;
    }

    public void SetToViewingAngle(ProteinLink proteinLink)
    {
        puzzleRootObject.transform.rotation = Quaternion.Euler(proteinLink.ViewingAngle);
    }

    public void ResetViewingAngleToZero()
    {
        puzzleRootObject.transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        for (int i = 0; i < proteinLinks.Count && jointIndex < proteinLinks.Count; i++)
        {
            proteinLinks[jointIndex].OnProteinLinkSolved -= ActivateNextIndex;
        }

        StopAllCoroutines();
        CancelInvoke();
    }

    private void ActivateNextIndex()
    {
        if (jointIndex >= 0)
        {
            proteinLinks[jointIndex].OnProteinLinkSolved -= ActivateNextIndex;
            proteinLinks[jointIndex].Deactivate();
        }

        jointIndex++;

        if (jointIndex >= proteinLinks.Count)
        {
            PuzzleComplete();
            return;
        }

        for (int i = jointIndex + 1; i < proteinLinks.Count && i <= jointIndex + inactiveSegments; i++)
        {
            proteinLinks[i].RevealInactive();
        }

        for (int i = jointIndex + inactiveSegments + 1; i < proteinLinks.Count && i <= jointIndex + inactiveSegments + partiallyHiddenSegments; i++)
        {
            proteinLinks[i].RevealPartial();
        }

        for (int i = jointIndex + inactiveSegments + partiallyHiddenSegments + 1; i < proteinLinks.Count; i++)
        {
            proteinLinks[i].Hide();
        }

        StartCoroutine(RotateToViewingAngle(proteinLinks[jointIndex], true));
    }

    private void PuzzleComplete()
    {
        ObjectiveUpdaterEvents.ObjectiveCompleted(ObjectiveID_ER.Rotation_Solved);

        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        guidedProteins.SetActive(true);
    }

    [ContextMenu("Reset Viewing Angle")]
    public void ResetViewingAngle(float? overrideTime = null)
    {
        proteinLinks[jointIndex].ColliderSet.SetActiveState(false);
        StartCoroutine(RotateToViewingAngle(proteinLinks[jointIndex], false, true, overrideTime));
    }

    IEnumerator RotateToViewingAngle(ProteinLink targetProteinLink, bool setLinkActive = true
        , bool overrideCollider = false, float? overrideTime = null)
    {
        float currentTime = 0;
        float targetTime = overrideTime == null ? targetProteinLink.RotationTimeToViewingAngle : overrideTime.Value;

        Quaternion startRotation = puzzleRootObject.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetProteinLink.ViewingAngle);

        while (currentTime < targetTime)
        {
            puzzleRootObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, currentTime / targetTime);
            currentTime += Time.deltaTime;

            yield return null;
        }

        puzzleRootObject.transform.rotation = targetRotation;

        if (setLinkActive)
        {
            proteinLinks[jointIndex].Activate();
            proteinLinks[jointIndex].OnProteinLinkSolved += ActivateNextIndex;
        }

        if (overrideCollider)
        {
            targetProteinLink.ColliderSet.SetActiveState(true);
        }
    }
}
