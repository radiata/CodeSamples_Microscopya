using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Build8DC4PuzzleFromRig : MonoBehaviour
{
    public int DEBUG_DisabledIndex = 3;

    [SerializeField] private GameObject rigged8DC4_prefab;
    private GameObject rigged8DC4_instance;
    private List<ProteinLink_ColliderSet> colliderSets;

    [SerializeField] private ProteinPuzzle_Rotator proteinRotator;

    [SerializeField] private Transform characterInteractionPosition;
    [SerializeField] private GameObject navigationObject_Root;

    [SerializeField] private AutoRig8DC4Puzzle_DataSO autoRigData;

    [Header("Materials")]
    [SerializeField] private Material partialLinkMaterial;
    [SerializeField] private Material inactiveLinkMaterial;
    [SerializeField] private Material activeLinkMaterial;
    [SerializeField] private Material solvedLinkMaterial;
    [SerializeField] private Material echoMaterial;

    [Header("Outline Modifiers")]
    [SerializeField] private Gradient activeOutlineGradient;


    private string proteinRotationPuzzle_Name = "Protein Rotation Puzzle";
    private GameObject proteinRotationPuzzle_Root;

    private PuzzleManager puzzleManager;

    private List<ProteinLink> proteinLinks;

    [ContextMenu("Build Auto Rig & Puzzle")]
    private void BuildPuzzle()
    {
        puzzleManager = GetComponentInChildren<PuzzleManager>();
        if (puzzleManager != null)
        {
            DestroyImmediate(puzzleManager.gameObject);
        }

        ProteinRotationPuzzle proteinRotationPuzzle = GetComponentInChildren<ProteinRotationPuzzle>();
        if (proteinRotationPuzzle != null)
        {
            DestroyImmediate(proteinRotationPuzzle.gameObject);
        }

        proteinRotationPuzzle_Root =
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(transform, proteinRotationPuzzle_Name);

        proteinRotationPuzzle = proteinRotationPuzzle_Root.AddComponent<ProteinRotationPuzzle>();

        SetupPuzzleManager(proteinRotationPuzzle);

        rigged8DC4_instance = Instantiate(rigged8DC4_prefab, proteinRotationPuzzle_Root.transform);

        AutoRig8DC4Puzzle autoRigger = rigged8DC4_instance.transform.GetComponent<AutoRig8DC4Puzzle>();
        autoRigger.ExecuteAutoRig();
        colliderSets = autoRigger.ColliderSets;

        rigged8DC4_instance.name = "Interactable Protein - Rigged 8DC4";

        BuildProteinLinks(proteinRotationPuzzle);
        proteinRotationPuzzle.SetProteinLinkList(proteinLinks);

        proteinRotationPuzzle.SetVariables(PuzzleKey.RoughER_ProteinRotation, FindPointerInteractable(), puzzleManager
            , proteinRotationPuzzle_Root, partialLinkMaterial, inactiveLinkMaterial, activeLinkMaterial, solvedLinkMaterial
            , activeOutlineGradient, proteinRotator.gameObject);

        proteinRotator.AssignVariables(proteinRotationPuzzle_Root, proteinRotationPuzzle, proteinRotationPuzzle_Root);
    }

    private void BuildProteinLinks(ProteinRotationPuzzle proteinRotationPuzzle)
    {
        GameObject rigged8DC4_instance_echoes =
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(rigged8DC4_instance.transform, "Echo - Mesh Renderers");

        GameObject proteinLinks_Root =
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(proteinRotationPuzzle_Root.transform, "Interactable Protein - Protein Links");

        GameObject instanceRootObj = rigged8DC4_instance.transform.Find(AutoRig8DC4Puzzle_DataSO.ChainTargets_ParentName).gameObject;

        List<GameObject> childObjects = GameObjectUtilities.GetAllChildGameObjects(instanceRootObj);
        List<GameObject> rotationBases = new List<GameObject>();

        for (int i = 0; i < childObjects.Count; i++)
        {
            if (childObjects[i].name.Contains(AutoRig8DC4Puzzle_DataSO.RotationBaseIdentifier))
            {
                rotationBases.Add(childObjects[i]);
            }
        }

        proteinLinks = new List<ProteinLink>();

        for (int i = 0; i < rotationBases.Count; i++)
        {
            rotationBases[i].transform.SetParent(proteinLinks_Root.transform);
            ProteinLink proteinLink = rotationBases[i].AddComponent<ProteinLink>();
            proteinLinks.Add(proteinLink);

            BuildInteractionElements(rotationBases[i], proteinLink, i);

            AddSkinnedMeshRenderer(proteinLink, i);
            AddEchoObject(proteinLink, i, rigged8DC4_instance_echoes.transform);
            AddViewingAngle(proteinLink, i);
            AddSolvedRotation(rotationBases[i], proteinLink);
            rigged8DC4_instance.SetActive(true);
            AddRig(proteinLink, i);
            AddRootBoneRotations(proteinLink, i);
            SetStartAngle(proteinLink, i);
            rigged8DC4_instance.SetActive(false);

            if(i >= DEBUG_DisabledIndex)
            {
                proteinLinks[i].DEBUG_DisableRig = true;
            }
        }
    }

    private void BuildInteractionElements(GameObject rotationBase, ProteinLink proteinLink, int linkIndex)
    {
        ProteinLink_ColliderSet colliderSet = colliderSets[linkIndex];

        foreach (Collider collider in colliderSet.Colliders)
        {
            collider.providesContacts = false;
            collider.layerOverridePriority = 0;
            collider.includeLayers = 0;
            collider.excludeLayers = 0;
            collider.enabled = false;

            collider.gameObject.AddComponent<DraggablePuzzlePiece_Handler>()
                .SetDraggablePuzzlePiece(rotationBase);
        }

        proteinLink.SetColliderSet(colliderSet);
    }

    private void AddSkinnedMeshRenderer(ProteinLink proteinLink, int index)
    {
        List<string> meshRendererNames = autoRigData.ProteinLinks[index].BoneMeshRenderers;

        List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();

        List<GameObject> childGameObjects = GameObjectUtilities.GetAllChildGameObjects(gameObject);

        for (int i = 0; i < meshRendererNames.Count; i++)
        {
            foreach (GameObject childObject in childGameObjects)
            {
                if (childObject.name.Equals(meshRendererNames[i]) == true)
                {
                    SkinnedMeshRenderer skinnedMeshRenderer = childObject.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer != null)
                    {
                        skinnedMeshRenderers.Add(skinnedMeshRenderer);
                        break;
                    }
                }
            }
        }

        proteinLink.SetSkinnedMeshRenderer(skinnedMeshRenderers);
    }

    private void AddEchoObject(ProteinLink proteinLink, int index, Transform echoObjects)
    {
        GameObject echoObject = 
            GameObjectUtilities.CreateGameObjectAtLocalOrigin(echoObjects, $"Link_{index:00} - Echoes");

        proteinLink.SetEchoObject(echoObject);

        foreach(SkinnedMeshRenderer skinnedMeshRenderer in proteinLink.SkinnedMeshRenderers)
        {
            GameObject echoRenderer = 
                GameObjectUtilities.CreateGameObjectAtLocalOrigin(skinnedMeshRenderer.transform, skinnedMeshRenderer.gameObject.name + " - Echo");

            echoRenderer.transform.parent = echoObject.transform;

            echoRenderer.AddComponent<MeshFilter>()
                .mesh = skinnedMeshRenderer.sharedMesh;

            MeshRenderer meshRenderer = echoRenderer.AddComponent<MeshRenderer>();

            meshRenderer.materials = new Material[] { echoMaterial };
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        }
    }

    private void AddViewingAngle(ProteinLink proteinLink, int index)
    {
        (Vector3, float) viewingAngleData;
        viewingAngleData.Item1 = new Vector3(autoRigData.GetViewingData(index).x, autoRigData.GetViewingData(index).y, autoRigData.GetViewingData(index).z);
        viewingAngleData.Item2 = autoRigData.GetViewingData(index).w;

        proteinLink.SetViewingData(viewingAngleData.Item1, viewingAngleData.Item2);
    }

    private void AddSolvedRotation(GameObject gameObject, ProteinLink proteinLink)
    {
        proteinLink.SetSolvedRotation(gameObject.transform.rotation);
    }

    private void AddRig(ProteinLink proteinLink, int linkIndex)
    {
        string rigName = AutoRig8DC4Puzzle_DataSO.RotationRigName(linkIndex);
        Rig rig = GameObject.Find(rigName).GetComponent<Rig>();
        if (rig == null || rig.gameObject.name != rigName)
        {
            Debug.LogError("Couldn't find correct Rig?");
        }

        ChainIKConstraint[] chainIKConstraints = new ChainIKConstraint[autoRigData.ProteinLinks[linkIndex].RotationChains.Count];
        for (int chainIndex = 0; chainIndex < chainIKConstraints.Length; chainIndex++)
        {
            string chainConstraintName = AutoRig8DC4Puzzle_DataSO.ChainIKConstraintName(linkIndex, chainIndex);

            chainIKConstraints[chainIndex] = GameObject.Find(chainConstraintName).GetComponent<ChainIKConstraint>();
            if (chainIKConstraints[chainIndex] == null || chainIKConstraints[chainIndex].gameObject.name != chainConstraintName)
            {
                Debug.LogError("Couldn't find correct Chain IK Constraint?");
            }
        }

        string OverrideTransformConstraintName = AutoRig8DC4Puzzle_DataSO.OverrideTransformConstraintName(linkIndex);
        OverrideTransform overrideTransform = GameObject.Find(OverrideTransformConstraintName).GetComponent<OverrideTransform>();
        if (overrideTransform == null || overrideTransform.gameObject.name != OverrideTransformConstraintName)
        {
            Debug.LogError("Couldn't find correct Override Transform?");
        }


        proteinLink.SetRotationRig(rig, chainIKConstraints, overrideTransform);
        proteinLink.SetRigDefaultValues();
    }

    private void AddRootBoneRotations(ProteinLink proteinLink, int linkIndex, int chainIndex = 0)
    {
        GameObject boneObj = null;

        List<GameObject> childGameObjects = GameObjectUtilities.GetAllChildGameObjects(gameObject);

        foreach (var childGameObject in childGameObjects)
        {
            if (childGameObject.name == autoRigData.ProteinLinks[linkIndex].RotationChains[chainIndex].LeadBone)
            {
                boneObj = childGameObject;
                break;
            }
        }

        if (boneObj == null)
        {
            Debug.LogError($"Bone: '{autoRigData.ProteinLinks[linkIndex].RotationChains[chainIndex].LeadBone}' not found for proteinLink {linkIndex}");

        }

        proteinLink.SetBone(boneObj);
        proteinLink.SetOverrideTransformRotation(linkIndex);
    }

    private void SetStartAngle(ProteinLink proteinLink, int index)
    {
        proteinLink.SetStartAngle(autoRigData.ProteinLinks[index].startAngle);
    }

    private void SetupPuzzleManager(ProteinRotationPuzzle proteinRotationPuzzle)
    {
        GameObject puzzleManagerObject =
                GameObjectUtilities.CreateGameObjectAtLocalOrigin(transform, "Puzzle Manager");
        puzzleManagerObject.transform.SetSiblingIndex(0);
        puzzleManager = puzzleManagerObject.AddComponent<PuzzleManager>();

        BasePuzzle puzzle = proteinRotationPuzzle;
        PuzzleCamera camera = FindPuzzleCamera();
        FacingDirection facingDirection = FacingDirection.right;

        Transform interactionPosition = characterInteractionPosition;
        GameObject navigationObject = navigationObject_Root;

        CharacterFacingReporter facingReporter = GameObject.Find("Character Facing Reporter").GetComponent<CharacterFacingReporter>();
        if (facingReporter == null)
        {
            Debug.LogWarning("Missing Facing Reporter");
        }

        NavMeshAgent navAgent = GameObject.Find("Character Root").GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogWarning("Missing NavAgent");
        }

        puzzleManager.SetVariables(puzzle, camera, facingReporter, facingDirection, interactionPosition, navigationObject, navAgent);

        AssignToPointerInteractable(puzzleManager);
    }

    private PuzzleCamera FindPuzzleCamera()
    {
        List<GameObject> childGameObjects = GameObjectUtilities.GetAllChildGameObjects(gameObject);

        foreach (GameObject childGameObject in childGameObjects)
        {
            PuzzleCamera puzzleCamera = childGameObject.GetComponent<PuzzleCamera>();

            if (puzzleCamera != null)
            {
                return puzzleCamera;
            }
        }

        return null;
    }

    private void AssignToPointerInteractable(PuzzleManager puzzleManager)
    {
        List<GameObject> childGameObjects = GameObjectUtilities.GetAllChildGameObjects(gameObject);

        foreach (GameObject childGameObject in childGameObjects)
        {
            PuzzleManager_PointerInteractable puzzleManager_PointerInteractable = childGameObject.GetComponent<PuzzleManager_PointerInteractable>();

            if (puzzleManager_PointerInteractable != null)
            {
                puzzleManager_PointerInteractable.SetPuzzleManager(puzzleManager);
                return;
            }
        }
    }

    private PuzzleManager_PointerInteractable FindPointerInteractable()
    {
        List<GameObject> childGameObjects = GameObjectUtilities.GetAllChildGameObjects(gameObject);

        foreach (GameObject childGameObject in childGameObjects)
        {
            PuzzleManager_PointerInteractable puzzleManager_PointerInteractable = childGameObject.GetComponent<PuzzleManager_PointerInteractable>();

            if (puzzleManager_PointerInteractable != null)
            {
                return puzzleManager_PointerInteractable;
            }
        }

        Debug.LogWarning("Missing PuzzleManager_PointerInteractable");
        return null;
    }
}
