using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TRNA_NegativeFeedback : Base_NegativeFeedback
{
    [SerializeField] private SoundEffect incorrect_Sound;
    [SerializeField] private float durationMove = .3f;
    [SerializeField] private float durationShake = .5f;

    private Transform transformToMove;
    private Transform transformToShake;

    private Tweener moveTween;

    private Tweener shakeTween;
    [SerializeField] private Vector3 shakeStrength = new Vector3(1, 1, 0);

    private Coroutine waitForTweens;

    public override void ExecuteNegativeFeedback()
    {
        AudioController.Instance.PlaySoundEffect(incorrect_Sound, false);

        StartNegativeFeedback();

        waitForTweens = StartCoroutine(RunTweens());
    }

    private IEnumerator RunTweens()
    {
        shakeTween = transform.parent.DOShakePosition(durationShake, shakeStrength);
        shakeTween.Play();
        
        moveTween = transform.DOLocalMove(Vector3.zero, durationMove, false);
        moveTween.Play();

        yield return moveTween.WaitForCompletion();
        yield return shakeTween.WaitForCompletion();

        moveTween.Kill();
        shakeTween.Kill();

        EndNegativeFeedback();

        waitForTweens = null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        CancelInvoke();
    }

}
