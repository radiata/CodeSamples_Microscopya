using System.Collections;
using UnityEngine;

public class SimpleTRNA : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;
    [SerializeField] private FloatInAndFadeIn floatInAndFadeIn;
    [SerializeField] private FloatAwayAndFadeOut floatAwayAndFadeOut;

    [SerializeField] private SpriteRenderer tRNASprite;
    [SerializeField] private SpriteRenderer aminoAcidSprite;
    [SerializeField] private AlphaBlendAnimator primaryAlphaBlendAnimator;
    [SerializeField] private AlphaBlendAnimator secondaryAlphaBlendAnimator;

    private bool inUse = false;

    public bool isInUse => inUse;

    public float SpawnTimeToDestination => floatInAndFadeIn.TimeToDestination;

    private void Start()
    {
        Initialize();
        floatAwayAndFadeOut.OnLerpToLocationCompleted += FinishDespawn;
    }

    private void Initialize()
    {
        tRNASprite.enabled = false;
        primaryAlphaBlendAnimator.UpdateAlphaBlend(0);

        if (aminoAcidSprite != null)
        {
            aminoAcidSprite.enabled = false;
            secondaryAlphaBlendAnimator.UpdateAlphaBlend(0);
        }
    }

    public void Spawn()
    {
        inUse = true;
        Initialize();
        StartCoroutine(SpawnDelay());
    }

    public void Despawn()
    {
        secondaryAlphaBlendAnimator.UpdateAlphaBlend(0);
        floatAwayAndFadeOut.StartBehaviour_NonDestructive();
    }

    private void FinishDespawn()
    {
        inUse = false;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        floatAwayAndFadeOut.OnLerpToLocationCompleted -= FinishDespawn;
    }

    private IEnumerator SpawnDelay()
    {
        yield return null;

        tRNASprite.enabled = true;

        if (aminoAcidSprite != null)
        {
            aminoAcidSprite.enabled = true;
        }

        floatInAndFadeIn.StartBehaviour();
    }
}
