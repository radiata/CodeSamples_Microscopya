using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class NavMeshAgentSettings
{
    //Nav Mesh Agent Settings
    [SerializeField] private AgentType agentType;
    [SerializeField] private float baseOffset;
    //Steering
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private bool autoBraking;
    //Obstacle Avoidance
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private ObstacleAvoidanceType obstacleAvoidanceType;
    [SerializeField] private int avoidancePriority;
    //Path Finding
    [SerializeField] private bool autoTraverseOffMeshLink;
    [SerializeField] private bool autoRepath;
    [SerializeField] private LayerMask areaMask;
    //Hidden Settings
    [SerializeField] private bool updateRotation;

    public static string AgentTypeVariableName => nameof(agentType);
    public static string BaseOffsetVariableName => nameof(baseOffset);
    public static string SpeedVariableName => nameof(speed);
    public static string AngularSpeedVariableName => nameof(angularSpeed);
    public static string AccelerationVariableName => nameof(acceleration);
    public static string StoppingDistanceVariableName => nameof(stoppingDistance);
    public static string AutoBrakingVariableName => nameof(autoBraking);
    public static string RadiusVariableName => nameof(radius);
    public static string HeightVariableName => nameof(height);
    public static string QualityVariableName => nameof(obstacleAvoidanceType);
    public static string PriorityVariableName => nameof(avoidancePriority);
    public static string AutoTraverseOffMeshLinkVariableName => nameof(autoTraverseOffMeshLink);
    public static string AutoRepathVariableName => nameof(autoRepath);
    public static string AreaMaskVariableName => nameof(areaMask);
    public static string UpdateRotationVariableName => nameof(updateRotation);

    public void ApplyAgentSettings(NavMeshAgent navMeshAgent)
    {
        navMeshAgent.agentTypeID = (int)agentType;
        navMeshAgent.baseOffset = baseOffset;
        navMeshAgent.speed = speed;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.autoBraking = autoBraking;
        navMeshAgent.radius = radius;
        navMeshAgent.height = height;
        navMeshAgent.obstacleAvoidanceType = obstacleAvoidanceType;
        navMeshAgent.avoidancePriority = avoidancePriority;
        navMeshAgent.autoTraverseOffMeshLink = autoTraverseOffMeshLink;
        navMeshAgent.autoRepath = autoRepath;
        navMeshAgent.areaMask = areaMask;
        navMeshAgent.updateRotation = updateRotation;
    }
}


