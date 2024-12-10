using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System;

public class ProteinLink : MonoBehaviour, I_DraggablePuzzlePiece
{
    public bool DEBUG_DisableRig = false;

    [SerializeField] private PuzzleManager puzzleManager;

    [SerializeField] private Vector3 rotationAxis_CameraRelative;
    [SerializeField] private GameObject rotationAxis;
    [SerializeField] private GameObject rotationTarget;

    [SerializeField] private GameObject rootObject;

    [SerializeField] private Rig rotationRig;
    [SerializeField] private ChainIKConstraint[] chainIKConstraints;
    [SerializeField] private OverrideTransform overrideTransform;

    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers;

    [SerializeField] private ProteinLink_ColliderSet colliderSet;

    [SerializeField] private GameObject echoObject;

    [Range(0f, 1f)]
    [SerializeField] private float snappingDistance = .1f;
    [Range(0f, 1f)]
    [SerializeField] private float overrideWeightScalar = .5f;
    [Range(0f, 1f)]
    [SerializeField] private float chainWeightScalar = .5f;


    [SerializeField] private Vector3 viewingAngle;
    [SerializeField] private float rotationTimeToViewingAngle = 1f;

    [SerializeField] private Quaternion solvedRotation;

    [SerializeField] private Material outlineMaterial;

    private Gradient activeOutlineGradient;
    private Color currentColor;

    private Material partialMaterial;
    private Material inactiveMaterial;
    private Material activeMaterial;
    private Material solvedMaterial;
    private float dragOffsetAngle;
    private Quaternion dragStartRotation;

    [SerializeField] private float startAngle = 180f;

    public delegate void ProteinLinkSolved();
    public event ProteinLinkSolved OnProteinLinkSolved;

    [SerializeField] private GameObject rootBone;
    [SerializeField] private Quaternion rootBone_StartLocalRotation;

    private Transform solutionTransform_Current;
    private Transform solutionTransform_Solved;
    private float IKChainTarget_maxDistance;
    private ProteinLink nextProteinLink;

    [SerializeField] private SoundEffect correctPlacement_Sound = SoundEffect.TubulinDelivered;

    public Vector3 ViewingAngle => viewingAngle;
    public float RotationTimeToViewingAngle => rotationTimeToViewingAngle;
    public List<SkinnedMeshRenderer> SkinnedMeshRenderers => skinnedMeshRenderers;
    public ProteinLink_ColliderSet ColliderSet => colliderSet;

    #region Setters
    public void SetColliderSet(ProteinLink_ColliderSet colliderSet)
    {
        this.colliderSet = colliderSet;
    }

    public void SetSkinnedMeshRenderer(List<SkinnedMeshRenderer> skinnedMeshRenderers)
    {
        this.skinnedMeshRenderers = skinnedMeshRenderers;
    }

    public void SetOutlineGradient(Gradient activeOutlineGradient)
    {
        this.activeOutlineGradient = activeOutlineGradient;
    }

    public void SetViewingData(Vector3 viewingAngle, float rotationTime)
    {
        this.viewingAngle = viewingAngle;
        rotationTimeToViewingAngle = rotationTime;
    }

    public void SetSolvedRotation(Quaternion solvedRotation)
    {
        this.solvedRotation = solvedRotation;
    }

    public void SetRotationRig(Rig rig, ChainIKConstraint[] chainIKConstraint, OverrideTransform overrideTransform)
    {
        rotationRig = rig;
        this.chainIKConstraints = chainIKConstraint;
        this.overrideTransform = overrideTransform;
    }

    public void SetBone(GameObject bone)
    {
        rootBone = bone;
    }

    public void SetOverrideTransformRotation(int linkIndex)
    {
        CreateRotationAxis(linkIndex);
        overrideTransform.data.sourceObject = rotationTarget.transform;
    }

    public void SetRigDefaultValues()
    {
        foreach (ChainIKConstraint chainIKConstraint in chainIKConstraints)
        {
            chainIKConstraint.data.chainRotationWeight = 1;
            chainIKConstraint.data.tipRotationWeight = 0;
            chainIKConstraint.data.maxIterations = 50;
            chainIKConstraint.data.tolerance = 0;
        }

        overrideTransform.data.space = OverrideTransformData.Space.Pivot;
        overrideTransform.data.positionWeight = 0;
        overrideTransform.data.rotationWeight = 1;
    }

    public void SetStartAngle(float startAngle)
    {
        this.startAngle = startAngle;
    }
    
    public void SetEchoObject(GameObject echoObject)
    {
        this.echoObject = echoObject;
    }
    #endregion

    public void Initialize(ProteinRotationPuzzle proteinRotationPuzzle, ProteinLink nextProteinLink)
    {
        this.nextProteinLink = nextProteinLink;

        solutionTransform_Current = chainIKConstraints[chainIKConstraints.Length - 1].data.target;
        solutionTransform_Solved =
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(
                chainIKConstraints[chainIKConstraints.Length - 1].data.target,
                chainIKConstraints[chainIKConstraints.Length - 1].data.target.gameObject.name
                ).transform;
        solutionTransform_Solved.name += " Solution";
        solutionTransform_Solved.parent = rootObject.transform.parent;
        solutionTransform_Solved.localScale = Vector3.one;

        IKChainTarget_maxDistance = Vector3.Distance(solutionTransform_Solved.position, rootObject.transform.position) * 2;


        rotationTarget.transform.localRotation = Quaternion.Euler(rotationAxis_CameraRelative * startAngle);

        proteinRotationPuzzle.SetToViewingAngle(this);

        ResetToStartRotation();

        proteinRotationPuzzle.ResetViewingAngleToZero();

        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.material = inactiveMaterial;
        }
    }

    public void AssignMaterials(Material partialMaterial, Material inactiveMaterial, Material activeMaterial, Material solvedMaterial)
    {
        this.partialMaterial = partialMaterial;
        this.inactiveMaterial = inactiveMaterial;
        this.activeMaterial = activeMaterial;
        this.solvedMaterial = solvedMaterial;
    }

    public void Activate()
    {
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = true;
            //renderer.material = activeMaterial;
            renderer.materials = new Material[2] { activeMaterial, outlineMaterial };
        }

        overrideTransform.weight = 0f;

        foreach (ChainIKConstraint chainIKConstraint in chainIKConstraints)
        {
            chainIKConstraint.weight = 1f;
        }

        colliderSet.SetActiveState(true);

        HandleDistanceBlend_Rigs(nextProteinLink);
        HandleDistanceBlend_Material();

        if (echoObject != null)
        {
            echoObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        rotationRig.weight = 0f;

        colliderSet.SetActiveState(false);

        if (echoObject != null)
        {
            echoObject?.SetActive(false);
        }

        if (solutionTransform_Solved != null)
        {
            Destroy(solutionTransform_Solved.gameObject);
        }
    }

    public void RevealInactive()
    {
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = true;
            renderer.material = inactiveMaterial;
        }
    }

    public void RevealPartial()
    {
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = true;
            renderer.material = partialMaterial;
        }
    }
    public void Hide()
    {
        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            renderer.enabled = false;
            renderer.material = null;
        }
    }

    public void OnDragStart(Vector3 worldPosition)
    {
        dragOffsetAngle = FindMouseAngle(worldPosition);

        dragStartRotation = rootObject.transform.rotation;
        puzzleManager.Navigate();
    }

    public void WhileDragging(Vector3 worldPosition, Vector3 cameraForward)
    {
        var angle = FindMouseAngle(worldPosition) - dragOffsetAngle;
        HandleRotation(angle);
        HandleDistanceBlend_Rigs(nextProteinLink);
        HandleDistanceBlend_Material();
    }

    public void OnDragEnd(Vector3 worldPosition)
    {
        HandleSolution();
    }

    private void Awake()
    {
        foreach (ChainIKConstraint chainIKConstraint in chainIKConstraints)
        {
            chainIKConstraint.weight = 0f;
        }
        overrideTransform.weight = 1f;

        rotationRig.weight = 1f;

        if (DEBUG_DisableRig == true)
        {
            rotationRig.weight = 0f;
        }

        echoObject.SetActive(false);
    }

    private float FindMouseAngle(Vector3 worldPosition)
    {
        Vector3 pivotPosition = rootObject.transform.position;
        Vector3 mousePosition = worldPosition;

        var rPosition = new Vector2(mousePosition.x - pivotPosition.x, mousePosition.y - pivotPosition.y);

        var angle = Vector2.SignedAngle(Vector2.up, rPosition);
        return angle;
    }

    private void HandleRotation(float rotationAngle)
    {
        rootObject.transform.rotation = Quaternion.AngleAxis(rotationAngle, rotationAxis_CameraRelative) * dragStartRotation;
    }

    private void HandleDistanceBlend_Rigs(ProteinLink nextProteinLink)
    {
        if (nextProteinLink != null)
        {
            float distance = Vector3.Distance(solutionTransform_Solved.position, solutionTransform_Current.transform.position);
            float percentageDistance = Mathf.Clamp01(distance / IKChainTarget_maxDistance);

            float overrideWeight;
            float chainWeight;

            float xMin = 0;
            float xMax = 1;

            xMin = overrideWeightScalar;
            xMax = 1;
            overrideWeight = (percentageDistance - xMin) / (xMax - xMin);

            xMin = 0;
            xMax = chainWeightScalar;
            chainWeight = 1 - ((percentageDistance - xMin) / (xMax - xMin));

            overrideWeight = Mathf.Clamp01(overrideWeight);
            chainWeight = Mathf.Clamp01(chainWeight);

            nextProteinLink.overrideTransform.weight = overrideWeight;

            foreach (ChainIKConstraint chainIKConstraint in nextProteinLink.chainIKConstraints)
            {
                chainIKConstraint.weight = chainWeight;
            }
        }
    }

    private void HandleDistanceBlend_Material()
    {
        float distance = Vector3.Distance(solutionTransform_Solved.position, solutionTransform_Current.transform.position);
        float percentageDistance = Mathf.Clamp01(distance / IKChainTarget_maxDistance);
        
        if(float.IsNaN(percentageDistance))
        {
            Debug.LogWarning("Percentage Distance was NaN");
            percentageDistance = 1;
        }

        Color newColor = activeOutlineGradient.Evaluate(percentageDistance);

        if(currentColor != newColor)
        {
            foreach(SkinnedMeshRenderer skinnedMeshRenderer in SkinnedMeshRenderers)
            {
                skinnedMeshRenderer.materials[1].SetColor("_OutlineColor", newColor);
            }
            //activeMaterial.SetColor("_OutlineColor", newColor);
            currentColor = newColor;
        }
    }

    private bool HandleSolution()
    {
        var distance = Vector3.Distance(solutionTransform_Solved.position, solutionTransform_Current.transform.position);

        if (distance / IKChainTarget_maxDistance <= snappingDistance)
        {
            rootObject.transform.localRotation = solvedRotation;

            Color newColor = activeOutlineGradient.Evaluate(1);

            foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
            {
                renderer.materials[0] = solvedMaterial;
                renderer.materials[1].SetColor("_OutlineColor", newColor);
            }

            OnProteinLinkSolved?.Invoke();
            AudioController.Instance.PlaySoundEffect(correctPlacement_Sound, false);

            return true;
        }

        return false;
    }

    [ContextMenu("Reset To Start Rotation")]
    private void ResetToStartRotation()
    {
        rootObject.transform.localRotation = solvedRotation;

        dragStartRotation = rootObject.transform.rotation;
        HandleRotation(startAngle);
    }

    private void CreateRotationAxis(int linkIndex)
    {
        Quaternion invertedViewingAngle = Quaternion.Inverse(Quaternion.Euler(viewingAngle));
        Quaternion invertedRotationAxis = Quaternion.Euler(rotationAxis_CameraRelative) * invertedViewingAngle;

        if (rotationAxis != null)
        {
            DestroyImmediate(rotationAxis);
        }

        rotationAxis = new GameObject(AutoRig8DC4Puzzle_DataSO.RotationAxisName(linkIndex));
        rotationAxis.transform.position = rootObject.transform.position;
        rotationAxis.transform.rotation = invertedRotationAxis;

        rotationAxis.transform.parent = rootBone.transform.parent;
        rotationAxis.transform.SetAsFirstSibling();

        rotationAxis.transform.localScale = Vector3.one;

        rotationTarget =
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(rotationAxis.transform, AutoRig8DC4Puzzle_DataSO.RotationTargetName(linkIndex));

        var dRay = rotationAxis.AddComponent<DebugRayDrawer_Editor>();
        dRay.rayColor = Color.cyan;
    }

    private void Reset()
    {
        rotationAxis_CameraRelative = Vector3.forward;
        rootObject = gameObject;
    }
}
