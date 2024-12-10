using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CharacterPathFinder
{
    public delegate void GetPathCompleted(NavMeshPath navMeshPath);
    public static event GetPathCompleted OnGetPathCompleted;


    private CancellationTokenSource pathFindingCancellationToken = null;

    public void GetPath(NavMeshAgent navMeshAgent, Vector3 destination, bool ignorePathingLimits, Camera referenceCamera = null, float? maxPathingDistance = null)
    {
        NavMeshPath path;
        EvaluatePath(navMeshAgent, destination, ignorePathingLimits, out path, referenceCamera, maxPathingDistance);
        OnGetPathCompleted?.Invoke(path);
    }

    public bool EvaluatePath(NavMeshAgent navMeshAgent, Vector3 destination, bool ignorePathingLimits, out NavMeshPath navMeshPath, Camera referenceCamera = null, float? maxPathingDistance = null)
    {
        Task_Utilities.RefreshToken(ref pathFindingCancellationToken);

        Task<NavMeshPath> findPathTask;
        if (ignorePathingLimits == true
            || (referenceCamera == null && maxPathingDistance == null))
        {
            findPathTask = FindPathAsync(navMeshAgent, destination, pathFindingCancellationToken.Token);
        }
        else if (referenceCamera != null && maxPathingDistance == null)
        {
            findPathTask = FindPathAsync(navMeshAgent, destination, referenceCamera, pathFindingCancellationToken.Token);
        }
        else if (referenceCamera == null && maxPathingDistance != null)
        {
            findPathTask = FindPathAsync(navMeshAgent, destination, maxPathingDistance.Value, pathFindingCancellationToken.Token);
        }
        else
        {
            findPathTask = FindPathAsync(navMeshAgent, destination, referenceCamera, maxPathingDistance.Value, pathFindingCancellationToken.Token);
        }

        findPathTask.Wait();

        if(findPathTask.Result == null)
        {
            navMeshPath = null;
            return false;
        }

        navMeshPath = findPathTask.Result;
        return true;
    }

    public void PathCompleted(NavMeshPath navMeshPath)
    {
        OnGetPathCompleted?.Invoke(navMeshPath);
    }

    #region FindPathAsync(...)
    private async Task<NavMeshPath> FindPathAsync(NavMeshAgent navMeshAgent, Vector3 destination, Camera camera, float maxPathingDistance, CancellationToken cancellationToken)
    {
        NavMeshPath navMeshPath = FindPathAsync(navMeshAgent, destination, cancellationToken).Result;

        float? checkPathDistanceTask = await CheckPathDistanceAsync(navMeshPath, maxPathingDistance, cancellationToken);
        Task<bool> checkPathOnScreenTask = CheckPathOnScreenAsync(navMeshPath, camera, cancellationToken);

        while (checkPathOnScreenTask.IsCompleted == false)
        {
            if (cancellationToken.IsCancellationRequested == true)
            {
                return null;
            }
            await Task.Yield();
        }

        if (checkPathDistanceTask == null || checkPathOnScreenTask.Result == false)
        {
            return null;
        }

        return navMeshPath;
    }

    private async Task<NavMeshPath> FindPathAsync(NavMeshAgent navMeshAgent, Vector3 destination, Camera camera, CancellationToken cancellationToken)
    {
        NavMeshPath navMeshPath = FindPathAsync(navMeshAgent, destination, cancellationToken).Result;

        Task<bool> checkPathOnScreenTask = CheckPathOnScreenAsync(navMeshPath, camera, cancellationToken);

        while (checkPathOnScreenTask.IsCompleted == false)
        {
            if (cancellationToken.IsCancellationRequested == true)
            {
                return null;
            }
            await Task.Yield();
        }

        if (checkPathOnScreenTask.Result == false)
        {
            return null;
        }

        return navMeshPath;
    }

    private async Task<NavMeshPath> FindPathAsync(NavMeshAgent navMeshAgent, Vector3 destination, float maxPathingDistance, CancellationToken cancellationToken)
    {
        NavMeshPath navMeshPath = FindPathAsync(navMeshAgent, destination, cancellationToken).Result;

        float? checkPathDistanceTask = await CheckPathDistanceAsync(navMeshPath, maxPathingDistance, cancellationToken);

        if (checkPathDistanceTask == null)
        {
            return null;
        }

        return navMeshPath;
    }

    private async Task<NavMeshPath> FindPathAsync(NavMeshAgent navMeshAgent, Vector3 destination, CancellationToken cancellationToken)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        navMeshAgent.CalculatePath(destination, navMeshPath);

        if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
        {
            return null;
        }

        return navMeshPath;
    }
    #endregion

    private async Task<float?> CheckPathDistanceAsync(NavMeshPath navMeshPath, float maxPathingDistance, CancellationToken cancellationToken)
    {
        float? pathDistanceCheckTask = NavMeshPathUtilities.GetPathRemainingDistance(navMeshPath);
        
        if (pathDistanceCheckTask == null
            || pathDistanceCheckTask.Value > maxPathingDistance)
        {
            return null;
        }

        return pathDistanceCheckTask.Value;
    }

    private async Task<bool> CheckPathOnScreenAsync(NavMeshPath navMeshPath, Camera camera, CancellationToken cancellationToken)
    {
        Task<bool> pathOnScreenCheckTask = NavMeshPathUtilities.GetCornersInCameraViewAsync(navMeshPath, camera, cancellationToken);

        if (pathOnScreenCheckTask.IsCompleted == false)
        {
            if (cancellationToken.IsCancellationRequested == true)
            {
                return false;
            }
            await Task.Yield();
        }

        if (pathOnScreenCheckTask.Result == false)
        {
            return false;
        }

        return true;
    }
}
