using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigationVelocityController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private CharacterNavigation_Utilities characterNavigation_Utilities;

    public delegate void CharacterBrakeEvent();
    public static event CharacterBrakeEvent OnCharacterBrake;

    private float agentMaxSpeed = 11f;
    private float agentMinSpeed = 4f;

    private float agentBrakeSpeedRequirement = 9f;
    private bool canBrake = false;

    private float requiredDistanceForMaxSpeed = 10f;

    private float defaultStoppingDistance = 0f;
    private float brakeStartDistance = 5f;
    private float brakeTime = .75f;

    private float velocityPreservation = .5f;
    private float testVelocity = 0f;

    public void InitializeVariables(float agentMaxSpeed, float agentMinSpeed, float agentBrakeSpeedRequirement, float requiredDistanceForMaxSpeed,
        float defaultStoppingDistance, float brakeStartDistance, float brakeTime, float testVelocity)
    {
        this.agentMaxSpeed = agentMaxSpeed;
        this.agentMinSpeed = agentMinSpeed;
        this.agentBrakeSpeedRequirement = agentBrakeSpeedRequirement;
        this.requiredDistanceForMaxSpeed = requiredDistanceForMaxSpeed;
        this.defaultStoppingDistance = defaultStoppingDistance;
        this.brakeStartDistance = brakeStartDistance;
        this.brakeTime = brakeTime;
        this.testVelocity = testVelocity;
    }

    private void FixedUpdate()
    {
        if (navMeshAgent.velocity.sqrMagnitude == 0)
        {
            return;
        }

        if (canBrake && navMeshAgent.remainingDistance < brakeStartDistance)
        {
            OnCharacterBrake?.Invoke();
            navMeshAgent.speed = Mathf.SmoothDamp(navMeshAgent.speed, 0, ref testVelocity, brakeTime, agentMaxSpeed, Time.fixedDeltaTime);
        }
    }

    public void SetDestination(NavMeshPath navMeshPath)
    {
        if (navMeshPath.corners.Length > 1
            && characterNavigation_Utilities.isWorldPositionInCharacterForwardDirection(navMeshPath.corners[1]) == false)
        {
            navMeshAgent.velocity *= velocityPreservation;
        }

        float speedRatio = NavMeshPathUtilities.GetPathRemainingDistance(navMeshPath) / requiredDistanceForMaxSpeed;
        float newSpeed = Mathf.Clamp(speedRatio * agentMaxSpeed, agentMinSpeed, agentMaxSpeed);

        navMeshAgent.speed = newSpeed;
        canBrake = newSpeed >= agentBrakeSpeedRequirement ? true : false;
    }

    public void ResetStoppingDistance()
    {
        navMeshAgent.stoppingDistance = defaultStoppingDistance;
    }
}
