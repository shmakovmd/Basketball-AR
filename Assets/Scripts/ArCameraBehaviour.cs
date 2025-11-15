using UnityEngine;

public class ArCameraBehaviour : MonoBehaviour
{
    private static readonly int StayInvisible = Animator.StringToHash("StayInvisible");

    // [SerializeField] private Transform ring;
    // [SerializeField] private GameObject tooCloseMessage;
    [SerializeField] private AudioClip tooCloseSound;
    [SerializeField] private GameObject ball;

    private AudioSource _audioSource;
    private Animator _ballAnimator;
    private BallAnimationEventHandler _ballAnimEventHandler;
    private bool _isTooCloseSoundPlayed;

    private void Start()
    {
        // if (!IsOwner) return;

        _audioSource = GetComponent<AudioSource>();
        _ballAnimator = ball.GetComponentInChildren<Animator>();
        _ballAnimEventHandler = ball.GetComponentInChildren<BallAnimationEventHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (!IsOwner) return;
        if (!other.CompareTag("Too Close Trigger")) return;

        _audioSource.PlayOneShot(tooCloseSound);
        // tooCloseMessage.SetActive(true);
        if (!_ballAnimEventHandler.IsDestructiveFadingOut && !_ballAnimEventHandler.IsDelayBeforeDestructiveFadingOut)
            _ballAnimator.Play("BallFadeOut");
        ball.GetComponent<BallBehaviour>().IsTooClose = true;
        _ballAnimator.SetBool(StayInvisible, true);
    }

    private void OnTriggerExit(Collider other)
    {
        // if (!IsOwner) return;
        if (!other.CompareTag("Too Close Trigger")) return;

        // tooCloseMessage.SetActive(false);
        if (!_ballAnimEventHandler.IsDestructiveFadingOut && !_ballAnimEventHandler.IsDelayBeforeDestructiveFadingOut)
            _ballAnimator.Play("BallFadeIn");
        ball.GetComponent<BallBehaviour>().IsTooClose = false;
        _ballAnimator.SetBool(StayInvisible, false);
    }
}