using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadedAudioAsset
{
    internal AudioType audioType;
    internal int audioID;

    internal AsyncOperationHandle<AudioClip> operationHandle;
    internal int referenceCount = 0;

    public bool isLoaded => operationHandle.IsDone;

    public LoadedAudioAsset(AsyncOperationHandle<AudioClip> operationHandle, AudioType audioType, int audioID)
    {
        this.operationHandle = operationHandle;

        this.audioType = audioType;
        this.audioID = audioID;

        referenceCount += 1;
    }
}
