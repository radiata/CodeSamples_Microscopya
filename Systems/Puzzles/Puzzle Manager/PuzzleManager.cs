using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleCamera puzzleCamera;
    [SerializeField] private BasePuzzle puzzle;

    [SerializeField] private CharacterFacingReporter characterFacingReporter;
    [SerializeField] private FacingDirection characterFacingDirection;

    [SerializeField] private CharacterNavigationManager characterNavigationManager;

    [SerializeField] private Transform characterInteractionPosition;
    private Vector3 interactionPositionDestination;

    [SerializeField] private GameObject navigationObject_RootObject;
    [SerializeField, HideInInspector] private NavigationObject interactionPositionNavigationObject;

    [SerializeField] private NavMeshAgent targetNavMeshAgent;

    [SerializeField] private bool debug_IsolatedPuzzle = false;

    private Coroutine pathCheckRoutine;

    private bool puzzleActive = false;
    private bool lockedInPuzzleMode = false;

    public bool PuzzleActive => puzzleActive;

    public void SetVariables(BasePuzzle basePuzzle, PuzzleCamera puzzleCamera, CharacterFacingReporter characterFacingReporter, FacingDirection facingDirection,
        Transform characterInteractionPosition, GameObject navigationObject_Root, NavMeshAgent navMeshAgent)
    {
        this.puzzle = basePuzzle;
        this.puzzleCamera = puzzleCamera;
        this.characterFacingReporter = characterFacingReporter;
        this.characterFacingDirection = facingDirection;
        this.characterInteractionPosition = characterInteractionPosition;
        this.navigationObject_RootObject = navigationObject_Root;
        this.targetNavMeshAgent = navMeshAgent;
    }

    public void Navigate()
    {
        if (lockedInPuzzleMode == true || puzzleActive == true)
        {
            return;
        }

        if (puzzle.NavigateAvailable == false)
        {
            return;
        }

        ClearPath();

        if (debug_IsolatedPuzzle == true)
        {
            EnterPuzzle_Isolated();
            return;
        }

        NavMeshPath navMeshPath_Puzzle;
        bool isValid_PathPuzzle = characterNavigationManager.EvaluatePath(interactionPositionDestination, false, out navMeshPath_Puzzle);

        if (isValid_PathPuzzle == false)
        {
            return;
        }

        characterNavigationManager.OnNavigate(navMeshPath_Puzzle);

        pathCheckRoutine = StartCoroutine(PathCompleteCheck());

        NavigationObject.OnNavigate += HandleNavigationEvents;
    }

    public void LockInPuzzleMode(bool isLocked)
    {
        lockedInPuzzleMode = isLocked;
    }

    private void HandleNavigationEvents(Vector3 destination, bool ignorePathingLimits)
    {
        ClearPath();
    }

    private void HandlePuzzleExit(Vector3 destination, bool ignorePathingLimits)
    {
        ExitPuzzle();
    }

    private void HandlePuzzleComplete(PuzzleKey puzzleKey)
    {
        if (puzzle.PuzzleKey == puzzleKey)
        {
            PuzzleComplete();
        }
    }

    private void ClearPath()
    {
        NavigationObject.OnNavigate -= HandleNavigationEvents;

        if (pathCheckRoutine != null)
        {
            StopCoroutine(pathCheckRoutine);
            pathCheckRoutine = null;
        }
    }

    private void EnterPuzzle()
    {
        NavigationObject.OnNavigate -= HandlePuzzleExit;
        NavigationObject.OnNavigate += HandlePuzzleExit;

        if (puzzleActive == false)
        {
            CameraZoomController.CacheZoomValue();
            puzzle.ActivatePuzzle();
            puzzleCamera.Activate();
            characterFacingReporter.ChangeFacingDirection(characterFacingDirection);

            puzzleActive = true;
        }
    }

    public void ExitPuzzle()
    {
        if (lockedInPuzzleMode == true)
        {
            return;
        }
        NavigationObject.OnNavigate -= HandlePuzzleExit;

        if (puzzleActive == true)
        {
            CameraZoomController.ResetZoomToLastCachedValue();
        }

        puzzle.DeactivatePuzzle();
        puzzleCamera.Deactivate();
        puzzleActive = false;
    }

    private void PuzzleComplete()
    {
        ExitPuzzle();
        puzzle.DisablePuzzlePointerHandler();
    }

    private IEnumerator PathCompleteCheck()
    {
        while (targetNavMeshAgent.remainingDistance != 0)
        {
            yield return null;
        }

        EnterPuzzle();
        ClearPath();
    }


    private void OnEnable()
    {
        BasePuzzle.OnPuzzleCompleted += HandlePuzzleComplete;
    }

    private void OnDisable()
    {
        ClearPath();
        ExitPuzzle();
        BasePuzzle.OnPuzzleCompleted -= HandlePuzzleComplete;
        NavigationObject.OnNavigate -= HandlePuzzleExit;
    }

    private void Awake()
    {
        if (interactionPositionNavigationObject != null
            && characterInteractionPosition != null)
        {
            var worldPosition = characterInteractionPosition.position;
            worldPosition.z = interactionPositionNavigationObject.ZPosition;

            interactionPositionDestination = interactionPositionNavigationObject.FindDestination(worldPosition);
        }

        puzzle.InitializePuzzle_Awake();
    }

    private void Start()
    {
        puzzle.InitializePuzzle_Start();
    }

    private void OnValidate()
    {
        if (navigationObject_RootObject != null)
        {
            interactionPositionNavigationObject = navigationObject_RootObject.GetComponentInChildren<NavigationObject>();
        }
    }

    private void EnterPuzzle_Isolated()
    {
        if (puzzleActive == false)
        {
            CameraZoomController.CacheZoomValue();
            puzzle.ActivatePuzzle();
            puzzleCamera.Activate();
            puzzleActive = true;
        }
    }
}
